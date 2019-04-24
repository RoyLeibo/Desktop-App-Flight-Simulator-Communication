using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator.Model
{
    public delegate void handler();
    public class IO
    {
        public event handler IoEvent;
        public Socket socket { get; set; }
        public TcpClient client { get; set; }
        public string command;
        public Thread newThread;

        private Point lonAndLat;
        public Point LonAndLat
        {
            get
            {
                return this.lonAndLat;
            }
            set
            {
                this.lonAndLat = value;
                IoEvent?.Invoke();
            }
        }

        public void ReadDataFromSimulator(TcpListener client)
        {
            byte[] Buffer = new byte[1024];
            int recv = 0;
            int EndOfLine = 0;
            String StringData = "";
            String Result = "";
            String Remainder = "";
            bool IsEndOfLine;
            while (true)
            {
                StringData = "";
                recv = this.socket.Receive(Buffer);
                StringData = Encoding.ASCII.GetString(Buffer, 0, recv);
                IsEndOfLine = true;
                Result = Remainder;
                EndOfLine = StringData.IndexOf('\n');
                if (EndOfLine != -1)
                {
                    Result += StringData.Substring(0, EndOfLine);
                    StringData.Remove(0, EndOfLine + 1);
                    ParseAndUpdate(Result);
                    Result = "";
                    Remainder = StringData;
                    Array.Clear(Buffer, 0, Buffer.Length);
                }
                else
                {
                    Remainder += StringData;
                    IsEndOfLine = false;
                }
            }
        }

        public void ParseAndUpdate(String StringData)
        {
            int StartOfLon = 0;
            int EndOfLon = StringData.IndexOf(',', StartOfLon);
            double Lon = Double.Parse(StringData.Substring(StartOfLon, EndOfLon - StartOfLon));
            int StartOfLat = EndOfLon + 1;
            int EndOfLat = StringData.IndexOf(',', StartOfLat);
            double Lat = Double.Parse(StringData.Substring(StartOfLat, EndOfLat - StartOfLat));
            this.LonAndLat = new Point(Lat, Lon);
        }

        public void SendCommandToSimulator(String command)
        {
            this.command = command;
            this.newThread = new Thread(new ThreadStart(FunctionInThread));
        }

        public void FunctionInThread()
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stream = this.client.GetStream();
            string[] CommandsArray = command.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string Command in CommandsArray)
            {
                byte[] ByteArray = asen.GetBytes(command);
                stream.Write(ByteArray, 0, ByteArray.Length);
                stream.Flush();

            }
            this.newThread.Abort();
        }

        public void UpdateDataInSimulator(String DataName, double value)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stream = this.client.GetStream();
            byte[] ByteArray;
            String command;
            switch (DataName)
            {
                case "Ailron":
                    command = "set /controls/flight/aileron " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    System.Diagnostics.Debug.WriteLine($"Ailron: {value}");
                    break;
                case "Elevator":
                    command = "set /controls/flight/elevator " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    System.Diagnostics.Debug.WriteLine($"Elevator: {value}");
                    break;
                case "Throttle":
                    command = "set /controls/engines/current-engine/throttle " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    System.Diagnostics.Debug.WriteLine($"Throttle: {value}");
                    break;
                case "Rudder":
                    command = "set /controls/flight/rudder " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    System.Diagnostics.Debug.WriteLine($"Rudder: {value}");
                    break;
                default:
                    ByteArray = asen.GetBytes("");
                    break;
            }
            stream.Write(ByteArray, 0, ByteArray.Length);
            stream.Flush();
        }
    }
}
