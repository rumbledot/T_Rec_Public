using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;

namespace T_Rec
{
    //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/databases
    public partial class T_Rec_DB_Job
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<T_Rec_DB_Job> Instance = new AsyncLazy<T_Rec_DB_Job>
        (
            async () =>
            {
                var instance = new T_Rec_DB_Job();
                CreateTableResult result = await Database.CreateTableAsync<JobUnit>();
                return instance;
            }
        );

        public T_Rec_DB_Job()
        {
            try
            {
                Database = new SQLiteAsyncConnection(T_Rec_Controller.DatabasePath, T_Rec_Controller.Flags);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<JobUnit>> GetTodayJobs()
        {
            try
            {
                return GetTodayJobs(DateTime.Now);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<JobUnit>> GetTodayJobs(DateTime today)
        {
            try
            {
                today = DateTime.Parse(today.Date.ToString("yyyy-MM-dd 00:00:00"));
                DateTime today_end = DateTime.Parse(today.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
                //Console.WriteLine($"getting jobs between {today} - {today_end}");

                return Database.Table<JobUnit>().Where(job => (
                job.time_start > today && job.time_start < today_end)).OrderByDescending(job => (job.time_start)).ToListAsync();
            }
            catch (Exception ex )
            {
                throw ex;
            }
        }

        public Task<List<JobUnit>> GetWeekReviews()
        {
            try
            {
                DateTime input = DateTime.Now;
                DateTime weekstart_day = DateTime.Now;
                DateTime weekend_day = DateTime.Now;
                int delta = 0;
                int sunday_offset = 0; //if this is Sunday then get a week before

                //if (input.DayOfWeek == DayOfWeek.Sunday) sunday_offset = -7;

                delta = DayOfWeek.Sunday - input.DayOfWeek;
                weekstart_day = DateTime.Parse(input.AddDays(delta + sunday_offset).ToString("yyyy-MM-dd 00:00:00"));
                //Console.WriteLine($"Weel start date {weekstart_day}");

                delta = DayOfWeek.Saturday - input.DayOfWeek;
                weekend_day = DateTime.Parse(input.AddDays(delta + 1 + sunday_offset).ToString("yyyy-MM-dd 00:00:00"));
                //Console.WriteLine($"Weel start date {weekend_day}");

                return Database.Table<JobUnit>()
                    .Where
                    (
                        job => 
                        (
                            job.time_start > weekstart_day && job.time_start < weekend_day
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<JobUnit>> GetJobsAsync()
        {
            return Database.Table<JobUnit>().ToListAsync();
        }


        public Task<List<JobUnit>> GetJobsNotDoneAsync()
        {
            return Database.QueryAsync<JobUnit>("SELECT * FROM [JOBUNIT] WHERE [JOB_DONE] = 0");
        }

        public Task<JobUnit> GetJobAsync(int id)
        {
            return Database.Table<JobUnit>()
                .Where
                (
                    i => i.job_id == id
                )
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveJobAsync(JobUnit item)
        {
            if (item.job_id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteJobAsync(JobUnit item)
        {
            return Database.DeleteAsync(item);
        }
    }
}