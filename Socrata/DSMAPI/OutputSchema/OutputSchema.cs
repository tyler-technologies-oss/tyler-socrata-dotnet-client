using Socrata.HTTP;

namespace Socrata.DSMAPI
{
    public class OutputSchema
    {
        SocrataHttpClient httpClient;
        public OutputSchemaModel osModel { get; internal set; }
        string sourceLink;
        public OutputSchema(SocrataHttpClient httpClient, OutputSchemaModel osModel, string sourceLink)
        {
            this.osModel = osModel;
            this.httpClient = httpClient;
            this.sourceLink = sourceLink;
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
            this.httpClient.PostJson<OutputSchemaModel>(this.sourceLink, this.osModel);
        }
    }


}
