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
    public delegate void whiteScreen();
    public class IO
    {
        public event handler IoEvent;
        public event whiteScreen colorEvent;
        public Socket socket { get; set; }
        public TcpClient client { get; set; }
        public string command;
        public Thread newThread;

        private KeyValuePair<double, double> lonAndLat;
        public KeyValuePair<double, double> LonAndLat
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

        public IO(){
            this.LonAndLat = new KeyValuePair<double, double>(0, 0);
            this.LonAndLat = new KeyValuePair<double, double>(1, 1);
        }

        public void ReadDataFromSimulator(TcpListener client)
        {
            MessageBox.Show("Inside ReadDataFromSimulator");
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
                while (IsEndOfLine)
                {
                    EndOfLine = StringData.IndexOf('\n');
                    if (EndOfLine != -1)
                    {
                        Result += StringData.Substring(0, EndOfLine);
                        StringData.Remove(0, EndOfLine + 1);
                        ParseAndUpdate(StringData);
                        Result = "";
                        Remainder = StringData;
                    }
                    else
                    {
                        Remainder += StringData;
                        IsEndOfLine = false;
                    }
                }
            }
        }

        public void ParseAndUpdate(String StringData)
        {
            int StartOfLon = StringData.IndexOf(',') + 1;
            int EndOfLon = StringData.IndexOf(',', StartOfLon);
            double Lon = Double.Parse(StringData.Substring(StartOfLon, EndOfLon-StartOfLon));
            int StartOfLat = EndOfLon + 1;
            int EndOfLat = StringData.IndexOf(',', StartOfLat); 
            double Lat = Double.Parse(StringData.Substring(StartOfLat, EndOfLat-StartOfLat));
            this.LonAndLat = new KeyValuePair<double, double>(Lon, Lat);
            MessageBox.Show
                ("Lon: , Lat:");
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
            string[] CommandsArray = command.Split('\n');
            foreach (string Command in CommandsArray)
            {
                byte[] ByteArray = asen.GetBytes(command);
                stream.Write(ByteArray, 0, ByteArray.Length);
                stream.Flush();

            }
            this.colorEvent?.Invoke();
            this.newThread.Abort();
        }

        public void UpdateDataInSimulator(String DataName, double value)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stream = this.client.GetStream();
            byte[] ByteArray;
            String command;
            switch (DataName) {
                case "Ailron":
                    command = "set /controls/flight/aileron " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    break;
                case "Elevator":
                    command = "set /controls/flight/elevator " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    break;
                case "Throttle":
                    command = "set /controls/engines/current-engine/throttle " + value + "\r\n";
                    ByteArray = asen.GetBytes(command);
                    break;
                case "Rudder":
                    command = "set /controls/flight/rudder " + value + "\r\n"; 
                    ByteArray = asen.GetBytes(command);
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
