using System.Collections.Generic;

namespace Socrata.SODA.Schema
{
    public class Column
    {
        public string columnName {get; set; }
        public string apiFieldName { get; set; }
        public string description {get; set; }
        public string columnId {get; set; }
        
        public ColumnMetadata Metadata {get; }
        public SocrataDataType type {get; set; }

        public Column(string name, string fieldName, SocrataDataType type, string description, string id)
        {
            this.columnName = name;
            this.apiFieldName = fieldName;
            this.type = type;
            this.description = description;
            this.columnId = id;
            this.Metadata = null;
        }
        public Column(string name, SocrataDataType type, string description)
        {
            this.columnName = name;
            this.type = type;
            this.description = description;
            this.columnId = null;
            this.Metadata = null;
        }

        public Column(string name, SocrataDataType type)
        {
            // TODO: names must be fieldName compliant
            this.columnName = name;
            this.type = type;
            this.description = null;
            this.columnId = null;
            this.Metadata = null;
        }

        public Column(ColumnMetadata metadata)
        {
            this.columnName = metadata.name;
            this.apiFieldName = metadata.fieldName;
            this.type = SocrataDataType.Parse(metadata.dataTypeName);
            this.description = metadata.description;
            this.columnId = metadata.id;
            this.Metadata = metadata;
        }
         public Column UpdateName(string name)
         {
             this.columnName = name;
             return this;
         }

         public Column UpdateType(SocrataDataType typ)
         {
             this.type = typ;
             return this;
         }

        public Dictionary<string, object> ToColumnDictionary()
        {
            return new Dictionary<string, object>
            {
                { "id", this.columnId },
                { "name", this.columnName },
                { "fieldName", this.columnName.ToLower() },
                { "dataTypeName", this.type.Value },
                { "description", this.description },
            };
        }

        public Dictionary<string, object> ToNewColumnDictionary()
        {
            return new Dictionary<string, object>
            {
                { "name", this.columnName },
                { "fieldName", this.columnName.ToLower() },
                { "dataTypeName", this.type.Value },
                { "description", this.description },
            };
        }
    
    }
}
