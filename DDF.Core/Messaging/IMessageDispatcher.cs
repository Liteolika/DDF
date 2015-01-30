using System;
using System.Collections.Generic;
namespace DDF.Core.Messaging
{
    public interface IMessageDispatcher<T>
     where T : IMessage
    {
        void Dispatch(T message);
        void Dispatch(IEnumerable<T> messages);
        void RegisterHandler(object handlerObject);
    }
}
