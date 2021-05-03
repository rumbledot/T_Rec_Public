using System;
using T_Rec.Services;
using T_Rec.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<JobsMockData>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
