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
        private T_Rec_Database Database;

        private JobsViewModel _view_model;

        public TodayJobsPage()
        {
            InitializeComponent();

            BindingContext = _view_model = new JobsViewModel(tbtn_add_job);
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                Database = await T_Rec_Database.Instance;

                _view_model.OnAppearing();

                tbtn_add_job.IsEnabled = _view_model.can_add_job;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show("Page load failed");
            }
        }

        async void OnJobDone(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;

                var item = sender as Button;
                JobUnit j = item.CommandParameter as JobUnit;
                j.time_end = DateTime.Now;
                j.job_done = true;

                await Database.SaveItemAsync(j);

                _view_model.can_add_job = true;

                tbtn_add_job.IsEnabled = _view_model.can_add_job;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show("Failed to set a job to done");
            }
            finally 
            {
                IsBusy = false;
            }
        }

        async void OnJobDelete(object sender, EventArgs e)
        {
            JobUnit j = new JobUnit();

            try
            {
                var item = sender as Button;
                j = item.CommandParameter as JobUnit;

                await Database.DeleteItemAsync(j);
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show("Failed to delete a job");
            }
        }

        async void OnJobAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JobPage
            {
                BindingContext = new Models.JobUnit()
            });
        }
    }
}