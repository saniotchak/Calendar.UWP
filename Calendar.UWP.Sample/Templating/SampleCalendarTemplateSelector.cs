using Calendar.UWP.Sample.Data;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Calendar.UWP.Sample.Templating
{
    /// <summary>
    /// Template selector for the calendar.
    /// <para>
    /// Templates are set in \Templating\SampleTemplates.xaml.
    /// </para>
    /// </summary>
    public class SampleCalendarTemplateSelector : CalendarTemplateSelectorBase
    {
        /// <summary>
        /// Default day template.
        /// </summary>
        public DataTemplate DefaultTemplate { get; set; }

        /// <summary>
        /// Green day template.
        /// </summary>
        public DataTemplate GreenTemplate { get; set; }

        /// <summary>
        /// Orange day template.
        /// </summary>
        public DataTemplate OrangeTemplate { get; set; }

        /// <summary>
        /// Overriding the base method of template selection in order to set the logic for the particular case using new styles. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate SelectTemplateCore(object sender, DependencyObject container)
        {
            // Getting the items source bound to the calendar.
            var itemsSource = HostControl.DataContext as IEnumerable<SampleDataItem>;

            // Getting the data context of day element.
            var dayContext = (DayViewItem)((FrameworkElement)container).DataContext;

            // Setting Green template where needed.
            if ((from item in itemsSource
                 where item.Date.Date.Equals(dayContext.Date.Date) && item.ItemType == SampleDataItem.ItemTypes.Green
                 select item).Any()) return GreenTemplate;

            // Setting Orange template.
            if ((from item in itemsSource
                 where item.Date.Date.Equals(dayContext.Date.Date) && item.ItemType == SampleDataItem.ItemTypes.Orange
                 select item).Any()) return OrangeTemplate;

            // Setting default template for all other days.
            return DefaultTemplate;
        }
    }
}
