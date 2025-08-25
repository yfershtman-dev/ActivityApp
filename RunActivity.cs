using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava
{
    public class RunActivity : iActivity
    {
        public string name { get; set; }

        private double _distance;

        public double distance
        {
            get { return _distance; } 
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Invalid distance!");
                _distance = value;
            }
        }
        public int timeMinutes { get; set; }

        public RunActivity(string name, double distance, int timeMinutes)
        {
            this.name = name;
            this.distance = distance;
            this.timeMinutes = timeMinutes;
        }
    }
}
