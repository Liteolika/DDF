using DDF.Infrastructure.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Impl
{
    public class EventPublisher : IEventPublisher
    {
        private IEventHandlerFactory _eventHandlerFactory;

        public EventPublisher(IEventHandlerFactory eventHandlerFactory)
        {
            this._eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish<TEvent>(TEvent @event)
        {
            var handlers = _eventHandlerFactory.GetHandlersForEvent(@event);
            foreach (var handler in handlers)
                handler.Handle(@event);
        }
    }
}
