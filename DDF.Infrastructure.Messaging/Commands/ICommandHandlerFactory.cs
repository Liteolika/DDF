using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Commands
{
    public interface ICommandHandlerFactory
    {
        IEnumerable<ICommandHandler<T>> GetHandlersForCommand<T>(T command);
    }
}
