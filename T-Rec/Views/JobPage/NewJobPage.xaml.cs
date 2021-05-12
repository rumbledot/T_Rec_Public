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
    public partial class NewJobPage : ContentPage
    {
        public T_Rec_DB_Job Database { get; set; }
        public NewJobPage()
        {
            InitializeComponent();
        }

        public async void OnAppearing()
        {
            this.Database = await T_Rec_DB_Job.Instance;
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            this.Database = await T_Rec_DB_Job.Instance;

            JobUnit j = new JobUnit() 
            {
                description = tbox_Description.Text,
                time_start = DateTime.Now,
                job_done = false
            };

            T_Rec_DB_Job database = await T_Rec_DB_Job.Instance;

            await database.SaveItemAsync(j);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}