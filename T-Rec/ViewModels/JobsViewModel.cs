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
                this.Database = await T_Rec_DB_Job.Instance;

                is_busy = true;
                can_add_job = true;
                Jobs.Clear();

                var items = await Database.GetTodayJobs();
                
                if (items != null && items.Count > 0)
                {
                    foreach (JobUnit item in items)
                    {
                        Jobs.Add(item);
                        if (!item.job_done)
                        {
                            can_add_job = false;
                        }
                    }

                    DependencyService.Get<Toast>().Show("Jobs loaded");

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
