using System.Runtime.Serialization;

namespace Socrata.SODA
{
    [DataContract]
    public class RowCount
    {
        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "Count")]
        public long Count { get; private set; }
    }
}
