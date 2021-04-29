using System.Runtime.Serialization;

namespace Socrata.DSMAPI
{
    // {"resource":{"seq_num":0,"end_byte_offset":68468,"byte_offset":0},"links":{}}
    [DataContract]
    public class ByteUploadResponse
    {
        /// <summary>
        /// The amount of parallelism supported by the ByteSink
        ///</summary>
        [DataMember(Name="resource")]
        public ByteResponse Resource { get; internal set; }
    }

    [DataContract]
    public class ByteResponse
    {
        /// <summary>
        /// The amount of parallelism supported by the ByteSink
        ///</summary>
        [DataMember(Name="seq_num")]
        public int SeqNum { get; internal set; }
        /// <summary>
        /// The amount of parallelism supported by the ByteSink
        ///</summary>
        [DataMember(Name="end_byte_offset")]
        public int EndByteOffset { get; internal set; }
    }

}
