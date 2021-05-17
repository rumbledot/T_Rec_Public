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

            BindingContext = _viewModel = new WeekReviewViewModel(label_Proper_hours, label_Proper_mins);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            dtpicker_Review_date.MinimumDate = DateTime.Now.AddYears(-1);
            dtpicker_Review_date.MaximumDate = DateTime.Now;
            dtpicker_Review_date.Date = DateTime.Now.AddDays(-1);

            _viewModel.OnAppearing();
        }

        public async void OnMore(object sender, EventArgs e)
        {
            DateTime day_to_view = DateTime.Now;
            try
            {
                var item = sender as Button;
                JobInADay job = item.CommandParameter as JobInADay;

                day_to_view = DateTime.Parse(job.day_date);

                await Navigation.PushAsync(new TodayJobsPage(day_to_view));
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to review jobs {day_to_view.ToString("dd MMM yyyy")} \n {ex.Message}");
            }
        }

        public async void OnReviewByDate(object sender, EventArgs e) 
        {
            DateTime day_to_view = dtpicker_Review_date.Date;
            try
            {
                await Navigation.PushAsync(new TodayJobsPage(day_to_view));
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to review jobs {day_to_view.ToString("dd MMM yyyy")} \n {ex.Message}");
            }
        }
    }
}