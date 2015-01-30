using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Domain
{
    public abstract class AggregateBase : IAggregate
    {

        public Guid Id { get; protected set; }
        public long AggregateVersion { get; private set; }
        private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();

        protected AggregateBase(Guid id)
        {
            this.Id = id;
        }

        public AggregateBase() { }

        public IEnumerable<IDomainEvent> GetUncommittedChanges()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedChanges()
        {
            _uncommittedEvents.Clear();
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                UpdateAggregate(@event);
                AggregateVersion++;
            }
        }

        protected void Apply(IDomainEvent @event)
        {
            _uncommittedEvents.Add(@event);
            UpdateAggregate(@event);
            AggregateVersion++;
        }

        public abstract bool Validate();

        private void UpdateAggregate(IDomainEvent @event)
        {
            var aggregateType = this.GetType();
            var eventType = @event.GetType();

            const string methodName = "UpdateFromEvent";

            var method = aggregateType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.Name == methodName)
                    .Where(x => x.GetParameters()
                        .Single()
                        .ParameterType
                        .IsAssignableFrom(eventType))
                        .SingleOrDefault();

            if (method == null)
                throw new MissingMethodException("The type " + aggregateType.FullName + " does not implement the method " + methodName + " with parameter type " + eventType.FullName);

            method.Invoke(this, new[] { @event });

        }

        

    }

}
