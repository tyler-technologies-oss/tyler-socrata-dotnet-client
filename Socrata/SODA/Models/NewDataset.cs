using System.Runtime.Serialization;
namespace Socrata
{
    [DataContract]
    public class NewDataset
    {
        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; private set; }
    }
}
