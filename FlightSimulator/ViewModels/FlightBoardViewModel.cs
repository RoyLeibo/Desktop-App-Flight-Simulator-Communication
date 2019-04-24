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
using System.ComponentModel;

namespace FlightSimulator.ViewModels
{
    public delegate void handler(object sender, PropertyChangedEventArgs e);
    public class FlightBoardViewModel : BaseNotify
    {
        public event handler FVBMEvent;
        private ApplicationModel AM;
        public Point LonAndLat
        {
            get
            {
                return this.LonAndLat;
            }
            set
            {
                this.LonAndLat = value;
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
           var swvm = new SettingsWindowViewModel(ApplicationModel.Instance);
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
            this.AM = ApplicationModel.Instance;
            this.AM.Io.IoEvent += () =>
            {
                this.LonAndLat = this.AM.Io.LonAndLat;
                FVBMEvent?.Invoke(this, null);
            };
            AM.Connect();
        }
        #endregion
        #endregion


    }
}