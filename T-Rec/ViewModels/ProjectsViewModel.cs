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

                is_busy = true;

                Projects.Clear();

                #region safe keeping

                //this.Database = await T_Rec_DB_Project.Instance;
                //this.Database_Company = await T_Rec_DB_Company.Instance;

                //var items = await Database.GetAllProjects();
                //var companies = await Database_Company.GetCompanies();

                //if (items != null && items.Count > 0 && companies != null && companies.Count > 0)
                //{
                //    foreach (var item in items)
                //    {
                //        companies.ForEach(delegate (Company c)
                //        {
                //            if (c.company_id == item.company_id) item.company_name = c.name;
                //        });
                //        //Console.WriteLine(item.name + " : " + item.description + " : " + item.company_name);
                //        Projects.Add(item);
                //    }

                //    DependencyService.Get<Toast>().Show("All Projects loaded");
                //} 

                #endregion //safe keeping

                List<object[]> result = T_Rec_Controller.QueryFetch("SELECT PROJECT.*, COMPANY.NAME FROM PROJECT JOIN COMPANY ON PROJECT.COMPANY_ID=COMPANY.COMPANY_ID", false);

                Project p;

                foreach (var row in result)
                {
                    if (row != null)
                    {
                        p = new Project()
                        {
                            project_id = Convert.ToInt32(row[0]),
                            company_id = Convert.ToInt32(row[1]),
                            project_status = (ProjectStatus)Convert.ToInt32(row[2]),
                            project_status_note = row[3].ToString(),
                            name = row[4].ToString(),
                            description = row[5].ToString(),
                            project_started = Convert.ToDateTime(row[6]),
                            project_ended = Convert.ToDateTime(row[7]),
                            billable = Convert.ToBoolean(row[8]),
                            company_name = row[9].ToString()
                        };

                        Projects.Add(p);
                        //Console.WriteLine(new string('-', 20));
                    }
                }

                DependencyService.Get<Toast>().Show("All Projects loaded");
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to load projects \n {ex.Message} \n {ex.StackTrace}");
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
