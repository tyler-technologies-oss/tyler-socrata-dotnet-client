using System.Runtime.Serialization;

namespace Socrata
{
    [DataContract]
    public class DomainResource
    {
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="name")]
        public string Name { get; internal set; }
        
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="id")]
        public string Id { get; internal set; }
        
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="description")]
        public string Description { get; internal set; }
        
        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="type")]
        public string Type { get; internal set; }

        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="updatedAt")]
        public string UpdatedAt { get; internal set; }

        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="createdAt")]
        public string CreatedAt { get; internal set; }

        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="permalink")]
        public string Permalink { get; internal set; }

        /// <summary>
        /// value
        ///</summary>
        [DataMember(Name="link")]
        public string Link { get; internal set; }
    }
}
