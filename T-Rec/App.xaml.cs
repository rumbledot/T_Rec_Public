using System;
using T_Rec.Converters;
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

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();

            Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();

            Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
        }

        private void SetTheme(Theme theme)
        {
            DependencyService.Get<Toast>().Show($"Current OS Theme");
            if (theme == Theme.Dark)
            {
                Resources["PrimaryText"] = Color.LightGray;
                Resources["SecondaryText"] = Color.LightGray;

                Resources["Background"] = Color.FromHex("212121");
                Resources["BackgroundLight"] = Color.FromHex("484848");
                Resources["BackgroundDark"] = Color.Black;
            }
        }
    }
}
