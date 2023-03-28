using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    // {"preferred_upload_parallelism":4,"preferred_chunk_size":4194304}
    [DataContract]
    public class InitiateResponse
    {
        /// <summary>
        /// The amount of parallelism supported by the ByteSink
        ///</summary>
        [DataMember(Name="preferred_upload_parallelism")]
        public long PreferredParallelism { get; internal set; }

        /// <summary>
        /// The preferred chunk size of each
        ///</summary>
        [DataMember(Name="preferred_chunk_size")]
        public long PreferredChunkSize { get; internal set; }

    }
}
