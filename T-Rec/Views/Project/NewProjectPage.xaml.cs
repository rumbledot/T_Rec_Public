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
    public partial class NewProjectPage : ContentPage
    {
        public T_Rec_DB_Project Database { get; set; }

        public T_Rec_DB_Company Database_Company { get; set; }

        public Project edited_project { get; set; }

        Dictionary<int, string> _companies_picker = new Dictionary<int, string>();

        public List<string> companies { get; set; }

        public int selected_company_id
        {
            get { return _companies_picker.ElementAt(picker_Company.SelectedIndex).Key; }
        }

        public static Dictionary<string, ProjectStatus> _project_statusses 
            = new Dictionary<string, ProjectStatus>()
                {
                    { "Active"      , ProjectStatus.active    },
                    { "Pending"     , ProjectStatus.pending   },
                    { "Cancelled"   , ProjectStatus.cancelled },
                    { "Finished"    , ProjectStatus.finished  },
                    { "Stopped"     , ProjectStatus.stop      }
                };

        public List<string> statusses { get; set; } = _project_statusses.Keys.ToList();

        public ProjectStatus selected_status
        {
            get { return _project_statusses.ElementAt(picker_Project_status.SelectedIndex).Value; }
        }

        public NewProjectPage()
        {
            InitializeComponent();
        }

        public NewProjectPage(Project p)
        {
            InitializeComponent();

            edited_project = p;
        }

        protected override void OnAppearing()
        {
            picker_Project_status.ItemsSource = statusses;
            picker_Project_status.SelectedIndex = 0;

            LoadCompanies();

            dtpicker_Start_date.MinimumDate = DateTime.Now;
            dtpicker_Start_date.MaximumDate = DateTime.Now.AddMonths(6);
            dtpicker_Start_date.Date = DateTime.Now;

            dtpicker_End_date.IsEnabled = false;

            dtpicker_Start_date.DateSelected += Dtpicker_Start_date_DateSelected;
            dtpicker_End_date.DateSelected += Dtpicker_End_date_DateSelected;

            if (edited_project != null)
            {
                tbox_Project_name.Text = edited_project.name;
                tbox_Project_description.Text = edited_project.description;
                picker_Project_status.SelectedIndex = GetStatusPickerIndex(edited_project.project_status);
                tbox_Note.Text = edited_project.project_status_note;
                switch_Billable.IsToggled = edited_project.billable;
                dtpicker_Start_date.Date = edited_project.project_started;
                dtpicker_End_date.Date = edited_project.project_ended;
            }
        }

        private async void LoadCompanies()
        {
            try
            {
                this.Database_Company = await T_Rec_DB_Company.Instance;

                List<Company> result = await Database_Company.GetCompanies();

                foreach (var c in result)
                {
                    _companies_picker.Add(c.company_id, c.name);
                }

                companies = _companies_picker.Values.ToList();

                picker_Company.ItemsSource = companies;

                picker_Company.SelectedIndex = edited_project != null ? GetCompanyPickerIndex(edited_project.company_id) : 0;
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Error loading companies \n {ex.Message}");
            }
            finally 
            {
                Database_Company = null;
            }
        }

        private int GetCompanyPickerIndex(int company_id)
        {
            int index = 0;

            foreach (var item in _companies_picker)
            {
                if (item.Key == company_id) return index;
                index++;
            }

            return index;
        }

        private int GetStatusPickerIndex(ProjectStatus project_status)
        {
            int index = 0;

            foreach (var item in _project_statusses)
            {
                if (item.Value == project_status) return index;
                index++;
            }

            return index;
        }

        private void Picker_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Dtpicker_Start_date_DateSelected(object sender, DateChangedEventArgs e)
        {
            DatePicker dt = sender as DatePicker;

            dtpicker_End_date.IsEnabled = true;
            dtpicker_End_date.MinimumDate = dt.Date;
            dtpicker_End_date.MaximumDate = dt.Date.AddMonths(6);
            dtpicker_End_date.Date = dt.Date;
        }

        private void Dtpicker_End_date_DateSelected(object sender, DateChangedEventArgs e)
        {
            DatePicker dt = sender as DatePicker;
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                this.Database = await T_Rec_DB_Project.Instance;

                Project p = new Project()
                {
                    project_id = edited_project != null ? edited_project.project_id : -1,
                    company_id = selected_company_id,
                    name = tbox_Project_name.Text,
                    description = tbox_Project_description.Text,
                    project_status = selected_status,
                    project_status_note = tbox_Note.Text,
                    billable = switch_Billable.IsToggled,
                    project_started = dtpicker_Start_date.Date,
                    project_ended = dtpicker_End_date.IsEnabled ? dtpicker_End_date.Date : dtpicker_Start_date.Date
                };

                await Database.SaveProjectAsync(p);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to save company \n {ex.Message}");
            }
            finally 
            {
                Database = null;
            }
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}