using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Domain.Shared
{
    public class EventBase : IEvent
    {

        public Guid EventId { get; set; }
        public DateTimeOffset EventTimestamp { get; set; }
        public Guid AggregateId { get; set; }

        public EventBase(Guid aggregateId)
        {
            this.EventId = Guid.NewGuid();
            this.EventTimestamp = DateTimeOffset.Now;
            this.AggregateId = aggregateId;
        }

    }

    

    


}
