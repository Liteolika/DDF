using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage.EntityFramework
{
    public class StreamEntry
    {
        [Key]
        public Guid StreamId { get; set; }
        public long CurrentSequence { get; set; }
    }
}
