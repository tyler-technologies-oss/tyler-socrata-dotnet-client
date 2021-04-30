namespace Socrata.SODA.Schema
{
    public class SocrataDataType
    {
        private SocrataDataType(string value) { Value = value; }

        public string Value { get; set; }

        public static SocrataDataType Parse(string typ)
        {
            switch (typ)
            {
                case "number":
                    return new SocrataDataType("number");
                case "calendar":
                    return new SocrataDataType("calendar_date");
                case "checkbox":
                    return new SocrataDataType("checkbox");
                case "point":
                    return new SocrataDataType("point");
                case "multipoint":
                    return new SocrataDataType("multipoint");
                case "calendar_date":
                    return new SocrataDataType("calendar_date");
                default:
                    return new SocrataDataType("text");

            }
        }

        public static SocrataDataType TEXT   { get { return new SocrataDataType("text"); } }
        public static SocrataDataType NUMBER   { get { return new SocrataDataType("number"); } }
        public static SocrataDataType DATETIME    { get { return new SocrataDataType("calendar_date"); } }
        public static SocrataDataType BOOLEAN { get { return new SocrataDataType("checkbox"); } }
        public static SocrataDataType POINT   { get { return new SocrataDataType("point"); } }
        public static SocrataDataType MULTIPOINT   { get { return new SocrataDataType("multipoint"); } }
    }
}
