using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage.EntityFramework
{
    public class StreamSnapshot
    {
        protected StreamSnapshot()
        { }

        public StreamSnapshot(Guid aggregateId, long version, string body)
        {
            this.SnapshotId = Guid.NewGuid();
            this.AggregateId = aggregateId;
            this.Version = version;
            this.Body = body;
        }

        [Key]
        public Guid SnapshotId { get; set; }
        public Guid AggregateId { get; set; }
        public long Version { get; set; }

        public string Body { get; set; }

    }
}
