using DDF.Infrastructure.Messaging.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Impl
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private ICommandHandlerFactory _commandHandlerFactory;

        public CommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            this._commandHandlerFactory = commandHandlerFactory;
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            var handlers = _commandHandlerFactory.GetHandlersForCommand(command);
            foreach (var handler in handlers)
                handler.Handle(command);
        }
    }
}
