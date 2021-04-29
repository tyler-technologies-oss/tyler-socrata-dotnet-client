using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class SourceResponse
    {
        /// <summary>
        /// Resource
        ///</summary>
        [DataMember(Name="resource")]
        public SourceResource Resource { get; set; }

        /// <summary>
        /// Links
        ///</summary>
        [DataMember(Name="links")]
        public SourceLinks Links { get; set; }

    }
}
