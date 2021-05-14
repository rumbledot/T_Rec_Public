using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class JobsViewModel : BaseViewModel
    {
        private T_Rec_DB_Job Database;

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

        public JobsViewModel(ToolbarItem btn)
        {
            try
            {
                Title = "Today's Jobs";
                toolbtn_add_new_job = btn;
                Jobs = new ObservableCollection<JobUnit>();

                LoadTodayJobsCommand = new Command(async () => await ExecuteLoadJobsCommand());

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

        async Task ExecuteLoadJobsCommand()
        {
            try
            {
                is_busy = true;
                can_add_job = true;

                Jobs.Clear();

                //this.Database = await T_Rec_DB_Job.Instance;

                //var items = await Database.GetTodayJobs();

                //if (items != null && items.Count > 0)
                //{
                //    foreach (JobUnit item in items)
                //    {
                //        Jobs.Add(item);
                //        if (!item.job_done)
                //        {
                //            can_add_job = false;
                //        }
                //    }

                //    DependencyService.Get<Toast>().Show("Jobs loaded");

                //    toolbtn_add_new_job.IsEnabled = can_add_job;
                //}

                List<object[]> result = T_Rec_Controller.RunSQL("SELECT JOBUNIT.*, PROJECT.NAME FROM JOBUNIT JOIN PROJECT ON JOBUNIT.PROJECT_ID=PROJECT.PROJECT_ID", false);
                //List<object[]> result = T_Rec_Controller.RunSQL("SELECT * FROM JOBUNIT", false);

                JobUnit j;
                Console.WriteLine("JOBS");
                Console.WriteLine(new string('-', 20));
                if (result != null && result.Count > 0)
                {
                    foreach (var row in result)
                    {
                        if (row != null)
                        {
                            foreach (var cell in row)
                            {
                                Console.Write(cell + " : ");
                            }
                            //j = new JobUnit()
                            //{
                            //    project_id = Convert.ToInt32(row[0]),
                            //    company_id = Convert.ToInt32(row[1]),
                            //    project_status = (ProjectStatus)Convert.ToInt32(row[2]),
                            //    project_status_note = row[3].ToString(),
                            //    name = row[4].ToString(),
                            //    description = row[5].ToString(),
                            //    project_started = Convert.ToDateTime(row[6]),
                            //    project_ended = Convert.ToDateTime(row[7]),
                            //    billable = Convert.ToBoolean(row[8]),
                            //    company_name = row[9].ToString()
                            //};

                            //Jobs.Add(j);
                            //Console.WriteLine(new string('-', 20));
                        }
                        Console.WriteLine();
                        Console.WriteLine(new string('-', 20));
                    }

                    DependencyService.Get<Toast>().Show("All Projects loaded");

                    toolbtn_add_new_job.IsEnabled = can_add_job;
                }
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
