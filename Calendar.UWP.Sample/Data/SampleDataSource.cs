using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Calendar.UWP.Sample.Data
{
    /// <summary>
    /// This represents a calendar datasource for demonstration purposes.
    /// <para>
    /// It contains only days that should be marked somehow in the calendar.
    /// </para>
    /// </summary>
    public class SampleDataSource : List<SampleDataItem>
    {
        public SampleDataSource()
        {
            Task.Run(() => LoadData()).Wait();            
        }

        async Task LoadData()
        {
            var serializer = new DataContractJsonSerializer(typeof(SampleDataItem[]));
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/SampleData.json", UriKind.RelativeOrAbsolute));
            using (var stream = await file.OpenStreamForReadAsync())
            {
                AddRange((SampleDataItem[])serializer.ReadObject(stream));
            }
        }
    }
}
