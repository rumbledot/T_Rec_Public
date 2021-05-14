using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class CompaniesViewModel : BaseViewModel
    {
        private T_Rec_DB_Company Database;

        public ObservableCollection<Company> Companies { get; protected set; }
        public Command LoadCompaniesCommand { get; }
        public Command OnAddCompanyCommand { get; }

        public CompaniesViewModel()
        {
            try
            {
                Title = "Registered Companies";
                Companies = new ObservableCollection<Company>();

                LoadCompaniesCommand = new Command(async () => await ExecuteLoadCompaniesCommand());

                OnAddCompanyCommand = new Command(async () => await OnAddCompany());
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

        async Task ExecuteLoadCompaniesCommand()
        {
            try
            {
                this.Database = await T_Rec_DB_Company.Instance;

                is_busy = true;

                Companies.Clear();

                var companies = await Database.GetCompanies();

                foreach (var company in companies)
                {
                    Companies.Add(company);
                }

                DependencyService.Get<Toast>().Show("Companies ready");
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to load week reviews \n {ex.Message}");
            }
            finally
            {
                is_busy = false;

                this.Database = null;
            }
        }

        async Task OnAddCompany() 
        {
            await Shell.Current.GoToAsync(nameof(NewCompanyPage));
        }

    }
}
