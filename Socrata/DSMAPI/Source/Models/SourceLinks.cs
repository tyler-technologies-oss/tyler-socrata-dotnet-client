using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class SourceLinks
    {
        /// <summary>
        /// update
        ///</summary>
        [DataMember(Name="update")]
        public string Update { get; }

        /// <summary>
        /// show
        ///</summary>
        [DataMember(Name="show")]
        public string Show { get; set; }

        /// <summary>
        /// load
        ///</summary>
        [DataMember(Name="load")]
        public string Load { get; }

        /// <summary>
        /// input_schema_links
        ///</summary>
        [DataMember(Name="input_schema_links")]
        public InputSchemaLinks InputSchemaLinks { get; internal set; }

        /// <summary>
        /// initiate
        ///</summary>
        [DataMember(Name="initiate")]
        public string Initiate { get; internal set; }

        /// <summary>
        /// commit
        ///</summary>
        [DataMember(Name="commit")]
        public string Commit { get; internal set; }

        /// <summary>
        /// chunk
        ///</summary>
        [DataMember(Name="chunk")]
        public string Chunk { get; internal set; }

        /// <summary>
        /// cancel
        ///</summary>
        [DataMember(Name="cancel")]
        public string Cancel { get; internal set; }

        /// <summary>
        /// bytes
        ///</summary>
        [DataMember(Name="bytes")]
        public string Bytes { get; internal set; }

        /// <summary>
        /// add_to_revision
        ///</summary>
        [DataMember(Name="add_to_revision")]
        public string AddToRevision { get; internal set; }
    }
}
