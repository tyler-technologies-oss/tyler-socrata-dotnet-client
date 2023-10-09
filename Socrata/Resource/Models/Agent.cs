using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Socrata
{
    using Socrata.SODA.Schema;
    
    [DataContract]
    public class Agent
    {
        [DataMember(Name = "resource")]
        public AgentResource Resource { get; }
    }

    [DataContract]
    public class AgentResource
    {
        [DataMember(Name = "name")]
        public string Name { get; }
        
        [DataMember(Name = "agent_uid")]
        public string AgentUid { get; }
        [DataMember(Name = "offline_reason")]
        public string OfflineReason { get; }
        [DataMember(Name = "went_online_at")]
        public string WentOnlineAt { get; }
        [DataMember(Name = "went_offline_at")]
        public string WentOfflineAt { get; }

    }
}
