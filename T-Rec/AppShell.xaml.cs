using System;
using System.Collections.Generic;
using T_Rec.ViewModels;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        T_Rec_DB_Job Database;

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewJobPage), typeof(NewJobPage));
            Routing.RegisterRoute(nameof(NewProjectPage), typeof(NewProjectPage));
            Routing.RegisterRoute(nameof(NewCompanyPage), typeof(NewCompanyPage));

            CloseJobs();
        }

        public async void CloseJobs() 
        {
            try
            {
                //closing job that isn't done yesterday.
                Database = await T_Rec_DB_Job.Instance;

                var unclosed = await Database.CloseUpJobs();

                if (unclosed != null && unclosed.Count > 0) 
                {
                    foreach (var job in unclosed)
                    {
                        job.job_done = true;
                        await Database.SaveJobAsync(job);
                    }

                    DependencyService.Get<Toast>().Show($"Closing {unclosed.Count} jobs");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Closing jobs failed \n {ex.Message}");
            }
            finally 
            {
                Database = null;
            }
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
