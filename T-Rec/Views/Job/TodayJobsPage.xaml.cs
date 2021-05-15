using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using T_Rec.Models;
using T_Rec.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/databases
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodayJobsPage : ContentPage
    {
        private T_Rec_DB_Job Database;

        private JobsViewModel _view_model;

        public bool viewing_only { get; set; } = false;

        public TodayJobsPage()
        {
            try
            {
                InitializeComponent();

                viewing_only = false;
                fab_Add_job.IsVisible = true;
                fab_Add_job.IsVisible = true;

                BindingContext = _view_model = new JobsViewModel(tbtn_add_job, DateTime.Now);
            }
            catch (Exception ex)
            {

                DependencyService.Get<Toast>().Show($"Page load failed \n {ex.Message}");
            }
        }

        public TodayJobsPage(DateTime get_job_on_date)
        {
            try
            {
                InitializeComponent();
                
                Title = "Jobs Reviews";
                viewing_only = true;
                fab_Add_job.IsVisible = false;

                BindingContext = _view_model = new JobsViewModel(tbtn_add_job, get_job_on_date);
            }
            catch (Exception ex)
            {

                DependencyService.Get<Toast>().Show($"Page load failed \n {ex.Message}");
            }
        }

        protected override void OnAppearing()
        {
                base.OnAppearing();

                _view_model.OnAppearing();

                tbtn_add_job.IsEnabled = _view_model.can_add_job && !viewing_only;
        }

        async void OnJobDone(object sender, EventArgs e)
        {
            if (!viewing_only)
            {
                try
                {
                    Database = await T_Rec_DB_Job.Instance;

                    IsBusy = true;

                    var item = sender as Button;
                    JobUnit j = item.CommandParameter as JobUnit;
                    j.time_end = DateTime.Now;
                    j.job_done = true;

                    await Database.SaveJobAsync(j);

                    _view_model.can_add_job = true;

                    tbtn_add_job.IsEnabled = _view_model.can_add_job && !viewing_only;
                }
                catch (Exception ex)
                {
                    DependencyService.Get<Toast>().Show($"Failed to set a job to done \n {ex.Message}");
                }
                finally
                {
                    IsBusy = false;

                    Database = null;
                } 
            }
        }

        async void OnJobDelete(object sender, EventArgs e)
        {
            if (!viewing_only)
            {
                try
                {
                    Database = await T_Rec_DB_Job.Instance;

                    JobUnit j = new JobUnit();

                    var item = sender as Button;
                    j = item.CommandParameter as JobUnit;

                    await Database.DeleteJobAsync(j);
                }
                catch (Exception ex)
                {
                    DependencyService.Get<Toast>().Show($"Failed to delete a job \n {ex.Message}");
                }
                finally
                {
                    Database = null;
                } 
            }
        }

        async void OnJobAdded(object sender, EventArgs e)
        {
            if (!viewing_only)
            {
                await Navigation.PushAsync(new NewJobPage
                {
                    BindingContext = new JobUnit()
                }); 
            }
        }
    }
}