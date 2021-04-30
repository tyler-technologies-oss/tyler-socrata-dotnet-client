namespace Socrata.DSMAPI
{
    public class Transforms 
    {
        private Transforms(string value) { Value = value; }

        public string Value { get; internal set; }

        public static Transforms CUSTOM(string transform) 
        { return new Transforms(transform); }
        public static Transforms TEXT(string column) 
        { return new Transforms("to_text(`" + column + "`)"); }
        public static Transforms URL(string column) 
        { return new Transforms("to_url(`" + column + "`)"); }
        public static Transforms NUMBER(string column)   
        { return new Transforms("to_number(`" + column + "`)"); }
        public static Transforms FLOATING_TIMESTAMP(string column, string dateformat = null)    
        { return new Transforms("to_floating_timestamp(`" + column + "`)"); }
        public static Transforms BOOLEAN(string column) 
        { return new Transforms("to_checkbox(`" + column + "`)"); }
        public static Transforms POINT(string column)   
        { return new Transforms("point(`" + column + "`)"); }
        public static Transforms MULTIPOINT(string column)   
        { return new Transforms("multipoint(`" + column + "`)"); }
        public static Transforms POLYGON(string column)   
        { return new Transforms("point(`" + column + "`)"); }
        public static Transforms MULTIPOLYGON(string column)   
        { return new Transforms("multipoint(`" + column + "`)"); }
        public static Transforms LINESTRING(string column)   
        { return new Transforms("point(`" + column + "`)"); }
        public static Transforms MULTILINESTRING(string column)   
        { return new Transforms("multipoint(`" + column + "`)"); }
    }
}
