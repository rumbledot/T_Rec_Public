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

        public void OnMore(object sender, EventArgs e)
        {
            Button menu = sender as Button;
            JobInADay j = menu.CommandParameter as JobInADay;
            if (j == null)
            {
                j = new JobInADay() 
                {
                    day_total_hours = 0,
                    day_total_jobs = 0
                };
            }
            Console.WriteLine($"Detail clicked {j.day_name} : {j.day_total_hours}hrs : {j.day_total_jobs}");
        }
    }
}