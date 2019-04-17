using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator.ViewModels
{
    class ManualPilotViewModel
    {
        private double rudder;
        public double Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                MessageBox.Show(this.rudder.ToString());
            }
        }
    }
}
