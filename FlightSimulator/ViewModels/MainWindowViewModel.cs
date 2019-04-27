using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class MainWindowViewModel 
    {
        public MainWindowViewModel()
        {
                
        }
        private ICommand closeWindowCommand;

        public ICommand CloseWindowCommand
        {
            get
            {
                if (closeWindowCommand == null)
                {
                    closeWindowCommand = new RelayCommand(param => this.CloseWindow(), null);
                }
                return closeWindowCommand;
            }
        }

        private void CloseWindow()
        {
            //Do your operations
        }
    }
}
