using DDF.Infrastructure.Messaging;
using DDF.Infrastructure.Messaging.Commands;
using DDF.Infrastructure.Messaging.Events;
using DDF.Infrastructure.Messaging.Impl;
using DDF.Infrastructure.Messaging.Structuremap;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.IntegrationTests
{
    public class TestBaseWithMessageBus
    {

        protected IContainer container;

        public TestBaseWithMessageBus()
        {
            container = new Container();
            container.Configure(cfg =>
            {
                cfg.Scan(scan =>
                {
                    scan.WithDefaultConventions();
                    scan.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                    scan.TheCallingAssembly();
                });

                cfg.For<IMessageBus>().Use<InMemoryBus>();
                cfg.For<ICommandHandlerFactory>().Use<StructuremapCommandHandlerFactory>();
                cfg.For<IEventHandlerFactory>().Use<StructuremapEventHandlerFactory>();
                cfg.For<IEventPublisher>().Use<EventPublisher>();
                cfg.For<ICommandDispatcher>().Use<CommandDispatcher>();

            });
        }

    }
}
