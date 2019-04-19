using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    public delegate void handler();
    public class IO
    {
        public event handler IoEvent;
        public Socket socket { get; set; }
        public TcpClient client { get; set; }
        
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
                while (IsEndOfLine)
                {
                    EndOfLine = StringData.IndexOf('\n');
                    if (EndOfLine != -1)
                    {
                        Result += StringData.Substring(0, EndOfLine);
                        StringData.Remove(EndOfLine + 1);
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
            double Lon = Double.Parse(StringData.Substring(StartOfLon, EndOfLon));
            double Lat = Double.Parse(StringData.Substring(EndOfLon + 1, StringData.IndexOf(',', StartOfLon)));
            this.LonAndLat = new KeyValuePair<double, double>(Lon, Lat); 
        }

        public void SendCommandToSimulator(String command)
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
        }

        public void UpdateDataInSimulator(String DataName, double value)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stream = this.client.GetStream();
            byte[] ByteArray;
            String command;
            switch (DataName) {
                case "Ailron":
                    command = "set /controls/flight/aileron " + value;
                    ByteArray = asen.GetBytes(command);
                    break;
                case "Elevator":
                    command = "set /controls/flight/elevator " + value;
                    ByteArray = asen.GetBytes(command);
                    break;
                case "Throttle":
                    command = "set /controls/engines/current-engine/throttle " + value;
                    ByteArray = asen.GetBytes(command);
                    break;
                case "Rudder":
                    command = "set /controls/flight/rudder " + value; 
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
