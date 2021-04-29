using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class InputSchema
    {
        /// <summary>
        /// total_rows
        ///</summary>
        [DataMember(Name="total_rows")]
        public int TotalRows { get; }
        
        /// <summary>
        /// output_schemas
        ///</summary>
        [DataMember(Name="output_schemas")]
        public List<OutputSchemaModel> OutputSchemas { get; set; }

        public OutputSchemaModel GetLatestOutputSchema()
        {
            return OutputSchemas.OrderByDescending(x => x.Id).First();
        }

        /// <summary>
        /// num_row_errors
        ///</summary>
        [DataMember(Name="num_row_errors")]
        public int NumRowErrors { get; }

        /// <summary>
        /// name
        ///</summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// input_columns
        ///</summary>
        [DataMember(Name="input_columns")]
        public List<InputColumn> InputColumns { get; set; }

        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public int Id { get; }

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

    }
}
