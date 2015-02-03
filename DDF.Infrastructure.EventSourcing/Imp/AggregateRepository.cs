using DDF.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing.Imp
{
    public class AggregateRepository<T> : IAggregateRepository<T>
        where T : AggregateBase
    {

        private IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            this._eventStore = eventStore;
        }

        public void SaveAggregate(T aggregate)
        {

            var events = aggregate.UncommittedAggregateEvents.ToList();

            foreach (var @event in events)
            {
                ApplyAggregateEvent(@event, aggregate);
            }

            if (aggregate.IsValid())
            {
                foreach (var @event in events)
                {
                    _eventStore.SaveEvent(aggregate.AggregateId, @event, aggregate.AggregateVersion);
                }
                aggregate.ClearUncommittedAggregateEvents();
            }

        }


        private void ApplyAggregateEvent(IEvent @event, T aggregate)
        {
            var methods = typeof(T).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "Handle");
            var method = methods.Single(x => x.GetParameters().First().ParameterType.FullName == @event.GetType().FullName);
            method.Invoke(aggregate, new object[] { @event });
            aggregate.AggregateVersion++;
        }

        public T GetAggregate(Guid AggregateId)
        {
            IEnumerable<IEvent> events = _eventStore.GetEvents(AggregateId);

            var aggregate = (T)Activator.CreateInstance(typeof(T));
            foreach (var @event in events)
                ApplyAggregateEvent(@event, aggregate);

            return aggregate;
        }
    }
}
