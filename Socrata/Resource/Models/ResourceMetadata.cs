using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Socrata
{
    using Socrata.SODA.Schema;
    
    [DataContract]
    public class ResourceMetadata
    {
        /// <summary>
        /// Gets or sets the name of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the Row Id of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Row Id of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "rowid")]
        public string RowId { get; set; }

        /// <summary>
        /// Gets or sets the id of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the Resource that this ResourceMetadata describes.
        /// </summary>
        [DataMember(Name = "columns")]
        public List<ColumnMetadata> Columns { get; set; }
    }
}
