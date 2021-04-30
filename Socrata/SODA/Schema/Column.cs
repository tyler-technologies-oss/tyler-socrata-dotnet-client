using System.Collections.Generic;

namespace Socrata.SODA.Schema
{
    public class Column
    {
        public string columnName {get; set; }
        public string description {get; set; }
        public string columnId {get; set; }
        
        public ColumnMetadata Metadata {get; }
        public SocrataDataType type {get; set; }

        public Column(string name, SocrataDataType type, string description, string id)
        {
            // TODO: names must be fieldName compliant
            this.columnName = name;
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
            // TODO: names must be fieldName compliant
            this.columnName = metadata.name;
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
            Dictionary<string, object> col = new Dictionary<string, object>();
            col.Add("id", this.columnId);
            col.Add("name", this.columnName);
            col.Add("fieldName", this.columnName.ToLower());
            col.Add("dataTypeName", this.type.Value);
            col.Add("description", this.description);
            return col;
        }

        public Dictionary<string, object> ToNewColumnDictionary()
        {
            Dictionary<string, object> col = new Dictionary<string, object>();
            col.Add("name", this.columnName);
            col.Add("fieldName", this.columnName.ToLower());
            col.Add("dataTypeName", this.type.Value);
            col.Add("description", this.description);
            return col;
        }
    
    }
}
