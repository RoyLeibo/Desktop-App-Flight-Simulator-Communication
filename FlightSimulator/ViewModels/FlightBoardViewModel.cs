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

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {



        public double Lon
        {
            get;
        }

        public double Lat
        {
            get;
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
                return _settingsCommand ?? (_settingsCommand = new CommandHandler(() => ConnectClick()));
            }
        }
        private void ConnectClick()
        {
            ApplicationConnectModel ACM = new ApplicationConnectModel(ApplicationSettingsModel.Instance);
            ACM.Connect();
        }
        #endregion
        #endregion
    }
}