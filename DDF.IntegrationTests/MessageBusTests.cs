using DDF.Infrastructure.Messaging;
using DDF.Infrastructure.Messaging.Commands;
using DDF.Infrastructure.Messaging.Events;
using DDF.Infrastructure.Messaging.Impl;
using DDF.Infrastructure.Messaging.Structuremap;
using NUnit.Framework;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.IntegrationTests
{
    [TestFixture]
    public class MessageBusTests : TestBaseWithMessageBus
    {

        public MessageBusTests()
        {
            var cc = container.WhatDoIHave();
            var a = 1;
        }

        [Test]
        public void TestName()
        {

            ICommandDispatcher cmdDispatcher = container.GetInstance<ICommandDispatcher>();
            IEventPublisher eventPublisher = container.GetInstance<IEventPublisher>();
            IMessageBus bus = new InMemoryBus(cmdDispatcher, eventPublisher);

            bus.Send(new TheCommand("My Command"));

        }

        public class TheCommand
        {
            public string CommandName { get; set; }
            public TheCommand(string commandName)
            {
                this.CommandName = commandName;
            }
        }

        public class TheEvent
        {
            public string EventName { get; set; }
            public TheEvent(string eventName)
            {
                this.EventName = eventName;
            }
        }

        public class TheEventHandler :
            IEventHandler<TheEvent>
        {

            public void Handle(TheEvent @event)
            {
                var a = 1;
            }
        }

        public class TheOtherEventHandler :
            IEventHandler<TheEvent>
        {

            public void Handle(TheEvent @event)
            {
                var a = 1;
            }
        }

        public class TheCommandHandler :
            ICommandHandler<TheCommand>
        {

            private IMessageBus _bus;

            public TheCommandHandler(IMessageBus bus)
            {
                this._bus = bus;
            }

            public void Handle(TheCommand command)
            {
                var a = 1;
                _bus.Publish(new TheEvent("My Event for " + command.CommandName));
            }
        }

    }

    



}
