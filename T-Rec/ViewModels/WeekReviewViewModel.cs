using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.Helpers;
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

        public int total_proper_hours 
        {
            get
            {
                return (int)Math.Floor(_total_week_hours);
            }
        }

        public int total_proper_mins
        {
            get 
            {
                return (int)Math.Floor((_total_week_hours - Math.Floor(_total_week_hours)) * 60);
            }
        }

        public WeekReviewViewModel(Label proper_hours, Label proper_mins)
        {
            try
            {
                Title = "Week in review";
                Days = new ObservableCollection<JobInADay>();

                LoadWeekCommand = new Command(async () => await ExecuteLoadWeekCommand(proper_hours, proper_mins));
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

        async Task ExecuteLoadWeekCommand(Label proper_hours, Label proper_mins)
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

                    JobInADay day_reviews = new JobInADay()
                    {
                        day_name = day.ToString(),
                        day_date = each_day.ToString("d MMM yyyy"),
                        day_total_hours = total_hours,
                        day_total_jobs = 0,
                        active_day = false,
                        today_day = (day == DateTime.Now.DayOfWeek) //? Color.FromHex(ExtensionHelper.FindResource("PrimaryLight").ToString()) : Color.FromHex(ExtensionHelper.FindResource("BackgroundLight").ToString())
                    };

                    Console.WriteLine($"Day : {day} : {day_reviews.today_day} ({DateTime.Now.DayOfWeek})");
                    Console.WriteLine($"day total hours : {total_hours}");

                    if (today_jobs != null && today_jobs.Count > 0)
                    {
                        foreach (var job in today_jobs)
                        {
                            //Console.WriteLine($"job : {job.time_end} - {job.time_start}");
                            //Console.WriteLine($"job : {job.time_end.Subtract(job.time_start).TotalHours}");
                            //Console.WriteLine($"job : {job.project_name} : {job.job_time_in_hours}");
                            total_hours += (job.billable && job.job_done) ? job.job_time_in_hours : 0;
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

                    Days.Add(day_reviews);
                }
                proper_hours.Text = total_proper_hours.ToString();
                proper_mins.Text = total_proper_mins.ToString();

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
