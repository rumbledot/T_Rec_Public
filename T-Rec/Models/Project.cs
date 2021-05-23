using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace T_Rec.Models
{
    public enum ProjectStatus 
    {
        active,
        pending,
        cancelled,
        stop,
        finished,
        all
    }

    public class CompanyIDNames
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int project_id { get; set; }

        [Indexed]
        public int company_id { get; set; }

        public ProjectStatus project_status { get; set; }

        [MaxLength(200)]
        public string project_status_note { get; set; }

        [MaxLength(100)]
        public string name { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        public DateTime project_started { get; set; }

        public DateTime project_ended { get; set; }

        public bool billable { get; set; }

        [Ignore]
        public string company_name { get; set; }

        [Ignore]
        public bool open_ended { 
            get 
            {
                return project_ended == project_started;
            } 
        }

        [Ignore]
        public double project_duration
        {
            get 
            {
                return project_ended.Subtract(project_started).TotalDays;
            }

        }

        [Ignore]
        public string project_duration_String
        {
            get
            {
                int proper_day = (int)Math.Floor(project_duration);
                int proper_hours = (int)Math.Ceiling((project_duration - proper_day) * 24);
                return $"{proper_day} days : {proper_hours} hrs";
            }
        }

        public string GetCompanyName() 
        {
            string query = $"SELECT name FROM COMPANY WHERE company_id={company_id}";
            return query;
        }
    }
}
