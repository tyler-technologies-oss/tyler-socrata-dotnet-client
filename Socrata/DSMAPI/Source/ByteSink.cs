using System;
using System.IO;

namespace Socrata.DSMAPI {
    using Socrata.HTTP;
    public class ByteSink {
        private SocrataHttpClient httpClient;
        private long preferred_chunk_size;
        private long seq_num = 0;
        private long byte_offset = 0;
        private string chunkUri;
        public ByteSink(SocrataHttpClient httpClient, string chunkUri, InitiateResponse chunkInfo)
        {
            this.httpClient = httpClient;
            this.chunkUri = chunkUri;
            this.preferred_chunk_size = chunkInfo.PreferredChunkSize;
        }

        public void FileSink(string filepath)
        {
            ByteUploadResponse resp = null;
            using (FileStream fs = File.OpenRead(filepath))
            {
                byte[] b = new byte[this.preferred_chunk_size];
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    resp = WriteBytes(b);
                    this.seq_num += 1;
                    this.byte_offset += b.Length;
                }
            }

            Commit(resp);
        }

        public ByteUploadResponse WriteBytes(byte[] b)
        {
            string nextUri = this.chunkUri.Replace("{seq_num}", this.seq_num.ToString()).Replace("{byte_offset}", this.byte_offset.ToString());
            Console.WriteLine(nextUri);
            return httpClient.PostBytes<ByteUploadResponse>(nextUri, b);
        }

        public void Commit(ByteUploadResponse resp)
        {
            string commitUri = this.chunkUri.Replace("{seq_num}", resp.Resource.SeqNum.ToString()).Replace("{byte_offset}", resp.Resource.EndByteOffset.ToString());
            Console.WriteLine(commitUri);
            httpClient.PostEmpty<ByteUploadResponse>(commitUri + "/commit");
        }
    }
}
