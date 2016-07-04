using System;

namespace Calendar.UWP.Sample.Data
{
    /// <summary>
    /// A dummy data item.
    /// </summary>
    public class SampleDataItem
    {
        /// <summary>
        /// Green, orange or none color mark on a day.
        /// </summary>
        public enum ItemTypes
        {
            Green,
            Orange,
            None            
        }

        public ItemTypes ItemType { get; set; }

        /// <summary>
        /// Date is necessary in order to identify the item in the calendar.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
