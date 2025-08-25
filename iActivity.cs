using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava
{
    public interface iActivity
    {
        string name { get; set; }
        double distance { get; set; }
        int timeMinutes { get; set; }
    }
}
