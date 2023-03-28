using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Socrata.DSMAPI
{
    [DataContract]
    public class TransformInputColumn
    {
        /// <summary>
        /// input_column_id
        ///</summary>
        [DataMember(Name="input_column_id")]
        public long InputColumnId  { get; set; }
    }
}
