using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompaniesPage : ContentPage
    {
        public T_Rec_DB_Company Database { get; set; }

        private CompaniesViewModel _view_model;

        public CompaniesPage()
        {
            try
            {
                InitializeComponent();

                BindingContext = _view_model = new CompaniesViewModel();
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Page load failed {ex.Message}");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _view_model.OnAppearing();
        }

        async void OnEdit(object sender, EventArgs e)
        {
            try
            {
                var item = sender as Button;
                Company c = item.CommandParameter as Company;

                DependencyService.Get<Toast>().Show($"Edit a company {c.company_id}");

                //await Navigation.PushAsync(new NewCompanyPage
                //{
                //    BindingContext = c
                //});
                await Navigation.PushAsync(new NewCompanyPage(c));
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to edit a company \n {ex.Message}");
            }
        }

        async void OnDelete(object sender, EventArgs e)
        {
            try
            {
                this.Database = await T_Rec_DB_Company.Instance;

                IsBusy = true;
                var item = sender as Button;
                Company c = item.CommandParameter as Company;

                await Database.DeleteCompanyAsync(c);

                DependencyService.Get<Toast>().Show($"Company deleted");
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to delete a company \n {ex.Message}");
            }
            finally 
            {
                IsBusy = false;
            }
        }
    }
}