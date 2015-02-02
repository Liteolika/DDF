using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Events
{
    public interface IEventHandlerFactory
    {
        //ICommandHandler<T>[] GetHandlersForCommand<T>(T command);
        IEnumerable<IEventHandler<T>> GetHandlersForEvent<T>(T command);
    }
}
