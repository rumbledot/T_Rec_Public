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
        public int day_total_jobs { get; set; }
    }
}
