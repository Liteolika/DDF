using DDF.Core.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly Dictionary<Type, object> _handlers =
            new Dictionary<Type, object>();

        private readonly IMessageDispatcher<ICommand> _Commands;
        private readonly IMessageDispatcher<INotification> _Notifications;
        private readonly IMessageDispatcher<IDomainEvent> _DomainEvents;

        public IMessageDispatcher<ICommand> Commands
        { get { return _Commands; } }

        public IMessageDispatcher<INotification> Notifications
        { get { return _Notifications; } }

        public IMessageDispatcher<IDomainEvent> Events
        { get { return _DomainEvents; } }


        public MessageBus()
        {
            this._Commands = new MessageDispatcher<ICommand>(false);
            this._Notifications = new MessageDispatcher<INotification>(true);
            this._DomainEvents = new MessageDispatcher<IDomainEvent>(true);
        }

    }

    



    
}
