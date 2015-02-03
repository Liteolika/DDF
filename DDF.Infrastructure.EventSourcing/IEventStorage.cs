using DDF.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing
{
    public interface IEventStore
    {

        void SaveEvent(Guid aggregateId, IEvent @event, int aggregateVersion);
        IEnumerable<IEvent> GetEvents(Guid aggregateId);
    }
}
