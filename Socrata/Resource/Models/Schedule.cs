using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Socrata
{
    using Socrata.SODA.Schema;
    
    [DataContract]
    public class Schedule
    {
        [DataMember(Name = "resource")]
        public ScheduleResource Resource { get; set; }
    }

    [DataContract]
    public class ScheduleResource
    {
        /// <summary>
        /// The ID of the schedule.
        /// </summary>
        [DataMember(Name = "id")]
        public int id { get; }

        /// <summary>
        /// Current run state of the schedule.
        /// </summary>
        [DataMember(Name = "runstate")]
        public ScheduleRunState RunState { get; }
        
        /// <summary>
        /// Current row quota (within 24 hours).
        /// </summary>
        [DataMember(Name = "quota")]
        public string Quota { get; }
        /// <summary>
        /// Is schedule paused due to failure?
        /// </summary>
        [DataMember(Name = "paused_due_to_failure")]
        public bool PausedDueToFailure { get; }
        /// <summary>
        /// Is schedule paused?
        /// </summary>
        [DataMember(Name = "paused")]
        public bool Paused { get; set; }
        /// <summary>
        /// The last run time in UTC.
        /// </summary>
        [DataMember(Name = "last_run")]
        public string LastRun { get; }
        /// <summary>
        /// Has the schedule hit its quota?
        /// </summary>
        [DataMember(Name = "has_hit_quota")]
        public string has_hit_quota { get; }
        /// <summary>
        /// Asset scheduled.
        /// </summary>
        [DataMember(Name = "fourfour")]
        public string FourFour { get; }
        /// <summary>
        /// Dataset name of scheduled asset.
        /// </summary>
        [DataMember(Name = "dataset_name")]
        public string DatasetName { get; }
        /// <summary>
        /// Number of consecutive failures. Schedules pause after
        /// 35 consecutive failures.
        /// </summary>
        [DataMember(Name = "consecutive_failures")]
        public int ConsecutiveFailures { get; }
        /// <summary>
        /// The cadence of the schedule.
        /// </summary>
        [DataMember(Name = "cadence")]
        public ScheduleCadence Cadence { get; set; }
        /// <summary>
        /// The action the schedule will take.
        /// </summary>
        [DataMember(Name = "action")]
        public ScheduleAction Action { get; set; }
    }

    [DataContract]
    public class ScheduleRunState
    {
        /// <summary>
        /// Current run state of the schedule:
        /// - running
        /// - paused
        /// - scheduled
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; }
    }

    [DataContract]
    public class ScheduleCadence
    {
        /// <summary>
        /// The timezone of the schedule run, e.g. UTC, America/New_York.
        /// </summary>
        [DataMember(Name = "timezone")]
        public string Timezone { get; set; }
        /// <summary>
        /// The cron expression of the schedule.
        /// </summary>
        [DataMember(Name = "cron")]
        public string Cron { get; set; }
        /// <summary>
        /// Should the cron run within its timezone?
        /// </summary>
        [DataMember(Name = "cron_in_timezone")]
        public bool CronInTimezone { get; }
    }

    [DataContract]
    public class ScheduleAction
    {
        /// <summary>
        /// Schedule type:
        /// - connection_agent
        /// - import_from_url
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
        /// <summary>
        /// Schedule parameters.
        /// </summary>
        [DataMember(Name = "parameters")]
        public ScheduleParameters Parameters { get; set; }
    }

    [DataContract]
    public class ScheduleParameters
    {
        /// <summary>
        /// The plugin path to read from to determine the source.
        /// </summary>
        [DataMember(Name = "path")]
        public List<string> Path { get; set; }
        /// <summary>
        /// Plugin parameters (if any)
        /// </summary>
        [DataMember(Name = "parameters")]
        public Dictionary<string, string> Parameters { get; set; }
        /// <summary>
        /// Plugin namespace e.g.:
        /// {"type": "csv", "name": "MY CSV PLUGIN"}
        /// </summary>
        [DataMember(Name = "namespace")]
        public ScheduleAgentNamespace Namespace { get; set; }
        /// <summary>
        /// The agent UID to tie the schedule to.
        /// </summary>
        [DataMember(Name = "agent_uid")]
        public string AgentUid { get; set; }
    }

    [DataContract]
    public class ScheduleAgentNamespace
    {
        /// <summary>
        /// Plugin type, e.g. csv, mssql, oracle, etc.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
        /// <summary>
        /// The name of the installed plugin.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
