using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class InputColumn
    {
        /// <summary>
        /// soql_type
        ///</summary>
        [DataMember(Name="soql_type")]
        public string SoqlType { get; set; }

        /// <summary>
        /// semantic_type
        ///</summary>
        [DataMember(Name="semantic_type")]
        public string SemanticType { get; set; }

        /// <summary>
        /// position
        ///</summary>
        [DataMember(Name="position")]
        public long Position { get; set; }

        /// <summary>
        /// input_schema_id
        ///</summary>
        [DataMember(Name="input_schema_id")]
        public long InputSchemaId { get; set; }

        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public long Id { get; set; }

        /// <summary>
        /// field_name
        ///</summary>
        [DataMember(Name="field_name")]
        public string FieldName { get; set; }

        /// <summary>
        /// display_name
        ///</summary>
        [DataMember(Name="display_name")]
        public string DisplayName { get; set; }
    }
}
