using DDF.Domain.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing.Imp
{
    public class InMemoryEventStorage : IEventStore
    {
        private List<EventStreamItem> _events;
        private JsonSerializerSettings _jsonSettings;

        public InMemoryEventStorage()
        {
            this._events = new List<EventStreamItem>();
            this._jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
            };
        }

        public void SaveEvent(Guid aggregateId, IEvent @event, int aggregateVersion)
        {
            var eventType = @event.GetType().FullName;
            var eventBody = JsonConvert.SerializeObject(@event, @event.GetType(), _jsonSettings);

            _events.Add(new EventStreamItem
            {
                EventId = @event.EventId,
                AggregateId = @event.AggregateId,
                AggregateVersion = aggregateVersion,
                EventTimestamp = @event.EventTimestamp,
                EventType = eventType,
                EventBody = eventBody
            });

            var a = 1;
        }

        public IEnumerable<IEvent> GetEvents(Guid aggregateId)
        {
            var storedEvents = _events.Where(x => x.AggregateId == aggregateId)
                .OrderBy(x => x.AggregateVersion);

            List<IEvent> events = new List<IEvent>();
            foreach (var storedEvent in storedEvents)
            {
                var e = JsonConvert.DeserializeObject(storedEvent.EventBody, _jsonSettings);


                events.Add(e as IEvent);
            }
            return events;
        }

        private class EventStreamItem
        {
            public Guid EventId {get;set;}
            public DateTimeOffset EventTimestamp { get; set; }
            public Guid AggregateId { get; set; }
            public int AggregateVersion { get; set; }
            public string EventBody { get; set; }
            public string EventType { get; set; }

        }

    }
}
