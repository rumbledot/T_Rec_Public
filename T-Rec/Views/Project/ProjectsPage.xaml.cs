using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Data;
using T_Rec.Models;
using T_Rec.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPage
    {
        public T_Rec_DB_Project Database { get; set; }

        private ProjectsViewModel _view_model;

        public ProjectsPage()
        {
            try
            {
                InitializeComponent();

                BindingContext = _view_model = new ProjectsViewModel();
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Page load failed \n {ex.Message}");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _view_model.OnAppearing();
        }

        public void OnMore(object sender, EventArgs e)
        {
            var item = sender as Button;
            Project p = item.CommandParameter as Project;

            DependencyService.Get<Toast>().Show($"Project details {p.name}");
        }

        public async void OnEdit(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            Project p = btn.CommandParameter as Project;

            await Navigation.PushAsync(new NewProjectPage(p));
        }

        public async void OnDelete(object sender, EventArgs e) 
        {
            try
            {
                Database = await T_Rec_DB_Project.Instance;

                Button btn = sender as Button;
                Project p = btn.CommandParameter as Project;

                await Database.DeleteProjectAsync(p);

                _view_model.is_busy = true;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to delete a project \n {ex.Message}");
            }
            finally
            {
                _view_model.is_busy = false;
                Database = null;
            }
        }
    }
}