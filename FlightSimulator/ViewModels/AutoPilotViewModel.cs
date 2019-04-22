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
    public delegate void TextBoxClear();
    class AutoPilotViewModel
    {
        public ApplicationModel AM { get; set; }
        public event TextBoxClear TextBoxClear;
        public bool isFirstLetter { get; set; }

        public AutoPilotViewModel(ApplicationModel AM)
        {
            this.AM = AM;
            isFirstLetter = true;
            AM.Io.colorEvent += () => { this.Color = Colors.White; };
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
                if (isFirstLetter)
                {
                    Color = Colors.Pink;
                    isFirstLetter = false;
                } 
            }
        }

        public System.Windows.Media.Color color;
        public System.Windows.Media.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
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
            TextBoxClear?.Invoke();
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
    }
}
