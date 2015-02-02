using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDF.Infrastructure.Messaging.Commands;
using DDF.Infrastructure.Messaging.Events;

namespace DDF.Infrastructure.Messaging.Impl
{

    public class InMemoryBus : IMessageBus, IDisposable
    {

        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IEventPublisher _eventPublisher;

        public InMemoryBus(
            ICommandDispatcher commandDispathcer,
            IEventPublisher eventPublisher)
        {
            this._commandDispatcher = commandDispathcer;
            this._eventPublisher = eventPublisher;
        }


        public void Send<TCommand>(TCommand message)
        {
            _commandDispatcher.Dispatch(message);
        }

        public void Subscribe<TEvent>()
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent>()
        {
            throw new NotImplementedException();
        }

        public void Publish<TEvent>(TEvent message)
        {
            _eventPublisher.Publish(message);
        }


        #region Dispose

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //noop atm
                }
                disposed = true;
            }
        }
        #endregion



    }
}
