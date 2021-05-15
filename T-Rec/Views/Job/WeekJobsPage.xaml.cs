using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.ViewModels;
using T_Rec.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    public partial class WeekJobsPage : ContentPage
    {
        WeekReviewViewModel _viewModel;

        public WeekJobsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new WeekReviewViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.OnAppearing();
        }

        public async void OnMore(object sender, EventArgs e)
        {
            DateTime day_we_wanted = DateTime.Now;
            try
            {
                var item = sender as Button;
                JobInADay job = item.CommandParameter as JobInADay;

                //DependencyService.Get<Toast>().Show($"Edit a company {c.company_id}");

                //await Navigation.PushAsync(new NewCompanyPage
                //{
                //    BindingContext = c
                //});
                day_we_wanted = DateTime.Parse(job.day_date);

                await Navigation.PushAsync(new TodayJobsPage(day_we_wanted));
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to review jobs {day_we_wanted.ToString("dd MMM yyyy")} \n {ex.Message}");
            }
        }
    }
}