using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Data;
using T_Rec.Models;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class JobsViewModel : BaseViewModel
    {
        private T_Rec_DB_Job Database;
        private T_Rec_DB_Project Database_Project;

        bool _can_add_job = true;
        public bool can_add_job
        {
            get { return _can_add_job; }
            set { SetProperty(ref _can_add_job, value); }
        }

        private ToolbarItem toolbtn_add_new_job;

        public ObservableCollection<JobUnit> Jobs { get; }
        public Command LoadTodayJobsCommand { get; }
        public Command OnAddJobCommand { get; }

        public JobsViewModel(ToolbarItem btn, DateTime get_job_on_date)
        {
            try
            {
                Title = "Today's Jobs";
                toolbtn_add_new_job = btn;
                Jobs = new ObservableCollection<JobUnit>();

                LoadTodayJobsCommand = new Command(() => ExecuteLoadJobsCommand(get_job_on_date));

                OnAddJobCommand = new Command(async () => await OnAddJob());
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

        async void ExecuteLoadJobsCommand(DateTime get_job_on_date)
        {
            try
            {
                this.Database = await T_Rec_DB_Job.Instance;
                this.Database_Project = await T_Rec_DB_Project.Instance;

                is_busy = true;
                can_add_job = true;

                Jobs.Clear();

                var items = await Database.GetTodayJobs(get_job_on_date);

                Project p;

                if (items != null && items.Count > 0)
                {
                    foreach (JobUnit item in items)
                    {
                        p = await Database_Project.GetProjectAsync(item.project_id);

                        item.project_name = p.name;
                        item.billable = p.billable;
                        Jobs.Add(item);
                        if (!item.job_done)
                        {
                            can_add_job = false;
                        }
                    }

                    DependencyService.Get<Toast>().Show("Jobs loaded");

                    toolbtn_add_new_job.IsEnabled = can_add_job;
                }
                //string today_start = DateTime.Now.ToString("MM/dd/yyyy 12:00:00");
                //string today_end = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy 12:00:00");
                //string query = "SELECT JOBUNIT.*, PROJECT.NAME " +
                //                "FROM JOBUNIT " +
                //                "JOIN PROJECT ON JOBUNIT.PROJECT_ID=PROJECT.PROJECT_ID " +
                //                //$"WHERE JOBUNIT.TIME_START > '{today_start} AM' " +
                //                //$"AND JOBUNIT.TIME_START < '{today_end} AM' " +
                //                $"ORDER BY JOBUNIT.TIME_START DESC ";
                //Console.WriteLine(query);
                //List<object[]> result = T_Rec_Controller.QueryFetch(query, false);
                ////List<object[]> result = T_Rec_Controller.RunSQL("SELECT * FROM JOBUNIT", false);

                //JobUnit j;
                
                //if (result != null && result.Count > 0)
                //{
                //    foreach (var row in result)
                //    {
                //        if (row != null)
                //        {
                //            foreach (var cell in row)
                //            {
                //                Console.Write(cell + " : ");
                //            }

                //            j = new JobUnit()
                //            {
                //                job_id = Convert.ToInt32(row[0]),
                //                project_id = Convert.ToInt32(row[1]),
                //                description = row[2].ToString(),
                //                time_start = Convert.ToDateTime(row[3]),
                //                time_end = Convert.ToDateTime(row[4]),
                //                job_done = Convert.ToBoolean(row[5]),
                //                billable = Convert.ToBoolean(row[6])
                //            };

                //            if (!j.job_done) can_add_job = false;

                //            Jobs.Add(j);
                //        }
                //        Console.WriteLine();
                //        Console.WriteLine(new string('-', 20));
                //    }

                //    DependencyService.Get<Toast>().Show("All Projects loaded");

                //    toolbtn_add_new_job.IsEnabled = can_add_job;
                //}
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to load today's jobs \n {ex.Message}");
            }
            finally
            {
                is_busy = false;

                this.Database = null;
            }
        }

        private async Task OnAddJob()
        {
            await Shell.Current.GoToAsync(nameof(NewJobPage));
        }
    }
}
