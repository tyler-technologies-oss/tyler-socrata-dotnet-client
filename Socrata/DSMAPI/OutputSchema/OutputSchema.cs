using Socrata.HTTP;
using System;

namespace Socrata.DSMAPI
{
    public class OutputSchema
    {
        SocrataHttpClient httpClient;
        public OutputSchemaModel osModel { get; internal set; }
        string transformUri;
        string showUri;
        public OutputSchema(SocrataHttpClient httpClient, OutputSchemaModel osModel, string transformUri, string showUri)
        {
            this.osModel = osModel;
            this.httpClient = httpClient;
            this.transformUri = transformUri;
            this.showUri = showUri;
        }

        public OutputSchema ChangeTransform(string columnName, Transforms transform)
        {
            osModel.OutputColumns.ForEach((column) => {
                if(column.DisplayName == columnName)
                {
                    int i = osModel.OutputColumns.IndexOf(column);
                    osModel.OutputColumns[i].Transform.TransformExpr = transform.Value;
                }
            });
            return this;
        }

        public OutputSchema ChangeColumnName(string columnName, string newName)
        {
            osModel.OutputColumns.ForEach((column) => {
                if(column.DisplayName == columnName)
                {
                    int i = osModel.OutputColumns.IndexOf(column);
                    osModel.OutputColumns[i].DisplayName = newName;
                }
            });
            return this;
        }

        public OutputSchema ChangeColumnDescription(string columnName, string newDescription)
        {
            osModel.OutputColumns.ForEach((column) => {
                if(column.DisplayName == columnName)
                {
                    int i = osModel.OutputColumns.IndexOf(column);
                    osModel.OutputColumns[i].Description = newDescription;
                }
            });
            return this;
        }

        public void Submit()
        {
            OutputSchemaModel newModel = this.httpClient.PostJson<OutputSchemaModel>(this.transformUri, this.osModel);
        }

        public bool ValidateRowId(string columnName)
        {
            bool valid = false;
            OutputSchemaResult osResult = this.httpClient.GetJson<OutputSchemaResult>(this.showUri);
            string ValidateUri = osResult.Links.OutputSchemaLinks.ValidateRowIdUri;
            osModel.OutputColumns.ForEach((column) => {
                if(column.DisplayName == columnName)
                {
                    int i = osModel.OutputColumns.IndexOf(column);
                    int colId = osModel.OutputColumns[i].Transform.Id;
                    System.Console.WriteLine(colId);
                    System.Console.WriteLine(ValidateUri);
                    ValidResult res = this.httpClient.GetJson<ValidResult>(ValidateUri.Replace("{transform_id}", colId.ToString()));
                    valid = res.Valid;
                }
            });
            return valid;
        }

        public OutputSchema SetRowId(string columnName)
        {
            osModel.OutputColumns.ForEach((column) => {
                if(column.DisplayName == columnName)
                {
                    int i = osModel.OutputColumns.IndexOf(column);
                    osModel.OutputColumns[i].IsPrimaryKey = true;
                }
            });
            return this;
        }
    }


}
