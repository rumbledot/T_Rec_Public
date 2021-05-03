using System;
using System.Collections.Generic;
using System.Text;

namespace T_Rec.Models
{
    public class Project
    {
        public int project_id { get; set; }
        public int project_owner { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime project_started { get; set; }
        public DateTime project_ended { get; set; }
    }
}
