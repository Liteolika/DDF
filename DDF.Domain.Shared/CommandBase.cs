using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Domain.Shared
{
    public class CommandBase : ICommand
    {
        public Guid CommandId { get; set; }
        public DateTimeOffset CommandTimestamp { get; set; }
        public Guid AggregateId { get; set; }

        public CommandBase(Guid aggregateId)
        {
            this.CommandId = Guid.NewGuid();
            this.CommandTimestamp = DateTimeOffset.Now;
            this.AggregateId = aggregateId;
        }
    }
}
