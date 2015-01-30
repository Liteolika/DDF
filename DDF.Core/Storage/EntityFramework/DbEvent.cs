using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage.EntityFramework
{
    public class DbEvent
    {
        [Key]
        public Guid EventId { get; set; }
        public Guid StreamId { get; set; }
        public long Sequence { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string EventType { get; set; }
        public string EventBody { get; set; }
    }
}
