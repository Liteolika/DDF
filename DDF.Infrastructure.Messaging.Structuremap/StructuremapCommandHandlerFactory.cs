using DDF.Infrastructure.Messaging.Commands;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Structuremap
{
    public class StructuremapCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IContainer _container;

        public StructuremapCommandHandlerFactory(IContainer container)
        {
            this._container = container;
        }


        public IEnumerable<ICommandHandler<T>> GetHandlersForCommand<T>(T command)
        {
            var handlers = _container.GetAllInstances(typeof(ICommandHandler<T>));
            foreach (var handler in handlers)
                yield return handler as ICommandHandler<T>;

        }
        
    }
}
