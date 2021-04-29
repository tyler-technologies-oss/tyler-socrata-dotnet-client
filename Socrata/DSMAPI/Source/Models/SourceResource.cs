using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class SourceResource
    {
        /// <summary>
        /// Source Type
        ///</summary>
        [DataMember(Name="source_type")]
        public SourceTypeJson SourceType { get; set; }

        /// <summary>
        /// schemas
        ///</summary>
        [DataMember(Name="schemas")]
        public List<InputSchema> InputSchemas { get; set; }

        public InputSchema GetLatestInputSchema()
        {
            return this.InputSchemas.OrderByDescending(x => x.Id).First();
        }

        /// <summary>
        /// parse_options
        ///</summary>
        [DataMember(Name="parse_options")]
        public SourceParseOptions ParseOptions { get; set; }

        /// <summary>
        /// locale
        ///</summary>
        [DataMember(Name="locale")]
        public string Locale { get; set; }

        /// <summary>
        /// is_delete
        ///</summary>
        [DataMember(Name="is_delete")]
        public bool IsDelete { get; }

        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public string Id { get; }

        /// <summary>
        /// finished_at
        ///</summary>
        [DataMember(Name="finished_at")]
        public string FinishedAt { get; }

        /// <summary>
        /// filesize
        ///</summary>
        [DataMember(Name="filesize")]
        public int Filesize { get; }

        /// <summary>
        /// failure_details
        ///</summary>
        [DataMember(Name="failure_details")]
        public string FailureDetails { get; }

        /// <summary>
        /// failed_at
        ///</summary>
        [DataMember(Name="failed_at")]
        public string FailedAt { get; }

        /// <summary>
        /// export_filename
        ///</summary>
        [DataMember(Name="export_filename")]
        public string ExportFilename { get; }

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
        /// content_type
        ///</summary>
        [DataMember(Name="content_type")]
        public string ContentType { get; }

        /// <summary>
        /// columns
        ///</summary>
        [DataMember(Name="columns")]
        public List<object> Columns { get; set; }

        /// <summary>
        /// blob
        ///</summary>
        [DataMember(Name="blob")]
        public string Blob { get; }
    }
}
