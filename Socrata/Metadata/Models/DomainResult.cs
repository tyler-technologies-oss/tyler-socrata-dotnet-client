using System.Runtime.Serialization;

namespace Socrata
{
    [DataContract]
    public class DomainResult
    {
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="resource")]
        public DomainResource Resource { get; internal set; }
    }
}
