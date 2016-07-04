using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Calendar.UWP
{
    /// <summary>
    /// Base class for calendar template selector.
    /// </summary>
    public abstract class CalendarTemplateSelectorBase : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the Calendar control.
        /// </summary>
        public FrameworkElement HostControl { get; set; }
    }
}
