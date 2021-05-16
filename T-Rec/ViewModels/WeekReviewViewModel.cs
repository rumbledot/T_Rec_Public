using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class WeekReviewViewModel : BaseViewModel
    {
        private T_Rec_DB_Job Database;

        public ObservableCollection<JobInADay> Days { get; }
        public Command LoadWeekCommand { get; }

        double _total_week_hours = 0;
        public double total_week_hours
        {
            get { return _total_week_hours; }
            set { SetProperty(ref _total_week_hours, value); }
        }

        string _total_week_hours_string = "0";
        public string total_week_hours_string
        {
            get 
            {
                int proper_hour = (int)Math.Floor(_total_week_hours);
                return proper_hour.ToString();
            }
            set 
            {
                SetProperty(ref _total_week_hours_string, value);
            }
        }

        string _total_week_mins_string = "0";
        public string total_week_mins_string
        {
            get
            {
                int proper_hour = (int)Math.Floor(_total_week_hours);
                int proper_min = (int)Math.Ceiling((_total_week_hours - proper_hour) * 60);
                return proper_min.ToString();
            }
            set
            {
                SetProperty(ref _total_week_mins_string, value);
            }
        }

        public WeekReviewViewModel()
        {
            try
            {
                Title = "Week in review";
                Days = new ObservableCollection<JobInADay>();

                LoadWeekCommand = new Command(async () => await ExecuteLoadWeekCommand());
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Page failed to load \n {ex.Message}");
            }
        }

        public void OnAppearing()
        {
            is_busy = true;
        }

        async Task ExecuteLoadWeekCommand()
        {
            try
            {
                this.Database = await T_Rec_DB_Job.Instance;

                is_busy = true;
                total_week_hours = 0;
            
                int delta = 0;
                double total_hours = 0;
                DateTime input = DateTime.Now;
                DateTime each_day = DateTime.Now;
                DateTime each_day_end = DateTime.Now;
                List<JobUnit> today_jobs = new List<JobUnit>();

                Days.Clear();

                var jobs = await Database.GetWeekReviews();

                //Console.WriteLine($"total jobs in week {jobs.Count}");

                //week starts on Monday, ends on Saturday
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))
                              .OfType<DayOfWeek>()
                              .ToList())
                {
                    total_hours = 0;
                    delta = day - input.DayOfWeek;
                    each_day = DateTime.Parse(input.AddDays(delta).ToString("yyyy-MM-dd 00:00:00"));
                    each_day_end = DateTime.Parse(input.AddDays(delta).AddDays(1).ToString("yyyy-MM-dd 00:00:00"));

                    today_jobs = jobs.Where<JobUnit>(j =>(j.time_start > each_day && j.time_start < each_day_end)).OrderBy(j => (j.time_start)).ToList();

                    Console.WriteLine($"Day : {day}");
                    Console.WriteLine($"day total hours : {total_hours}");

                    JobInADay day_reviews = new JobInADay()
                    {
                        day_name = day.ToString(),
                        day_date = each_day.ToString("d MMM yyyy"),
                        day_total_hours = total_hours,
                        day_total_jobs = 0,
                        active_day = false,
                        today_day = day == DateTime.Now.DayOfWeek ? Color.LightGray : Color.WhiteSmoke
                    };

                    if (today_jobs != null && today_jobs.Count > 0)
                    {
                        foreach (var job in today_jobs)
                        {
                            //Console.WriteLine($"job : {job.time_end} - {job.time_start}");
                            //Console.WriteLine($"job : {job.time_end.Subtract(job.time_start).TotalHours}");
                            //Console.WriteLine($"job : {job.project_name} : {job.job_time_in_hours}");
                            total_hours += job.billable ? job.job_time_in_hours : 0;
                        }

                        day_reviews = new JobInADay()
                        {
                            day_name = day.ToString(),
                            day_date = each_day.ToString("d MMM yyyy"),
                            day_total_hours = total_hours,
                            day_total_jobs = today_jobs.Count,
                            active_day = true
                        };
                    }

                    total_week_hours += total_hours;
;
                    Days.Add(day_reviews);
                }

                DependencyService.Get<Toast>().Show("Weekly Reviews ready");
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to load week reviews \n {ex.Message}");
            }
            finally
            {
                is_busy = false;

                this.Database = null;
            }
        }
    }
}
