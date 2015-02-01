using DDF.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage
{
    public interface IEventStore
    {
        void StoreEvents(Guid aggregateId, IEnumerable<IDomainEvent> events, long expectedInitialVersion);
        IEnumerable<IDomainEvent> LoadEvents(Guid aggregateId, long version = 0);
        T GetSnapshot<T>(Guid aggregateId) where T : AggregateBase;
        void SaveSnapshot(AggregateBase aggregate);
    }
}
