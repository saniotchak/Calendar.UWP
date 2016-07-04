using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Calendar.UWP
{
    public class DayToBorderThicknessConverter : IValueConverter
    {
        /// <summary>
        /// Gets the day frame border thickness.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>2 if today; 0 if not today.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try {
                if (((DateTime)value).Date == DateTime.Now.Date)
                {
                    return new Thickness(2);
                }
            }
            catch { }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
