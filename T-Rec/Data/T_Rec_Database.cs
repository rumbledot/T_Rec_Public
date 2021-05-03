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
    public class T_Rec_Database
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<T_Rec_Database> Instance = new AsyncLazy<T_Rec_Database>(async () =>
        {
            var instance = new T_Rec_Database();
            CreateTableResult result = await Database.CreateTableAsync<JobUnit>();
            return instance;
        });

        public T_Rec_Database()
        {
            Database = new SQLiteAsyncConnection(T_Rec_Controller.DatabasePath, T_Rec_Controller.Flags);
        }

        public Task<List<JobUnit>> GetTodayJobs()
        {
            try
            {
                Console.WriteLine($"getting {DateTime.Now} jobs");
                return GetTodayJobs(DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get today jobs \n {ex.Message} \n {ex.StackTrace}");
                throw ex;
            }
        }

        public Task<List<JobUnit>> GetTodayJobs(DateTime today)
        {
            today = DateTime.Parse(today.Date.ToString("yyyy-MM-dd 00:00:00"));
            DateTime today_end = DateTime.Parse(today.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            Console.WriteLine($"getting jobs between {today} - {today_end}");

            return Database.Table<JobUnit>().Where(job => (
            job.time_start > today && job.time_start < today_end)).OrderByDescending(job =>(job.time_start)).ToListAsync();
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

                if (input.DayOfWeek == DayOfWeek.Sunday) sunday_offset = -7;

                delta = DayOfWeek.Monday - input.DayOfWeek;
                weekstart_day = DateTime.Parse(input.AddDays(delta + sunday_offset).ToString("yyyy-MM-dd 00:00:00"));

                delta = DayOfWeek.Saturday - input.DayOfWeek;
                weekend_day = DateTime.Parse(input.AddDays(delta + 1 + sunday_offset).ToString("yyyy-MM-dd 00:00:00"));

                Console.WriteLine($"week start : {weekstart_day} - {weekend_day}");

                return Database.Table<JobUnit>().Where(job => (
                job.time_start > weekstart_day && job.time_start < weekend_day)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to get week reviews \n {ex.Message} \n {ex.StackTrace}");
                throw ex;
            }
        }

        public Task<List<JobUnit>> GetItemsAsync()
        {
            return Database.Table<JobUnit>().ToListAsync();
        }


        public Task<List<JobUnit>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<JobUnit>("SELECT * FROM [JOBUNIT] WHERE [JOB_DONE] = 0");
        }

        public Task<JobUnit> GetItemAsync(int id)
        {
            return Database.Table<JobUnit>().Where(i => i.job_id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(JobUnit item)
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

        public Task<int> DeleteItemAsync(JobUnit item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
