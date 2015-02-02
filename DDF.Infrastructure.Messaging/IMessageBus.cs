using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging
{
    public interface IMessageBus
    {

        // Commands
        void Send<TCommand>(TCommand message);

        //Events
        void Subscribe<TEvent>();

        void Unsubscribe<TEvent>();

        void Publish<TEvent>(TEvent message);

    }
}
