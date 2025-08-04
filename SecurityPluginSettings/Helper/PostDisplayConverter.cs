using SecurityPluginSettings.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SecurityPluginSettings.Helper
{
    public class PostDisplayMultiConverter : IMultiValueConverter
    {
      
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2
                && values[0] is SettingsData post
                && values[1] is bool showId)
            {
                return showId ? post.Id.ToString() : post.UserId.ToString();
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
