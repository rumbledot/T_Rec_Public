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
        private T_Rec_Database Database;

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
        //public Command OnJobDoneCommand { get; }
        //public Command OnJobDeleteCommand { get; }

        public JobsViewModel(ToolbarItem btn)
        {
            Title = "Today's Jobs";
            toolbtn_add_new_job = btn;
            Jobs = new ObservableCollection<JobUnit>();

            LoadTodayJobsCommand = new Command(async (object view) => await ExecuteLoadJobsCommand(view));

            OnAddJobCommand = new Command(async () => await OnAddJob());

            //OnJobDoneCommand = new Command(async (object item) => await OnJobDone(item));

            //OnJobDeleteCommand = new Command(async (object item) => await OnJobDelete(item));
        }

        public async void OnAppearing()
        {
            is_busy = true;

            this.Database = await T_Rec_Database.Instance;
        }

        async Task ExecuteLoadJobsCommand(object view)
        {
            is_busy = true;
            can_add_job = true;

            try
            {
                this.Database = await T_Rec_Database.Instance;

                Jobs.Clear();
                var items = await Database.GetTodayJobs();
                Console.WriteLine($"item count {items.Count}");
                if (items != null && items.Count > 0)
                {
                    foreach (JobUnit item in items)
                    {
                        Jobs.Add(item);
                        if (!item.job_done)
                        {
                            Console.WriteLine($"job {item.job_id} is not done");

                            can_add_job = false;
                        }
                    }

                    toolbtn_add_new_job.IsEnabled = can_add_job; 
                }

                //CarouselView jobview = view as CarouselView;
                //Console.WriteLine("View orientation " + jobview.ItemsLayout.Orientation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load today jobs \n {ex.Message} \n {ex.StackTrace}");
            }
            finally
            {
                is_busy = false;
            }
        }

        private async Task OnAddJob()
        {
            await Shell.Current.GoToAsync(nameof(JobPage));
            //if (can_add_job)
            //{
            //    await Shell.Current.GoToAsync(nameof(JobPage));
            //}
        }

        //private async Task OnJobDone(object item)
        //{
        //    try
        //    {
        //        JobUnit job = (JobUnit)item;
        //        job.job_done = true;
        //        job.time_end = DateTime.Now;

        //        await Database.SaveItemAsync(job);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to set job done \n {ex.Message} \n {ex.StackTrace}");
        //    }
        //}

        //private async Task OnJobDelete(object item)
        //{
        //    try
        //    {
        //        JobUnit job = (JobUnit)item;

        //        await Database.DeleteItemAsync(job);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to delete job done \n {ex.Message} \n {ex.StackTrace}");
        //    }
        //}
    }
}
