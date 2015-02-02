using DDF.Infrastructure.Messaging.Events;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Structuremap
{
    public class StructuremapEventHandlerFactory : IEventHandlerFactory
    {
        private readonly IContainer _container;

        public StructuremapEventHandlerFactory(IContainer container)
        {
            this._container = container;
        }

        public IEnumerable<IEventHandler<T>> GetHandlersForEvent<T>(T command)
        {
            var handlers = _container.GetAllInstances(typeof(IEventHandler<T>));
            foreach (var handler in handlers)
                yield return handler as IEventHandler<T>;
        }
    }
}
