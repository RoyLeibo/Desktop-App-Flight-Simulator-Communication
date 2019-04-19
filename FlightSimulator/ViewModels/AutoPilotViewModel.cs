using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;

namespace FlightSimulator.ViewModels
{
    public delegate string TextBoxData();
    public delegate void TextBoxClear();
    class AutoPilotViewModel
    {
        private ApplicationModel AM;
        public event TextBoxData TextBoxData;
        public event TextBoxClear TextBoxClear;

        public AutoPilotViewModel(ApplicationModel AM)
        {
            this.AM = AM;
        }
        #region Commands
        #region OkCommand
        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new CommandHandler(() => OkClick()));
            }
        }
        private void OkClick()
        {
            this.AM.Io.SendCommandToSimulator(TextBoxData?.Invoke()); 
        }
        #endregion

        #region ClearCommand
        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new CommandHandler(() => OnClear()));
            }
        }
        private void OnClear()
        {
            TextBoxClear?.Invoke();
        }
        #endregion
        #endregion
    }
}
