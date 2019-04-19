using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlightSimulator.Model.EventArgs;
using FlightSimulator.Model;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class ManualPilotViewModel
    {
        private ApplicationModel VJEA;
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
                this.VJEA.Io.UpdateDataInSimulator("Rudder", value);
            }
        }

        private double throttle;
        public double Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                this.VJEA.Io.UpdateDataInSimulator("Throttle", value);
            }
        }

        private double elevator;
        public double Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                this.elevator = value;
                this.VJEA.Io.UpdateDataInSimulator("Elevator", value);
            }
        }

        private double ailron;
        public double Ailron
        {
            get
            {
                return this.ailron;
            }
            set
            {
                this.ailron = value;
                this.VJEA.Io.UpdateDataInSimulator("Ailron", value);
            }
        }

        public void UpdateData(object Sender, VirtualJoystickEventArgs VJEA)
        {
            this.Ailron = VJEA.Aileron;
            this.Elevator = VJEA.Elevator;
        }

        public ManualPilotViewModel(ApplicationModel VJEA)
        {
            this.VJEA = VJEA;
        }
    }
}
