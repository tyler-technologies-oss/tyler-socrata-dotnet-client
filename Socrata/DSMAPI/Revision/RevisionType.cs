namespace Socrata.DSMAPI
{
    public class RevisionType
    {
        private RevisionType(string value) { Value = value; }

        public string Value { get; set; }

        public static RevisionType Parse(string typ)
        {
            switch (typ)
            {
                case "replace":
                    return new RevisionType("replace");
                case "update":
                    return new RevisionType("update");
                case "delete":
                    return new RevisionType("delete");
                default:
                    return new RevisionType("replace");

            }
        }

        public static RevisionType REPLACE { get { return new RevisionType("replace"); } }
        public static RevisionType UPDATE { get { return new RevisionType("update"); } }
        public static RevisionType DELETE { get { return new RevisionType("delete"); } }
    }
}
