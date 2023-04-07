namespace Socrata
{
    public class AudienceLevel
    {
        private AudienceLevel(string value) { Value = value; }

        public string Value { get; set; }

        public static AudienceLevel Parse(string typ)
        {
            switch (typ)
            {
                case "public":
                    return new AudienceLevel("public");
                case "private":
                    return new AudienceLevel("private");
                case "site":
                    return new AudienceLevel("site");
                default:
                    return new AudienceLevel("private");

            }
        }

        public static AudienceLevel PUBLIC   { get { return new AudienceLevel("public"); } }
        public static AudienceLevel PRIVATE   { get { return new AudienceLevel("private"); } }
        public static AudienceLevel INTERNAL    { get { return new AudienceLevel("site"); } }
    }
}
