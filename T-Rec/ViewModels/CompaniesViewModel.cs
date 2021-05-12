using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class CompaniesViewModel : BaseViewModel
    {
        private T_Rec_DB_Company Database;

        public ObservableCollection<Company> Companies { get; protected set; }
        public Command LoadCompaniesCommand { get; }
        public Command OnAddCompanyCommand { get; }

        double _total_week_hours = 0;
        public double total_week_hours
        {
            get { return _total_week_hours; }
            set { SetProperty(ref _total_week_hours, value); }
        }

        string _total_week_hours_string = "0";
        public string total_week_hours_string
        {
            get
            {
                int proper_hour = (int)Math.Floor(_total_week_hours);
                int proper_min = (int)Math.Ceiling((_total_week_hours - proper_hour) * 60);
                return $"{proper_hour} hrs : {proper_min} mins";
            }
            set
            {
                SetProperty(ref _total_week_hours_string, value);
            }
        }

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

        public async void OnAppearing()
        {
            try
            {
                is_busy = true;

                this.Database = await T_Rec_DB_Company.Instance;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Database instance failed \n {ex.Message}");
            }
        }

        async Task ExecuteLoadCompaniesCommand()
        {
            is_busy = true;
            total_week_hours = 0;

            try
            {
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
            }
        }

        async Task OnAddCompany() 
        {
            DependencyService.Get<Toast>().Show("Add new company command");
        }

    }
}
