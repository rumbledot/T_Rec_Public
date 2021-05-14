using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Data;
using T_Rec.Models;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private T_Rec_DB_Project Database;
        private T_Rec_DB_Company Database_Company;

        public ObservableCollection<Project> Projects { get; }

        public Command LoadProjectsCommand { get; }

        public Command OnAddProjectCommand { get; }

        public ProjectsViewModel()
        {
            try
            {
                Title = "All Projects";
                Projects = new ObservableCollection<Project>();

                LoadProjectsCommand = new Command(async () => await ExecuteLoadProjectsCommand());

                OnAddProjectCommand = new Command(async () => await OnAddProject());
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

        async Task ExecuteLoadProjectsCommand()
        {
            try
            {
                this.Database = await T_Rec_DB_Project.Instance;
                this.Database_Company = await T_Rec_DB_Company.Instance;

                is_busy = true;

                Projects.Clear();

                var items = await Database.GetAllProjects();
                var companies = await Database_Company.GetCompanies();

                if (items != null && items.Count > 0 && companies != null && companies.Count > 0)
                {
                    foreach (var item in items)
                    {
                        companies.ForEach(delegate (Company c)
                        {
                            if (c.company_id == item.company_id) item.company_name = c.name;
                        });
                        Console.WriteLine(item.name + " : " + item.description + " : " + item.company_name);
                        Projects.Add(item);
                    }

                    DependencyService.Get<Toast>().Show("All Projects loaded");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to load projects \n {ex.Message}");
            }
            finally
            {
                is_busy = false;

                this.Database = null;
                this.Database_Company = null;
            }
        }

        private async Task OnAddProject()
        {
            await Shell.Current.GoToAsync(nameof(NewProjectPage));
        }
    }
}
