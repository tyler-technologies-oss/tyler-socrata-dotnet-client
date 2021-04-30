using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Socrata
{
    [DataContract]
    public class DomainResults
    {
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="results")]
        public List<DomainResult> Results { get; internal set; }
    }
}
