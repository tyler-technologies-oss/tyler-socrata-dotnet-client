using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Socrata
{
    [DataContract]
    public class TestJson
    {
        [DataMember(Name = "incident_id")]
        public string IncidentId { get; internal set; }
    }
}
