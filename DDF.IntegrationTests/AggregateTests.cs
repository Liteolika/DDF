using DDF.Domain.Shared;
using DDF.Infrastructure.EventSourcing;
using DDF.Infrastructure.EventSourcing.Imp;
using DDF.Infrastructure.Messaging;
using DDF.Infrastructure.Messaging.Commands;
using DDF.Infrastructure.Messaging.Events;
using DDF.Infrastructure.Messaging.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.IntegrationTests
{
    [TestFixture]
    public class AggregateTests : TestBaseWithMessageBus
    {

        private readonly IMessageBus bus;

        public AggregateTests()
        {
            container.Configure(cfg =>
            {
                cfg.For(typeof(IAggregateRepository<>)).Use(typeof(AggregateRepository<>));
                cfg.For<IEventStore>().Use<InMemoryEventStorage>();
            });

            
            ICommandDispatcher cmdDispatcher = container.GetInstance<ICommandDispatcher>();
            IEventPublisher eventPublisher = container.GetInstance<IEventPublisher>();
            bus = new InMemoryBus(cmdDispatcher, eventPublisher);
        }

        [Test]
        public void AggTest()
        {

            

            InMemoryEventStorage eventStore = new InMemoryEventStorage();
            AggregateRepository<MyThing> repo = new AggregateRepository<MyThing>(eventStore);
            MyThingCommandHandler handler = new MyThingCommandHandler(repo);

            CreateMyThing command = new CreateMyThing(Guid.NewGuid(), "Peter");
            bus.Send(command);

            

        }

    }

    public class MyThing : AggregateBase
    {

        private string _title { get; set; }

        public MyThing() {}

        public MyThing(Guid id, string title)
        {
            ApplyEvent(new MyThingCreated(id, title));
        }

        private void ApplyEvent(MyThingCreated @event)
        {
            this.Id = @event.Id;
            this._title = @event.Title;
        }
        
    }

    public class MyThingCommandHandler : 
        ICommandHandler<CreateMyThing>
    {

        private readonly IAggregateRepository<MyThing> _repo;

        public MyThingCommandHandler(
            IAggregateRepository<MyThing> aggregateRepo)
        {
            this._repo = aggregateRepo;
        }

        public void Handle(CreateMyThing command)
        {
            MyThing t = new MyThing(command.Id, command.Title);
            
            _repo.Save(t);

            var a = 1;
        }
         
    }

    public class CreateMyThing
    {
        public CreateMyThing(Guid id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }
    }

    public class MyThingCreated
    {
        public MyThingCreated(Guid id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }
    }


}
