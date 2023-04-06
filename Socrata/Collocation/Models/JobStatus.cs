using System.Runtime.Serialization;

namespace Socrata.Collocation
{
    [DataContract]
    public class JobStatus
    {
        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="jobId")]
        public string JobId { get; internal set; }

                /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="status")]
        public string Status { get; internal set; }

        /// <summary>
        /// activity_type
        ///</summary>
        [DataMember(Name="message")]
        public string Message { get; internal set; }
    }
}
