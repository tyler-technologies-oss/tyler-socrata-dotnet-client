using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{

    [DataContract]
    public class SourceTypeJson
    {
        /// <summary>
        /// type: view, url, or upload
        ///</summary>
        [DataMember(Name="type")]
        public string Type { get; set; }

        /// <summary>
        /// loaded
        ///</summary>
        [DataMember(Name="loaded")]
        public bool Loaded { get; set; }

        /// <summary>
        /// fourfour
        ///</summary>
        [DataMember(Name="fourfour")]
        public string FourFour { get; set; }
    }
}
