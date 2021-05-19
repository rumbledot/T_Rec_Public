using System;
using System.Globalization;
using T_Rec.Helpers;
using Xamarin.Forms;

namespace T_Rec.Converters
{
    internal class IsMutedToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool job_done)
            {
                //var result = job_done ? Color.FromHex(ExtensionHelper.FindResource("AccentLight").ToString()) : Color.FromHex(ExtensionHelper.FindResource("PrimaryLight").ToString());
                var result = job_done ? Color.DarkGray : Color.LightGray;
                return result;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
