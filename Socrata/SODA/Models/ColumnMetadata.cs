using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Socrata
{
    [DataContract]
    public class ColumnMetadata
    {
        /// <summary>
        /// Gets or sets the id of the Column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "id")]
        public string id { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the Column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the data type name of the Column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "dataTypeName")]
        public string dataTypeName { get; set; }

        /// <summary>
        /// Gets or sets the description of the column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "description")]
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the fieldName of the column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "fieldName")]
        public string fieldName { get; set; }

        /// <summary>
        /// Gets or sets the position of the column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "position")]
        public string position { get; set; }
        
        /// <summary>
        /// Gets or sets the renderTypeName of the column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "renderTypeName")]
        public string renderTypeName { get; set; }

        /// <summary>
        /// Gets or sets the tableColumnId of the column that this ColumnMetadata describes.
        /// </summary>
        [DataMember(Name = "tableColumnId")]
        public string tableColumnId { get; set; }

        /// fields returned in case of an error
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "error")]
        public string error { get; set; }

        [DataMember(Name = "message")]
        public string message { get; set; }


        public Column ToColumn()
        {
            return new Column(name, SocrataDataType.Parse(dataTypeName), description, id);
        }
    }
}
