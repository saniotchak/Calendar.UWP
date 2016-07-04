using Calendar.UWP.Sample.Data;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Calendar.UWP.Sample.Controls
{
    /// <summary>
    /// A context menu for demonstration purposes.
    /// </summary>
    public sealed partial class Menu : UserControl
    {
        public event EventHandler<SampleDataItem.ItemTypes> ItemClick;

        public Menu()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionHandler(object sender, RoutedEventArgs e)
        {
            // Activating the click event and passing the button command parameter, which corresponds to SampleDataItem.ItemTypes enum.
            OnItemClick((SampleDataItem.ItemTypes)int.Parse((string)((AppBarButton)sender).CommandParameter));
        }

        /// <summary>
        /// Activates the item click event.
        /// </summary>
        /// <param name="args"></param>
        void OnItemClick(SampleDataItem.ItemTypes args)
        {
            ItemClick?.Invoke(this, args);
        }

        /// <summary>
        /// Shows the menu in a popup at the position relative to the provided element (day of the claendar in this particular case).
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="showRelatedTo"></param>
        public static void Show(Menu menu, FrameworkElement showRelatedTo)
        {
            // Getting the element position relative to the window.
            var point = showRelatedTo.TransformToVisual(Window.Current.Content).TransformPoint(new Point(-50, -70));
            var popup = new Popup { Child = menu, HorizontalOffset = point.X, VerticalOffset = point.Y, IsLightDismissEnabled = true };
            menu.ItemClick += (sender, e) =>
            {
                popup.IsOpen = false;
            };
            popup.IsOpen = true;
        }
    }
}
