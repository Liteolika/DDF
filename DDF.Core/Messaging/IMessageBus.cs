using DDF.Core.Domain;
using System;
namespace DDF.Core.Messaging
{
    public interface IMessageBus
    {
        IMessageDispatcher<ICommand> Commands { get; }
        IMessageDispatcher<IDomainEvent> Events { get; }
        IMessageDispatcher<INotification> Notifications { get; }
    }
}
