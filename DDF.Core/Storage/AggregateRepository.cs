using DDF.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public T GetById<T>(Guid aggregateId) where T : IAggregate
        {
            var events = _eventStore.LoadEvents(aggregateId);
            var aggregate = (T)Activator.CreateInstance(typeof(T), true);
            aggregate.LoadFromHistory(events);

            return aggregate;
        }

        public void Save(IAggregate aggregate)
        {
            var uncommittedEvents = aggregate.GetUncommittedChanges().ToList();
            var currentVersion = aggregate.AggregateVersion;
            var initialVersion = currentVersion - uncommittedEvents.Count;

            _eventStore.StoreEvents(aggregate.Id, uncommittedEvents, initialVersion);
            aggregate.ClearUncommittedChanges();
        }
    }
}
