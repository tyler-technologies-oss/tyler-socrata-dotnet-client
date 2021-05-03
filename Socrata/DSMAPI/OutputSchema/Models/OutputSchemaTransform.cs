using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class OutputSchemaColumnTransform
    {
            /// <summary>
        /// transform_input_columns
        ///</summary>
        [DataMember(Name="transform_input_columns")]
        public List<TransformInputColumn> TransformInputColumns { get; set; }

        /// <summary>
        /// transform_expr
        ///</summary>
        [DataMember(Name="transform_expr")]
        public string TransformExpr { get; set; }

        /// <summary>
        /// parsed_expr
        ///</summary>
        [DataMember(Name="parsed_expr")]
        public TransformParsedExpression ParsedExpr { get; set; }

        /// <summary>
        ///output_soql_type
        ///</summary>
        [DataMember(Name="output_soql_type")]
        public string OutputSoqlType { get; }

        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public int Id { get; internal set; }

        /// <summary>
        /// has_cell_edits
        ///</summary>
        [DataMember(Name="has_cell_edits")]
        public bool HasCellEdits  { get; }

        /// <summary>
        /// finished_at
        ///</summary>
        [DataMember(Name="finished_at")]
        public string FinishedAt  { get; }

        /// <summary>
        /// failure_details
        ///</summary>
        [DataMember(Name="failure_details")]
        public string FailureDetails  { get; }

        /// <summary>
        /// failed_at
        ///</summary>
        [DataMember(Name="failed_at")]
        public string FailedAt  { get; }

        /// <summary>
        /// error_count
        ///</summary>
        [DataMember(Name="error_count")]
        public int? ErrorCount  { get; set; }

        /// <summary>
        /// completed_at
        ///</summary>
        [DataMember(Name="completed_at")]
        public string CompletedAt  { get; }

        /// <summary>
        /// attempts
        ///</summary>
        [DataMember(Name="attempts")]
        public int Attempts  { get; }
    }
}
