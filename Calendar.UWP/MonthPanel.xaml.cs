using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Calendar.UWP
{
    /// <summary>
    /// Represents a month UI in the calendar.
    /// </summary>
    public sealed partial class MonthPanel : UserControl
    {
        public MonthPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Day tapped event.
        /// </summary>
        public event EventHandler<TappedRoutedEventArgs> DayTapped;

        /// <summary>
        /// Fires day tapped event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDayTapped(object sender, TappedRoutedEventArgs e)
        {
            DayTapped?.Invoke(sender, e);
        }

        /// <summary>
        /// Handles item (day) tap.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemTappedHandler(object sender, TappedRoutedEventArgs e)
        {
            OnDayTapped(sender, e);
        }
    }
}
