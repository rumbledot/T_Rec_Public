using SQLite;
using System;
using Xamarin.Forms;

namespace T_Rec.Models
{
    public class JobUnit
    {
        [PrimaryKey, AutoIncrement]
        public int job_id { get; set; }

        [Indexed]
        public int project_id { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        public DateTime time_start { get; set; }

        public DateTime time_end { get; set; }

        public bool job_done { get; set; }

        [Ignore]
        public string project_name { get; set; }

        [Ignore]
        public double job_time_in_hours
        { 
            get 
            {
                //return (Math.Abs(time_end.Subtract(time_start).TotalHours)).ToString("0000.00");
                return Math.Abs(time_end.Subtract(time_start).TotalHours) < 0.009 ? 0 : Math.Abs(time_end.Subtract(time_start).TotalHours);
            }
        }

        [Ignore]
        public Color job_card_color 
        {
            get 
            {
                return job_done ? Color.LightGray : Color.WhiteSmoke;
            }
        }

        public string GetProjectName()
        {
            string query = $"SELECT name FROM project WHERE project_id={project_id}";
            return query;
        }
    }
}
