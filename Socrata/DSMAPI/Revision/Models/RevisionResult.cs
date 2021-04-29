using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class RevisionResult
    {
        /// <summary>
        /// Resource
        ///</summary>
        [DataMember(Name="resource")]
        public RevisionResource Resource { get; set; }
    }
}
