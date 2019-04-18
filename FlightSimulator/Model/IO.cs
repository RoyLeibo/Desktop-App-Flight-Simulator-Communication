using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    public delegate void handler();
    public class IO
    {
        public event handler ThisEvent;
        public Socket socket { get; set; }
        
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
                ThisEvent?.Invoke();
            }
        }

        public void ReadDataFromSimulator(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
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
                recv = ns.Read(Buffer, 0, Buffer.Length);
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
            this.socket.Send(asen.GetBytes(command));
        }
    }
}
