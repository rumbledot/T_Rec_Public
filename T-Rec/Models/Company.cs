using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace T_Rec.Models
{
    public class Company
    {
        [PrimaryKey, AutoIncrement]
        public int company_id { get; set; }

        [MaxLength(100)]
        public string name { get; set; }

        [MaxLength(50)]
        public string email { get; set; }
    }
}
