namespace Socrata.DSMAPI
{
    public class ContentType
    {
        private ContentType(string value) { Value = value; }

        public string Value { get; set; }

        public static ContentType Parse(string typ)
        {
            switch (typ)
            {
                case "text/csv":
                    return new ContentType("text/csv");
                case "application/json":
                    return new ContentType("application/json");
                case "text/tsv":
                    return new ContentType("view");
                case "application/octet-stream":
                    return new ContentType("application/octet-stream");
                default:
                    return new ContentType("application/octet-stream");
            }
        }

        public static ContentType CSV { get { return new ContentType("text/csv"); } }
        public static ContentType JSON { get { return new ContentType("application/json"); } }
        public static ContentType TSV { get { return new ContentType("url"); } }
        public static ContentType BLOB { get { return new ContentType("application/octet-stream"); } }
    }
}
