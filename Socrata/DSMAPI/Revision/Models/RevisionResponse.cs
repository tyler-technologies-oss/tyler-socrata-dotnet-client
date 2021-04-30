using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class RevisionResponse
    {
        /// <summary>
        /// Resource
        ///</summary>
        [DataMember(Name="resource")]
        public RevisionResource Resource { get; set; }

        /// <summary>
        /// Links
        ///</summary>
        [DataMember(Name="links")]
        public RevisionLinks Links { get; set; }

    }

    [DataContract]
    public class RevisionResource
    {
        /// <summary>
        /// Updated At
        /// </summary>
        [DataMember(Name = "updated_at")]
        public string UpdatedAt { get; internal set; }

        /// <summary>
        /// Revision Sequence
        /// </summary>
        [DataMember(Name = "revision_seq")]
        public int RevisionSeq { get; internal set; }

        /// <summary>
        /// metadata
        /// </summary>
        [DataMember(Name = "metadata")]
        public RevisionMetadata Metadata { get; internal set; }

        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; internal set; }

        /// <summary>
        /// href
        /// </summary>
        [DataMember(Name = "href")]
        public List<string> Href { get; }

        /// <summary>
        /// fourfour
        /// </summary>
        [DataMember(Name = "fourfour")]
        public string FourFour { get; internal set; }

        /// <summary>
        /// domain_id
        /// </summary>
        [DataMember(Name = "domain_id")]
        public int DomainId { get; }

        /// <summary>
        /// creation_source
        /// </summary>
        [DataMember(Name = "creation_source")]
        public string CreationSource { get; }

        /// <summary>
        /// created_by: user object for the person who created the revision
        /// </summary>
        [DataMember(Name = "created_by")]
        public Dictionary<string, string> CreatedBy { get; internal set; }

        /// <summary>
        /// closed_at
        /// </summary>
        [DataMember(Name = "closed_at")]
        public string ClosedAt { get; internal set; }

        /// <summary>
        /// blob_id
        /// </summary>
        [DataMember(Name = "blob_id")]
        public string BlobId { get; internal set; }

        /// <summary>
        /// attachments
        /// </summary>
        [DataMember(Name = "attachments")]
        public List<string> Attachments { get; internal set; }

        /// <summary>
        /// actions
        /// </summary>
        [DataMember(Name = "actions")]
        public Dictionary<string, string> Actions { get; }
        
        /// <summary>
        /// task_sets
        /// </summary>
        [DataMember(Name = "task_sets")]
        public List<TaskSet> TaskSets { get; set; }
    }

    [DataContract]
    public class TaskSet
    {
        /// <summary>
        /// status
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; } 
    }

    [DataContract]
    public class RevisionMetadata
    {
        /// <summary>
        /// tags
        /// </summary>
        [DataMember(Name = "tags")]
        public object Tags { get; }

        /// <summary>
        /// private metdata
        /// </summary>
        [DataMember(Name = "privateMetadata")]
        public Dictionary<string, string> PrivateMetadata { get; }

        /// <summary>
        /// name
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; internal set; }

        /// <summary>
        /// metadata
        /// </summary>
        [DataMember(Name = "metadata")]
        public object Metadata { get; }

        /// <summary>
        /// licenseId
        /// </summary>
        [DataMember(Name = "licenseId")]
        public string LicenseId { get; }

        /// <summary>
        /// license
        /// </summary>
        [DataMember(Name = "license")]
        public object License { get; }

        /// <summary>
        /// description
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; internal set; }

        /// <summary>
        /// category
        /// </summary>
        [DataMember(Name = "category")]
        public string Category { get; }

        /// <summary>
        /// attributionLink
        /// </summary>
        [DataMember(Name = "attributionLink")]
        public string AttributionLink { get; }

        /// <summary>
        /// attribution
        /// </summary>
        [DataMember(Name = "attribution")]
        public string Attribution { get; }

    }

    [DataContract]
    public class RevisionLinks
    {
        /// <summary>
        /// update
        /// </summary>
        [DataMember(Name = "update")]
        public string Update { get; internal set; }

        /// <summary>
        /// show
        /// </summary>
        [DataMember(Name = "show")]
        public string Show { get; internal set; }

        /// <summary>
        /// plan
        /// </summary>
        [DataMember(Name = "plan")]
        public string Plan { get; }

        /// <summary>
        /// list_sources
        /// </summary>
        [DataMember(Name = "list_sources")]
        public string ListSources { get; }

        /// <summary>
        /// discard
        /// </summary>
        [DataMember(Name = "discard")]
        public string Discard { get; internal set; }

        /// <summary>
        /// create_source
        /// </summary>
        [DataMember(Name = "create_source")]
        public string CreateSource { get; internal set; }

        /// <summary>
        /// apply
        /// </summary>
        [DataMember(Name = "apply")]
        public string Apply { get; internal set; }
    }
}
