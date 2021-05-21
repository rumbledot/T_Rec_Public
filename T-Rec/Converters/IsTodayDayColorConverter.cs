using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace T_Rec.Converters
{
    internal class IsTodayDayColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool today_day)
            {
                var result = today_day ? Application.Current.Resources["Primary"] : Application.Current.Resources["BackgroundLight"];
                //Console.WriteLine("today day converter " + today_day + " : color : " + result);
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
