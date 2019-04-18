using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for FlightMonitor.xaml
    /// </summary>
    public partial class FlightMonitor : UserControl
    {
        private double oldLon;
        public double OldLon
        {
            get
            {
                return this.oldLon;
            }
            set
            {
                this.oldLon = value;
            }
        }

        private double oldLat;
        public double OldLat
        {
            get
            {
                return this.oldLat;
            }
            set
            {
                this.oldLat = value;
            }
        }

        private double newLon;
        public double NewLon
        {
            get
            {
                return this.NewLon;
            }
            set
            {
                this.NewLon = value;
            }
        }

        private double newLat;
        public double NewLat
        {
            get
            {
                return this.newLat;
            }
            set
            {
                this.newLat = value;
            }
        }

        private int Counter = 0;

        public FlightMonitor()
        {
            InitializeComponent();
            FlightBoardViewModel FBVM = new FlightBoardViewModel();
            this.DataContext = FBVM;
            FBVM.ThisEvent += () =>
            {
                this.NewLon = FBVM.Lon;
                this.NewLat = FBVM.Lat;
            };
        }

        //public void draw
    }
}
