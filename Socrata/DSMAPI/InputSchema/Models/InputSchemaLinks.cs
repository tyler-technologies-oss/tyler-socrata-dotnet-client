using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class InputSchemaLinks
    {
        /// <summary>
        /// transform
        ///</summary>
        [DataMember(Name="transform")]
        public string Transform { get; }

        /// <summary>
        /// show
        ///</summary>
        [DataMember(Name="show")]
        public string Show { get; }

        /// <summary>
        /// row_errors
        ///</summary>
        [DataMember(Name="row_errors")]
        public string RowErrors { get; }

        /// <summary>
        /// output_schema_links
        ///</summary>
        [DataMember(Name="output_schema_links")]
        public OutputSchemaLinks OutputSchemaLinks { get; internal set; }

        /// <summary>
        /// latest_output
        ///</summary>
        [DataMember(Name="latest_output")]
        public string LatestOutput { get; }
    }
}
