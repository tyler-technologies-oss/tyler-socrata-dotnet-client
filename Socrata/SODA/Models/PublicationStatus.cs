using System.Runtime.Serialization;

namespace Socrata
{
    [DataContract]
    public class PublicationStatus
    {
        /// <summary>
        /// Gets the unix timestamp when request was created
        /// </summary>
        [DataMember(Name = "createdAt")]
        public string CreatedAt { get; }

        /// <summary>
        /// Gets the Request Id for the request
        /// </summary>
        [DataMember(Name = "requestId")]
        public string RequestId { get; }

        /// <summary>
        /// Gets the status of the request
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; }

        /// <summary>
        /// Gets the 4x4 of the underlying asset
        /// </summary>
        [DataMember(Name = "uid")]
        public string Uid { get; }
    }
}