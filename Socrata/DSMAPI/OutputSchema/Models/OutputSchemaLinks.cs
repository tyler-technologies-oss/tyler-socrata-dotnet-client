using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class OutputSchemaLinks
    {
        /// <summary>
        /// validate_row_id
        ///</summary>
        [DataMember(Name="validate_row_id")]
        public string ValidateRowId { get; }

        /// <summary>
        /// show
        ///</summary>
        [DataMember(Name="show")]
        public string Show { get; }

        /// <summary>
        /// schema_errors
        ///</summary>
        [DataMember(Name="schema_errors")]
        public string SchemaErrors { get; internal set; }

        /// <summary>
        /// rows
        ///</summary>
        [DataMember(Name="rows")]
        public string Rows { get; }

        /// <summary>
        /// build_config
        ///</summary>
        [DataMember(Name="build_config")]
        public string BuildConfig { get; }

    }
}
