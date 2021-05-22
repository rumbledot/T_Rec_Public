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
            Device.SetFlags(new string[] { "Shape_Experimental" });

            InitializeComponent();

            Theme theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);

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
            DependencyService.Get<Toast>().Show($"Current OS Theme {theme}");

            if (theme == Theme.Light)
            {
                DependencyService.Get<Toast>().Show($"Current OS Theme DARK");

                Resources["PrimaryText"] = Color.FromHex("ffd3d3d3");

                Resources["Background"] = Color.FromHex("212121");
                Resources["BackgroundLight"] = Color.FromHex("484848");
                Resources["BackgroundDark"] = Color.Black;
            }
            else
            {
                DependencyService.Get<Toast>().Show($"Current OS Theme LIGHT");

                Resources["PrimaryText"] = Color.FromHex("000000");

                Resources["Background"] = Color.FromHex("eeeeee");
                Resources["BackgroundLight"] = Color.FromHex("ffffff");
                Resources["BackgroundDark"] = Color.FromHex("bcbcbc");
            }
        }
    }
}
