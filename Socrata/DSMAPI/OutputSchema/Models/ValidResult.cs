using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class ValidResult
    {
        /// <summary>
        /// Resource
        ///</summary>
        [DataMember(Name="valid")]
        public bool Valid { get; set; }
    }
}
