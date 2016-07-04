using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml.Controls;

namespace Calendar.UWP
{
    /// <summary>
    /// This class provides methods to load data in the calendar.
    /// </summary>
    public class CalendarDataAssistant
    {
        /// <summary>
        /// Gets the list of DayViewItem for a month.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="itemTemplateSelector"></param>
        /// <returns></returns>
        public static IEnumerable<DayViewItem> GetMonthsDays(int year, int month, DataTemplateSelector itemTemplateSelector = null)
        {
            var firstDayOfWeekDate = GetFirstDayOfWeekDate(new DateTime(year, month, 1));
            return GetMonthsDaysFromDate(firstDayOfWeekDate, itemTemplateSelector);
        }

        /// <summary>
        /// Gets the date corresponding to the first day of weelk that will be displayed in the calendar.
        /// </summary>
        /// <param name="monthStartDate"></param>
        /// <returns></returns>
        static DateTime GetFirstDayOfWeekDate(DateTime monthStartDate)
        {
            return monthStartDate.AddDays(0 - GetDaysFromWeekStart(monthStartDate));
        }

        /// <summary>
        /// Gets the number of days between the provided date and the week start.
        /// </summary>
        /// <param name="monthStartDate"></param>
        /// <returns></returns>
        static int GetDaysFromWeekStart(DateTime monthStartDate)
        {
            var mondayIsFirstWeekDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday;
            if (CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday && monthStartDate.DayOfWeek == DayOfWeek.Sunday) return 6;
            return (int)monthStartDate.DayOfWeek + (mondayIsFirstWeekDay ? -1 : 0);
        }

        /// <summary>
        /// Gets the list of DayViewItem for a month.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="itemTemplateSelector"></param>
        /// <returns></returns>
        public static IEnumerable<DayViewItem> GetMonthsDaysFromDate(DateTime startDate, DataTemplateSelector itemTemplateSelector = null)
        {
            var days = new List<DayViewItem>();
            var date = startDate;
            var columnIndex = 0;
            var rowIndex = 0;
            var currentMonth = date.Day == 1 ? date.Month : (date.Month < 12 ? date.Month + 1 : 1);
            for (var i = 0; i < 42; i++)
            {
                if (columnIndex == 7)
                {
                    columnIndex = 0;
                    rowIndex++;
                }
                var dayItem = new DayViewItem
                {
                    Date = date,
                    ColumnIndex = columnIndex,
                    RowIndex = rowIndex,
                    IsInOtherView = date.Month != currentMonth
                };
                if(itemTemplateSelector != null)
                {
                    dayItem.DayTemplateSelector = itemTemplateSelector;
                }
                days.Add(dayItem);
                date = date.AddDays(1);
                columnIndex++;
            }
            return days;
        }
    }
}
