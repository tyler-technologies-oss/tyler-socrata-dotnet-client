namespace Socrata.DSMAPI
{
    public class SourceType
    {
        private SourceType(string value) { Value = value; }

        public string Value { get; set; }

        public static SourceType Parse(string typ)
        {
            switch (typ)
            {
                case "upload":
                    return new SourceType("upload");
                case "url":
                    return new SourceType("url");
                case "view":
                    return new SourceType("view");
                case "gateway":
                    return new SourceType("gateway");
                default:
                    return new SourceType("view");
            }
        }

        public static SourceType VIEW { get { return new SourceType("view"); } }
        public static SourceType UPLOAD { get { return new SourceType("upload"); } }
        public static SourceType URL { get { return new SourceType("url"); } }
        public static SourceType GATEWAY { get { return new SourceType("gateway"); } }
    }
}
