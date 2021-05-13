using System;
using System.Collections.Generic;
using T_Rec.ViewModels;
using T_Rec.Views;
using Xamarin.Forms;

namespace T_Rec
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewJobPage), typeof(NewJobPage));
            Routing.RegisterRoute(nameof(NewProjectPage), typeof(NewProjectPage));
            Routing.RegisterRoute(nameof(NewCompanyPage), typeof(NewCompanyPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
