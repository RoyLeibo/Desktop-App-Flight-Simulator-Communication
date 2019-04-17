using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.ViewModels.Windows;
using FlightSimulator.Views.Windows;
using System.Windows;

namespace FlightSimulator.ViewModels
{
    public delegate void handler();
    public class FlightBoardViewModel : BaseNotify
    {
        public event handler ThisEvent;
        private ApplicationConnectModel ACM;
        public double Lon
        {
            get
            {
                return this.Lon;
            }
            set
            {
                this.Lon = value;
            }
        }

        public double Lat
        {
            get
            {
                return this.Lat;
            }
            set
            {
                this.Lat = value;
            }
        }

        #region Commands
        #region ClickCommand
        private ICommand _settingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new CommandHandler(() => SettingsClick()));
            }
        }
        private void SettingsClick()
        {
           var swvm = new SettingsWindowViewModel(ApplicationSettingsModel.Instance);
           var sw = new SettingsWindow() {DataContext = swvm};
           swvm.OnRequestClose += (s, e) => sw.Close();
           sw.Show();
        }
        #endregion
        #endregion

        #region Commands
        #region ClickCommand
        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandHandler(() => ConnectClick()));
            }
        }
        private void ConnectClick()
        {
            this.ACM = new ApplicationConnectModel(ApplicationSettingsModel.Instance);
            this.ACM.Io.ThisEvent += () =>
            {
                KeyValuePair<double, double> LonAndLat = this.ACM.Io.LonAndLat;
                this.Lon = LonAndLat.Key;
                this.Lat = LonAndLat.Value;
                ThisEvent?.Invoke();
            };
            ACM.Connect();
        }
        #endregion
        #endregion


    }
}