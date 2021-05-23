using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Data;
using T_Rec.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectPage : ContentPage
    {
        public T_Rec_DB_Project Database { get; set; }

        Project selected_project;

        public ProjectPage()
        {
            InitializeComponent();
        }

        public ProjectPage(Project p)
        {
            selected_project = p;

            InitializeComponent();

            lbl_Project_name.Text = p.name;
            lbl_Project_status.Text = p.project_status.ToString();
            lbl_Project_tracked_label.IsVisible= p.billable;
            lbl_Project_start.Text = $"Start : {p.project_started.ToString("dd MMM yyyy")}";
            lbl_Project_end.Text = $"End : {p.project_ended.ToString("dd MMM yyyy")}";
            lbl_Project_end.IsVisible = p.project_ended > p.project_started;
            lbl_Project_end_open.IsVisible = p.project_ended == p.project_started;
            lbl_Project_company.Text = p.company_name;
            lbl_Project_description.Text = p.description;
            lbl_Project_note.Text = p.project_status_note;
        }

        public async void OnJobList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodayJobsPage(selected_project));
        }

        public async void OnMore(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage(selected_project));
        }

        public async void OnEdit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewProjectPage(selected_project));
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            try
            {
                Database = await T_Rec_DB_Project.Instance;

                await Database.DeleteProjectAsync(selected_project);
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to delete a project \n {ex.Message}");
            }
            finally
            {
                Database = null;
                await Navigation.PopAsync();
            }
        }
    }
}