using Calendar.UWP.Sample.Controls;
using Calendar.UWP.Sample.Data;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Calendar.UWP.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This is an example how to handle day click.
        /// <para>
        /// There is also an alternative way: using SelectedDate property.
        /// </para>
        /// </summary>
        /// <param name="sender">FrameworkElement representing the selected day.</param>
        /// <param name="e"></param>
        private void DayTappedHandler(object sender, TappedRoutedEventArgs e)
        {
            // A sample context menu.
            var contextMenu = new Menu();
            contextMenu.ItemClick += (s, args) =>
            {
                var dayContext = (DayViewItem)((FrameworkElement)sender).DataContext;
                // if the selected date is not yet in the items source, adding it.
                if (!(from item in (SampleDataSource)SampleCalendar.DataContext
                      where item.Date.Date.Equals(dayContext.Date.Date)
                      select item).Any())
                {
                    // If the selection is "none", doing nothing.
                    if (args == SampleDataItem.ItemTypes.None) return;
                    ((SampleDataSource)SampleCalendar.DataContext).Add(new SampleDataItem { Date = dayContext.Date, ItemType = args });
                }
                else
                {
                    // If the date is already in the list of items, changing the item property (color in our case).
                    foreach (var item in (SampleDataSource)SampleCalendar.DataContext)
                    {
                        if (item.Date.Date.Equals(dayContext.Date.Date))
                        {
                            // If the color is the same as selected in the menu, doing nothing.
                            if (item.ItemType == args) return;
                            item.ItemType = args;
                            break;
                        }
                    }
                }
                // Explicitely refreshing the calendar to display changes.
                SampleCalendar.RefreshCalendar();
            };
            Menu.Show(contextMenu,(FrameworkElement)sender);
        }
    }
}
