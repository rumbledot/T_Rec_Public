using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private T_Rec_DB_Job Database;

        public ObservableCollection<Project> Projects { get; }
        public Command LoadProjectsCommand { get; }

        public ProjectsViewModel()
        {
            Title = "All Projects";
            Projects = new ObservableCollection<Project>();

            LoadProjectsCommand = new Command(async () => await ExecuteLoadProjectsCommand());
        }

        public void OnAppearing()
        {
            is_busy = true;
        }

        async Task ExecuteLoadProjectsCommand()
        {
            try
            {
                this.Database = await T_Rec_DB_Job.Instance;

                is_busy = true;

                Projects.Clear();

                var items = await Database.GetTodayJobs();

                if (items != null && items.Count > 0)
                {
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
            }
        }

        private async Task OnAddProject()
        {
            await Shell.Current.GoToAsync(nameof(NewProjectPage));
        }
    }
}
