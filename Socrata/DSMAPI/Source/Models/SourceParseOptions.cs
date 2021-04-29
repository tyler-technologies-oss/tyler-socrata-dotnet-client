using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class SourceParseOptions
    {
        /// <summary>
        /// trim_whitespace
        ///</summary>
        [DataMember(Name="trim_whitespace")]
        public bool TrimWhitespace { get; set; }

        /// <summary>
        /// remove_empty_rows
        ///</summary>
        [DataMember(Name="remove_empty_rows")]
        public bool RemoveEmptyRows { get; set; }

        /// <summary>
        /// quote_char
        ///</summary>
        [DataMember(Name="quote_char")]
        public string QuoteChar { get; set; }

        /// <summary>
        /// parse_source
        ///</summary>
        [DataMember(Name="parse_source")]
        public bool ParseSource { get; set; }

        /// <summary>
        /// header_count
        ///</summary>
        [DataMember(Name="header_count")]
        public int HeaderCount { get; set; }

        /// <summary>
        /// encoding
        ///</summary>
        [DataMember(Name="encoding")]
        public string Encoding { get; set; }

        // <summary>
        // column_separator
        // </summary>
        [DataMember(Name="column_separator")]
        public string ColumnSeparator { get; set; }

        // <summary>
        /// column_header
        ///</summary>
        [DataMember(Name="column_header")]
        public int ColumnHeader { get; set; }

    }
}
