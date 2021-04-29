using System.Runtime.Serialization;

namespace Socrata.SODA
{
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "Rows Updated")]
        public int Updated { get; private set; }

        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "Rows Created")]
        public int Created { get; private set; }

        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "Rows Deleted")]
        public int Deleted { get; private set; }

        [DataMember(Name = "Errors")]
        public int Errors { get; private set; }

        /// Gets the explanatory text about this result.
        /// </summary>
        /// <summary>
        [DataMember(Name = "message")]
        public string Message { get; internal set; }

        /// <summary>
        /// Gets a flag indicating if one or more errors occured.
        /// </summary>
        [DataMember(Name = "error")]
        public bool IsError { get; internal set; }

        /// <summary>
        /// Gets data about any errors that occured.
        /// </summary>
        [DataMember(Name = "code")]
        public string ErrorCode { get; internal set; }

    }
}
