using System;
using System.Collections.Generic;
using System.Text;

namespace T_Rec.Models
{
    public class JobInADay
    {
        public string day_name { get; set; }
        public string day_date { get; set; }
        public double day_total_hours { get; set; }

        public string day_total_hours_string
        {
            get 
            {
                int proper_hour = (int)Math.Floor(day_total_hours);
                int proper_min = (int)Math.Ceiling((day_total_hours - proper_hour) * 60);
                return $"{proper_hour} hrs : {proper_min} mins";
            }
        }

        public int day_total_jobs { get; set; }
    }
}
