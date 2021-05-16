using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace T_Rec.Models
{
    public enum PrefferedWorkDays 
    {
        MondayFriday,
        MondaySaturday,
        WholeWeek
    }

    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int user_id { get; set; }

        [Indexed]
        public int company_id { get; set; }

        [MaxLength(50)]
        public string name { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(50)]
        public string phone { get; set; }

        [MaxLength(50)]
        public string mobile_phone { get; set; }

        public int daily_hours_goals { get; set; }

        public PrefferedWorkDays working_days { get; set; }

        public int entitled_break_mins { get; set; }
    }
}
