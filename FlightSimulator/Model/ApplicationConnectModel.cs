using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FlightSimulator.Model.Interface;

namespace FlightSimulator.Model
{
    class ApplicationConnectModel
    {
        ISettingsModel ASM;
        private Thread n_client;
        private TcpClient client;
        private TcpListener server;

        public ApplicationConnectModel(ISettingsModel ASM)
        {
            this.ASM = ASM;
        }

        public void Connect()
        {
            this.n_client = new Thread(new ThreadStart(Client));
            this.server = new TcpListener(IPAddress.Parse(ASM.FlightServerIP), ASM.FlightCommandPort);
            this.server.Start();
        }

        public void Client()
        {
            this.client = new TcpClient();
            client.Connect(ASM.FlightServerIP, ASM.FlightInfoPort);
            FlightSimulator.Model.IO.ReadDataFromSimulator(this.client);
        } 
    }
}
