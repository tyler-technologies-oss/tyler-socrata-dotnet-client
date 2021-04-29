using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class TransformParsedExpression
    {
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="value")]
        public string Value { get; set; }

        /// <summary>
        /// type
        ///</summary>
        [DataMember(Name="type")]
        public string Type { get; set; }

        /// <summary>
        /// qualifier
        ///</summary>
        [DataMember(Name="qualifier")]
        public string Qualifier { get; set; }

        /// <summary>
        /// position
        ///</summary>
        [DataMember(Name="position")]
        public ParsedExpressionPosition Position { get; set; }
    }
}
