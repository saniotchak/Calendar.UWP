using System;
using Windows.UI.Xaml;

namespace Calendar.UWP
{
    /// <summary>
    /// Default template selector for a day.
    /// </summary>
    public class DefaultCalendarTemplateSelector : CalendarTemplateSelectorBase
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var resDictionary = new ResourceDictionary
            {
                Source = new Uri("ms-appx:///Calendar.UWP/Resources/CalendarResourceDictionary.xaml", UriKind.RelativeOrAbsolute)
            };            
            var template = (DataTemplate)resDictionary["DefaultCalendarItemTemplate"];
            return template;
        }
    }
}
