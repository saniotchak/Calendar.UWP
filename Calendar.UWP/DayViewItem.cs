using Calendar.UWP.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Calendar.UWP
{
    /// <summary>
    /// Represents one day (model) in the calendar.
    /// </summary>
    public class DayViewItem : BindableBase
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets the text label.
        /// </summary>
        public string Label
        {
            get
            {
                return Date.Day.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the column index.
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets the row index.
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets whether the day is not in current month (is in another view).
        /// </summary>
        public bool IsInOtherView { get; set; }

        DataTemplateSelector _dayTemplateSelector = new DefaultCalendarTemplateSelector();

        /// <summary>
        /// Gets or sets the day template selector.
        /// </summary>
        public DataTemplateSelector DayTemplateSelector
        {
            get
            {
                return _dayTemplateSelector;
            }
            set
            {
                _dayTemplateSelector = value;
                OnPropertyChanged();
            }
        }

        Thickness _margin = new Thickness(0);

        /// <summary>
        /// Gets or sets margin.
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                OnPropertyChanged();
            }
        }

    }
}
