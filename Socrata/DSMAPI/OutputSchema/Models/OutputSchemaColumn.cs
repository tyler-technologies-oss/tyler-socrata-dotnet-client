using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class OutputSchemaColumn
    {
        /// <summary>
        /// transform
        ///</summary>
        [DataMember(Name="transform")]
        public OutputSchemaColumnTransform Transform { get; internal set; }
        
        /// <summary>
        /// position
        ///</summary>
        [DataMember(Name="position")]
        public int Position { get; set; }

        /// <summary>
        /// is_primary_key
        ///</summary>
        [DataMember(Name="is_primary_key")]
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// initial_output_column_id
        ///</summary>
        [DataMember(Name="initial_output_column_id")]
        public int? InitialOutputColumnId { get; set; }

        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// format
        ///</summary>
        [DataMember(Name="format")]
        public object Format { get; set; }

        /// <summary>
        /// flags
        ///</summary>
        [DataMember(Name="flags")]
        public List<object> Flags { get; set; }

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

        /// <summary>
        /// description
        ///</summary>
        [DataMember(Name="description")]
        public string Description { get; set; }
    }
}
