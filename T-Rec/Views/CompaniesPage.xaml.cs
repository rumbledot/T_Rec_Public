using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompaniesPage : ContentPage
    {
        public T_Rec_DB_Company Database { get; set; }
        public CompaniesPage()
        {
            InitializeComponent();
        }

        public async void OnAppearing()
        {
            this.Database = await T_Rec_DB_Company.Instance;
        }

        async void OnEdit(object sender, EventArgs e)
        {
            var item = sender as Button;
            Company c = item.CommandParameter as Company;

            DependencyService.Get<Toast>().Show($"Edit a company {c.company_id}");
        }

        async void OnDelete(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                var item = sender as Button;
                Company c = item.CommandParameter as Company;

                await Database.DeleteCompanyAsync(c);

                DependencyService.Get<Toast>().Show($"Company deleted");

                IsBusy = false;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to delete a company \n {ex.Message}");
            }
        }
    }
}