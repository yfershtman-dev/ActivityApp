using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava
{
    public class User
    {
        public User(string username, string password)
        {
            userName = username;
            passWord = password;
            _activities = new List<iActivity>();
        }
        public string userName { get; set; }
        public string passWord { get; set; }

        private List<iActivity> _activities;

        public List<iActivity> activities
        {
            get { return _activities; }

            set {  _activities = value; }
        }
        
        public void AddBikeActivity(BikeActivity activity)
        {
            activities.Add(activity);
        }

        public void AddRunActivity(RunActivity activity)
        {
            activities.Add(activity);
        }

    }
}
