﻿using System;
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
    public partial class NewJobPage : ContentPage
    {
        public T_Rec_DB_Job Database { get; set; }
        public T_Rec_DB_Project Database_Project { get; set; }

        Dictionary<Project, string> _projects_picker = new Dictionary<Project, string>();
        public List<string> projects { get; set; }
        public Project selected_project
        {
            get { return _projects_picker.ElementAt(picker_Active_projects.SelectedIndex).Key; }
        }

        public NewJobPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                await LoadPickerProjects();

                picker_Active_projects.SelectedIndex = 0;

                picker_Active_projects.SelectedIndexChanged += Picker_Active_projects_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Error loading companies \n {ex.Message}");
            }
            finally
            {
                Database_Project = null;
            }
        }

        protected override void OnDisappearing()
        {
            try
            {
                picker_Active_projects.SelectedIndexChanged -= Picker_Active_projects_SelectedIndexChanged;

                base.OnDisappearing();
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Disappearing exception \n {ex.Message}");
            }
        }

        private async Task LoadPickerProjects()
        {
            this.Database_Project = await T_Rec_DB_Project.Instance;

            List<Project> result = await Database_Project.GetActiveProjects();

            foreach (var p in result)
            {
                _projects_picker.Add(p, p.name);
            }

            projects = _projects_picker.Values.ToList();

            picker_Active_projects.ItemsSource = projects;
        }

        private async void Picker_Active_projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (picker_Active_projects.ItemsSource == null || picker_Active_projects.ItemsSource.Count <= 0)
                {
                    await LoadPickerProjects();
                }

                button_Is_billable.IsVisible = selected_project.billable;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to load picker Projects \n {ex.Message}");
            }
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                this.Database = await T_Rec_DB_Job.Instance;

                JobUnit j = new JobUnit()
                {
                    project_id = selected_project.project_id,
                    billable = selected_project.billable,
                    description = tbox_Description.Text,
                    time_start = DateTime.Now,
                    job_done = false
                };

                await Database.SaveJobAsync(j);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to save job \n {ex.Message}");
            }
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}