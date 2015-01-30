using DDF.Core.Domain;
using DDF.Core.Exceptions;
using DDF.Core.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DDF.Core.Storage.EntityFramework
{
    public class EventStore : IEventStore
    {

        private readonly IDbContextFactory<EventDbContext> _dbFactory;
        private readonly IMessageBus _bus;
        private readonly JsonSerializerSettings _jsonSettings;

        public EventStore(IMessageBus bus, IDbContextFactory<EventDbContext> dbFactory)
        {
            this._dbFactory = dbFactory;
            this._bus = bus;
            this._jsonSettings = DefaultJsonSerializerSettings();
        }

        public void StoreEvents(Guid aggregateId, IEnumerable<IDomainEvent> events, long expectedInitialVersion)
        {
            events = events.ToArray();
            var serializedEvents = events.Select(x =>
                new Tuple<string, string>(x.GetType().FullName, JsonConvert.SerializeObject(x, _jsonSettings)));

            using (var ctx = _dbFactory.Create())
            {
                long? existingSequence;
                var streamInfo = ctx.StreamInfos.Where(x => x.StreamId == aggregateId).FirstOrDefault();
                existingSequence = streamInfo == null ? (long?)null : (long)streamInfo.CurrentSequence;

                if (existingSequence.HasValue)
                    if (existingSequence > expectedInitialVersion)
                        throw new ConcurrencyException();



                using (var t = new TransactionScope())
                {

                    long? nextVersion = insertEventsAndReturnLastVersion(aggregateId, ctx, expectedInitialVersion, serializedEvents);
                    if (existingSequence == null)
                        startNewSequence(aggregateId, nextVersion.Value, ctx);
                    else
                        updateSequence(aggregateId, expectedInitialVersion, nextVersion.Value, ctx);

                    t.Complete();
                }

            }

            _bus.Events.Dispatch(events);
            
        }

        public IEnumerable<IDomainEvent> LoadEvents(Guid aggregateId, long version = 0)
        {
            using (var ctx = _dbFactory.Create())
            {
                var elist = ctx.Events
                    .Where(x => x.StreamId == aggregateId && x.Sequence >= version)
                    .OrderBy(x => x.TimeStamp)
                    .Select(x => new
                    {
                        EventType = x.EventType,
                        EventBody = x.EventBody
                    }).ToList();

                List<IDomainEvent> eventList = new List<IDomainEvent>();

                foreach (var e in elist)
                    yield return JsonConvert.DeserializeObject(
                        e.EventBody, _jsonSettings) as IDomainEvent;

            }
        }

        private JsonSerializerSettings DefaultJsonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            };
        }

        private void startNewSequence(
            Guid aggregateId, 
            long nextVersion, 
            EventDbContext ctx)
        {
            ctx.StreamInfos.Add(new StreamInfo()
            {
                StreamId = aggregateId,
                CurrentSequence = nextVersion
            });

            int changes = ctx.SaveChanges();

            if (changes != 1)
                throw new ConcurrencyException();

        }

        private void updateSequence(
            Guid aggregateId, 
            long expectedInitialVersion, 
            long nextVersion, 
            EventDbContext ctx)
        {
            var streamInfo = ctx.StreamInfos
                .Where(x => x.StreamId == aggregateId && x.CurrentSequence == expectedInitialVersion)
                .FirstOrDefault();
            if (streamInfo == null)
                throw new ConcurrencyException();
            streamInfo.CurrentSequence = nextVersion;
            ctx.SaveChanges();

        }

        private long insertEventsAndReturnLastVersion(
            Guid aggregateId, 
            EventDbContext ctx, 
            long nextVersion, 
            IEnumerable<Tuple<string, string>> serializedEvents)
        {
            foreach (var e in serializedEvents)
            {
                ctx.Events.Add(new DbEvent()
                {
                    EventId = Guid.NewGuid(),
                    StreamId = aggregateId,
                    Sequence = ++nextVersion,
                    TimeStamp = DateTimeOffset.Now,
                    EventType = e.Item1,
                    EventBody = e.Item2
                });
                ctx.SaveChanges();
            }

            return nextVersion;
        }


    }
}
