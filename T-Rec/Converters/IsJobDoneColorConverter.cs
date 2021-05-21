using System;
using System.Globalization;
using T_Rec.Helpers;
using Xamarin.Forms;

namespace T_Rec.Converters
{
    internal class IsJobDoneColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool job_done)
            {
                var result = job_done ? Application.Current.Resources["BackgroundDark"] : Application.Current.Resources["BackgroundLight"];
                Console.WriteLine(result);
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
