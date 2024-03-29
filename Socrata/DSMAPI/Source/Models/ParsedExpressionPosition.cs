
using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class ParsedExpressionPosition
    {
        /// <summary>
        /// line
        ///</summary>
        [DataMember(Name="line")]
        public long line { get; set; }

        /// <summary>
        /// column
        ///</summary>
        [DataMember(Name="column")]
        public long Column { get; set; }
    }
}
