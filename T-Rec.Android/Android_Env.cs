using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.Content.Res;
using Plugin.CurrentActivity;
using Xamarin.Forms;

using T_Rec;
using T_Rec.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Android_Env))]
namespace T_Rec.Droid
{
    public class Android_Env : IEnvironment
    {
        public Task<Theme> GetOperatingSystemThemeAsync() =>
            Task.FromResult(GetOperatingSystemTheme());

        public Theme GetOperatingSystemTheme()
        {
            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var ui_mode_flags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (ui_mode_flags)
                {
                    case UiMode.NightYes:
                        return Theme.Dark;

                    case UiMode.NightNo:
                        return Theme.Light;

                    default:
                        throw new NotSupportedException($"UiMode {ui_mode_flags} not supported");
                }
            }
            else
            {
                return Theme.Light;
            }
        }
    }
}