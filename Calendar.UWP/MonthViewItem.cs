using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml.Controls;

namespace Calendar.UWP
{
    /// <summary>
    /// Represents a month (model) in the calendar.
    /// </summary>
    public class MonthViewItem
    {
        public MonthViewItem(int month, int year, DataTemplateSelector itemTemplateSelector = null)
        {
            Month = month;
            Year = year;
            LoadDays(itemTemplateSelector);
        }

        /// <summary>
        /// Loads days for this month.
        /// </summary>
        /// <param name="itemTemplateSelector"></param>
        void LoadDays(DataTemplateSelector itemTemplateSelector = null)
        {
            Days = CalendarDataAssistant.GetMonthsDays(Year, Month, itemTemplateSelector);
        }

        /// <summary>
        /// Gets or sets the days list.
        /// </summary>
        public IEnumerable<DayViewItem> Days { get; private set; }

        /// <summary>
        /// Gets or sets the month #.
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// Gets or sets the year #.
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// Overrides the base method in order to display month and year.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new DateTime(Year, Month, 1).ToString("MMMM yyyy", CultureInfo.CurrentCulture);
        }
    }
}
