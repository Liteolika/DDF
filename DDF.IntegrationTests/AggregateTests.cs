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
                cfg.For(typeof(IAggregateRepository<>)).Singleton().Use(typeof(AggregateRepository<>));
                cfg.For<IEventStore>().Singleton().Use<InMemoryEventStorage>();
            });


            ICommandDispatcher cmdDispatcher = container.GetInstance<ICommandDispatcher>();
            IEventPublisher eventPublisher = container.GetInstance<IEventPublisher>();
            bus = new InMemoryBus(cmdDispatcher, eventPublisher);
        }

        [Test]
        public void AggTest()
        {
            //InMemoryEventStorage eventStore = new InMemoryEventStorage();
            //IAggregateRepository<MyThing> repo = new AggregateRepository<MyThing>(eventStore);
            //MyThingCommandHandler handler = new MyThingCommandHandler(repo);

            Guid id = Guid.NewGuid();

            CreateMyThing command = new CreateMyThing(id, "Peter");
            bus.Send(command);

            var repo = container.GetInstance<IAggregateRepository<MyThing>>();
            MyThing thing = repo.GetAggregate(id);

            bus.Send(new UpdateMyThing(id, "The new TIIIITLE!!!"));

            MyThing thing2 = repo.GetAggregate(id);

            bus.Send(new UpdateMyThing(id, "The new TIIIITLE!!!2"));
            bus.Send(new UpdateMyThing(id, "The new TIIIITLE!!!3"));
            bus.Send(new UpdateMyThing(id, "The new TIIIITLE!!!4"));

            MyThing thing3 = repo.GetAggregate(id);

            bus.Send(new AddSubThing(id, new MySubThing("Hej")));
            bus.Send(new AddSubThing(id, new MySubThing("Hopp")));

            MyThing thing4 = repo.GetAggregate(id);

            var st = thing4.SubThings.ToList().First();

            bus.Send(new RemoveSubThing(id, st.Id));

            MyThing thing5 = repo.GetAggregate(id);

            bus.Send(new UpdateMyThing(id, ""));

            MyThing thing6 = repo.GetAggregate(id);

            var a = 1;

        }

    }

    public class MyThing : AggregateBase
    {

        private string _title { get; set; }
        private List<MySubThing> subThings;

        public IEnumerable<MySubThing> SubThings
        {
            get { return subThings; }
        }

        public MyThing()
        {
            this.subThings = new List<MySubThing>();
        }

        public MyThing(Guid id, string title)
            : this()
        {
            ApplyEvent(new MyThingCreated(id, title));
        }

        private void Handle(MyThingCreated @event)
        {
            this.AggregateId = @event.AggregateId;
            this._title = @event.Title;
        }

        private void Handle(MyThingUpdated @event)
        {
            this._title = @event.Title;
        }

        private void Handle(MySubThingAdded @event)
        {
            this.subThings.Add(@event.Subthing);
        }

        private void Handle(SubThingRemoved @event)
        {
            var subthing = this.subThings.Where(x => x.Id == @event.SubThingId).FirstOrDefault();
            this.subThings.Remove(subthing);
        }

        public override string ToString()
        {
            return this._title;
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(this._title))
                return false;

            return true;
        }

    }

    public class MySubThing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public MySubThing(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
        }
    }


    public class MyThingCommandHandler :
        ICommandHandler<CreateMyThing>,
        ICommandHandler<UpdateMyThing>,
        ICommandHandler<AddSubThing>,
        ICommandHandler<RemoveSubThing>
    {

        private readonly IAggregateRepository<MyThing> _repo;

        public MyThingCommandHandler(
            IAggregateRepository<MyThing> aggregateRepo)
        {
            this._repo = aggregateRepo;
        }

        public void Handle(CreateMyThing command)
        {
            MyThing t = new MyThing(command.AggregateId, command.Title);

            _repo.SaveAggregate(t);

            var a = 1;
        }

        public void Handle(UpdateMyThing command)
        {
            MyThing t = _repo.GetAggregate(command.Id);
            t.ApplyEvent(new MyThingUpdated(t.AggregateId, command.Title));
            _repo.SaveAggregate(t);

        }
        
        public void Handle(AddSubThing command)
        {
            MyThing t = _repo.GetAggregate(command.AggregateId);
            t.ApplyEvent(new MySubThingAdded(t.AggregateId, command.Subthing));
            _repo.SaveAggregate(t);
        }

        public void Handle(RemoveSubThing command)
        {
            MyThing t = _repo.GetAggregate(command.AggregateId);
            t.ApplyEvent(new SubThingRemoved(t.AggregateId, command.SubThingId));
            _repo.SaveAggregate(t);
        }
    }

    public class RemoveSubThing : CommandBase
    {
        public RemoveSubThing(Guid aggregateId, Guid subThingId)
            : base(aggregateId)
        {
            this.SubThingId = subThingId;
        }

        public Guid SubThingId { get; set; }
    }

    public class SubThingRemoved : EventBase
    {
        public SubThingRemoved(Guid aggregateId, Guid subThingId)
            : base(aggregateId)
        {
            this.SubThingId = subThingId;
        }

        public Guid SubThingId { get; set; }
    }


    public class AddSubThing : CommandBase
    {
        public AddSubThing(Guid aggregateId, MySubThing subThing)
            : base(aggregateId)
        {
            this.Subthing = subThing;
        }

        public MySubThing Subthing { get; set; }
    }

    public class MySubThingAdded : EventBase
    {
        public MySubThingAdded(Guid aggregateId, MySubThing subThing)
            : base(aggregateId)
        {
            this.Subthing = subThing;
        }

        public MySubThing Subthing { get; set; }
    }

    public class UpdateMyThing
    {
        public UpdateMyThing(Guid aggregateId, string title)
        {
            this.Id = aggregateId;
            this.Title = title;
        }

        public string Title { get; set; }

        public Guid Id { get; set; }
    }

    public class MyThingUpdated : EventBase
    {
        public MyThingUpdated(Guid aggregateId, string title)
            : base(aggregateId)
        {
            this.Title = title;
        }

        public string Title { get; set; }
    }


    public class CreateMyThing
    {
        public CreateMyThing(Guid aggregateId, string title)
        {
            this.AggregateId = aggregateId;
            this.Title = title;
        }

        public Guid AggregateId { get; set; }

        public string Title { get; set; }
    }



    public class MyThingCreated : EventBase
    {
        public MyThingCreated(Guid aggregateId, string title)
            : base(aggregateId)
        {
            this.Title = title;
        }

        public string Title { get; set; }
    }


}
