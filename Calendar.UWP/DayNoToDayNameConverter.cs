using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Calendar.UWP
{
    public class DayNoToDayNameConverter : IValueConverter
    {
        /// <summary>
        /// Gets the abbreaviation of the day name depending on culture.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var mondayIsFirstWeekDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday;
            var dayNo = int.Parse((string)parameter);
            string dayName = "";
            DayOfWeek dayOfWeek;
            if (dayNo == 1)
            {
                dayOfWeek = mondayIsFirstWeekDay ? DayOfWeek.Monday : DayOfWeek.Sunday;
            }
            else if (dayNo == 7)
            {
                dayOfWeek = mondayIsFirstWeekDay ? DayOfWeek.Sunday : DayOfWeek.Saturday;
            }
            else
            {
                dayOfWeek = mondayIsFirstWeekDay ? (DayOfWeek)dayNo : (DayOfWeek)(dayNo - 1);
            }
            dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(dayOfWeek);
            return dayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
