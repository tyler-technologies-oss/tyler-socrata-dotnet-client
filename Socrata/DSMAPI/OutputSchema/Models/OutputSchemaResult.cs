using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class OutputSchemaResult
    {
        /// <summary>
        /// Resource
        ///</summary>
        [DataMember(Name="resource")]
        public OutputSchemaModel Resource { get; set; }

        /// <summary>
        /// Links
        ///</summary>
        [DataMember(Name="links")]
        public InputSchemaLinks Links { get; set; }
    }
}
