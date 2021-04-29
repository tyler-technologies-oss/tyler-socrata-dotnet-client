using System.Collections.Generic;
using System.Linq;
using Socrata.HTTP;

namespace Socrata.SODA
{
    public class WorkingCopy
    {
        public WorkingCopy(SocrataHttpClient httpClient, ResourceMetadata metadata)
        {
            this.HttpClient = httpClient;
            this.Metadata = metadata;
            this.ColumnsToAdd = new List<Column>();
            this.ColumnsToRemove = new List<Column>();
            this.ColumnsToUpdate = new List<Column>();
        }

        public SocrataHttpClient HttpClient;
        public ResourceMetadata Metadata;

        internal  List<Column> ColumnsToAdd;
        internal  List<Column> ColumnsToRemove;
        internal  List<Column> ColumnsToUpdate;


        public ResourceMetadata Publish()
        {
            // TODO: errors?
            return this.HttpClient.PostEmpty<ResourceMetadata>("/views/" + this.Metadata.Id + "/publication.json");
        }

        public Column AddColumn(Column col)
        {
            this.ColumnsToAdd.Add(col);
            return col;
        }

        public Column RemoveColumn(Column col)
        {
            this.ColumnsToRemove.Add(col);
            return col;
        }

        public Column UpdateColumn(Column col)
        {
            this.ColumnsToUpdate.Add(col);
            return col;
        }

        private void ClearQueuedActions()
        {
            this.ColumnsToAdd = new List<Column>();
            this.ColumnsToRemove = new List<Column>();
            this.ColumnsToUpdate = new List<Column>();
        }

        private void ApplyColumnsToAdd(List<Column> columnsToAdd)
        {
            System.Console.WriteLine("Adding new columns");
            columnsToAdd.ForEach(c => {
                HttpClient.PostJson<ColumnMetadata>("/views/" + this.Metadata.Id  + "/columns.json", c.ToNewColumnDictionary());
            });
        }

        private void ApplyColumnsToUpdate(List<Column> columnsToUpdate)
        {
            System.Console.WriteLine("Updating columns");
            Dictionary<string, object> columns = new Dictionary<string, object>();
            columns.Add("columns", columnsToUpdate.Select(c => c.ToColumnDictionary()));
            HttpClient.PutJson<ResourceMetadata>("/views/" + this.Metadata.Id, columns);
        }

        private void ApplyColumnsToRemove(List<Column> columnsToRemove)
        {
            System.Console.WriteLine("Removing columns");
            columnsToRemove.ForEach(c => {
                HttpClient.Delete<string>("/views/" + this.Metadata.Id  + "/columns/" + c.columnId + ".json");
            });
        }

        public SchemaBuilder GetSchema()
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder();
            Metadata.Columns.ForEach((c) => {
                schemaBuilder.AddColumn(new Column(c));
            });
            return schemaBuilder;
        }

        // ???: Should I try and clean up and return `this` here or just return a new WorkingCopy?
        public WorkingCopy Apply()
        {
            this.ApplyColumnsToUpdate(ColumnsToUpdate);
            this.ApplyColumnsToRemove(ColumnsToRemove);
            this.ApplyColumnsToAdd(ColumnsToAdd);
            // Let's clear the queue
            this.ClearQueuedActions();
            // Let's get the our updated metadata
            this.RefreshMetadata();
            return this;
        }

                public bool Discard()
        {
            HttpClient.Delete<string>("/api/views/" + this.Metadata.Id + ".json");
            return true;
        }

        public WorkingCopy SetSchema(Schema schema)
        {
            // basically let's just diff these schemas and then send it over to Apply
            List<Column> currentSchema = Metadata.Columns.Select(c => c.ToColumn()).ToList();
            this.ClearQueuedActions();
            this.DiffSchemas(currentSchema, schema.Columns);
            return this.Apply();
        }

        private bool CheckSchemaForColumn(List<Column> schema, Column col)
        {
            return schema.Exists(c => c.columnId == col.columnId);
        }

        private void DiffSchemas(List<Column> oldSchema, List<Column> newSchema)
        {
            // TODO: Make sure this order makes sense? Update --> Add --> Remove?
            // Assume if there's a column id it's an existing column to update
            // easier just to send a PUT request with all existing columns 
            // rather than try and spend too much time calculating the diff to see what changed
            ColumnsToUpdate = newSchema.FindAll(c => c.columnId != null);
            // Assume if there's no column id set in the new schema, it's a new column
            ColumnsToAdd = newSchema.FindAll(c => c.columnId == null);
            // Check to see which of the old columns are not present in the new schema. 
            // These are the columns we'll want to remove
            ColumnsToRemove = oldSchema.FindAll(c => !CheckSchemaForColumn(newSchema, c));
        }

        private void RefreshMetadata()
        {
            ResourceMetadata updatedMetadata = HttpClient.GetJson<ResourceMetadata>("/api/views/" + this.Metadata.Id + ".json");
            this.Metadata = updatedMetadata;
        }
    }
}
