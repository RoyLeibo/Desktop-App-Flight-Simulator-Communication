using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlightSimulator.Model.Interface;

namespace FlightSimulator.Model
{
    class ApplicationModel : ISettingsModel
    {
        private static ApplicationModel instance = null;
        private static readonly object padlock = new object();
        private Thread n_server;
        private TcpClient client;
        private TcpListener server;
        private IO io;
        public IO Io
        {
            get
            {
                return this.io;
            }
            set
            {
                this.io = value;
            }
        }

        #region ConnectionData
        public string FlightServerIP
        {
            get { return Properties.Settings.Default.FlightServerIP; }
            set { Properties.Settings.Default.FlightServerIP = value; }
        }
        public int FlightCommandPort
        {
            get { return Properties.Settings.Default.FlightCommandPort; }
            set { Properties.Settings.Default.FlightCommandPort = value; }
        }

        public int FlightInfoPort
        {
            get { return Properties.Settings.Default.FlightInfoPort; }
            set { Properties.Settings.Default.FlightInfoPort = value; }
        }
        #endregion

        ApplicationModel()
        {
        }

        public static ApplicationModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ApplicationModel();
                    }
                    return instance;
                }
            }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        public void ReloadSettings()
        {
            Properties.Settings.Default.Reload();
        }

        public void Connect()
        {
            this.io = new IO();
            this.n_server = new Thread(new ThreadStart(Server));
            this.client = new TcpClient();
            client.Connect(FlightServerIP, FlightInfoPort);
        }

        public void Server()
        {
            this.server = new TcpListener(IPAddress.Parse(FlightServerIP), FlightCommandPort);
            this.server.Start();
            this.io.socket = this.server.AcceptSocket();
            this.io.ReadDataFromSimulator(this.server);
        }
    }
}