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
    public partial class JobPage : ContentPage
    {
        public JobPage()
        {
            InitializeComponent();
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            JobUnit j = new JobUnit() 
            {
                project_name = tbox_Project_name.Text,
                description = tbox_Description.Text,
                time_start = DateTime.Now,
                job_done = false
            };

            T_Rec_Database database = await T_Rec_Database.Instance;

            await database.SaveItemAsync(j);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}