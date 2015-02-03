using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Domain.Shared
{
    public abstract class AggregateBase : IAggregate
    {
        private List<EventBase> _uncommittedAggregateEvents = new List<EventBase>();

        public Guid AggregateId { get; set; }

        public int AggregateVersion { get; set; }

        protected AggregateBase()
        {
            this.AggregateVersion = 0;
        }

        public IEnumerable<EventBase> UncommittedAggregateEvents
        {
            get { return _uncommittedAggregateEvents; }
        }

        public void ClearUncommittedAggregateEvents()
        {
            _uncommittedAggregateEvents.Clear();
        }

        public void ApplyEvent(EventBase @event)
        {
            _uncommittedAggregateEvents.Add(@event);
        }

        public abstract bool IsValid();

       
    }
}
