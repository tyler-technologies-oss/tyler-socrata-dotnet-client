using System.Runtime.Serialization;

namespace Socrata.ActivityLog
{
    [DataContract]
    public class ActivityLogModel
    {
        /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="id")]
        public string Id { get; internal set; }

                /// <summary>
        /// id
        ///</summary>
        [DataMember(Name="created_at")]
        public string CreatedAt { get; internal set; }

        /// <summary>
        /// activity_type
        ///</summary>
        [DataMember(Name="activity_type")]
        public string ActivityType { get; internal set; }

        /// <summary>
        /// acting_user_id
        ///</summary>
        [DataMember(Name="acting_user_id")]
        public string ActingUserId { get; internal set; }

        /// <summary>
        /// acting_user_name
        ///</summary>
        [DataMember(Name="acting_user_name")]
        public string ActingUserName { get; internal set; }

        /// <summary>
        /// service
        ///</summary>
        [DataMember(Name="service")]
        public string Service { get; internal set; }

        /// <summary>
        /// dataset_uid
        ///</summary>
        [DataMember(Name="dataset_uid")]
        public string DatasetUid { get; internal set; }

        /// <summary>
        /// dataset_name
        ///</summary>
        [DataMember(Name="dataset_name")]
        public string DatasetName { get; internal set; }

        /// <summary>
        /// asset_type
        ///</summary>
        [DataMember(Name="asset_type")]
        public string AssetType { get; internal set; }

        /// <summary>
        /// details
        ///</summary>
        [DataMember(Name="details")]
        public string Details { get; internal set; }

        /// <summary>
        /// affected_item
        ///</summary>
        [DataMember(Name="affected_item")]
        public string AffectedItem { get; internal set; }
    }
}
