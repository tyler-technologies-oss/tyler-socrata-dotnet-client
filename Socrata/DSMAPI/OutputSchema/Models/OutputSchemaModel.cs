using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class OutputSchemaModel
    {
        /// <summary>
        /// total_rows
        ///</summary>
        [DataMember(Name="total_rows")]
        public long TotalRows { get; }

        /// <summary>
        /// sort_bys
        ///</summary>
        [DataMember(Name="sort_bys")]
        public List<string> SortBys { get; internal set; }

        /// <summary>
        /// output_columms
        ///</summary>
        [DataMember(Name="output_columns")]
        public List<OutputSchemaColumn> OutputColumns { get; set; }

        /// <summary>
        /// input_schema_id
        ///</summary>
        [DataMember(Name="input_schema_id")]
        public long InputSchemaId { get; internal set; }

        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public long Id { get; internal set; }

        /// <summary>
        /// finished_at
        ///</summary>
        [DataMember(Name="finished_at")]
        public string finished_at { get; }

        /// <summary>
        /// error_count
        ///</summary>
        [DataMember(Name="error_count")]
        public long? error_count { get; internal set; }

        /// <summary>
        /// created_by
        ///</summary>
        [DataMember(Name="created_by")]
        public Dictionary<string, string> CreatedBy { get; }

        /// <summary>
        /// created_at
        ///</summary>
        [DataMember(Name="created_at")]
        public string CreatedAt { get; }

        /// <summary>
        /// completed_at
        ///</summary>
        [DataMember(Name="completed_at")]
        public string CompletedAt { get; set; }

    }
}
