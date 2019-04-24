using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : BaseNotify
    {
        public ApplicationModel AM { get; set; }
        public bool isFirstLetter { get; set; }

        public AutoPilotViewModel(ApplicationModel AM)
        {
            this.AM = AM;
            isFirstLetter = true;
            AM.Io.colorEvent += () => {
                IsPink = false;
                NotifyPropertyChanged("IsPink");
                isFirstLetter = true;
            };
            this.IsPink = false;
        }

        private string textFromTextBox;
        public string TextFromTextBox
        {
            get
            {
                return this.textFromTextBox;
            }
            set
            {
                this.textFromTextBox = value;
                NotifyPropertyChanged("TextFromTexrBox");
                if (isFirstLetter)
                {
                    IsPink = true;
                    isFirstLetter = false;
                }
            }
        }
        private bool isPink;
        public bool IsPink
        {
            get
            {
                return isPink;
            }
            set
            {
                isPink = value;
                NotifyPropertyChanged("IsPink");
            }

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
            this.AM.Io.SendCommandToSimulator(this.TextFromTextBox);
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
            TextFromTextBox = "";
            NotifyPropertyChanged("TextFromTextBox");
        }
        #endregion

        #region ChangeColorCommand
        private ICommand _changeColorCommand;
        public ICommand ChangeColorCommand
        {
            get
            {
                return _changeColorCommand ?? (_changeColorCommand = new CommandHandler(() => ChangeColorFunc()));
            }
        }
        private void ChangeColorFunc()
        {

        }
        #endregion
        #endregion

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 1;
        }
    }
}
