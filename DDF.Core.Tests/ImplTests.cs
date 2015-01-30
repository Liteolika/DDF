using DDF.Core.Domain;
using DDF.Core.Messaging;
using DDF.Core.Storage;
using DDF.Core.Storage.EntityFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Tests
{
    [TestFixture]
    public class ImplTests
    {

        [Test]
        public void AllOfIt()
        {

            IMessageBus bus = new MessageBus();
            IEventStore eventStore = new EventStore(bus, new EventDbFactory());
            IAggregateRepository repository = new AggregateRepository(eventStore);

            MyTestHandler handler = new MyTestHandler(repository);
            bus.Commands.RegisterHandler(handler);
            bus.Events.RegisterHandler(handler);

            MyEventHandler eh = new MyEventHandler();
            bus.Events.RegisterHandler(eh);

            Guid id = Guid.NewGuid();
            MyCreateCommand cmd = new MyCreateCommand(id, "PETER");

            bus.Commands.Dispatch(cmd);

            var theStoredAgg = repository.GetById<MyTestAggregate>(id);

            for (int i = 0; i < 22; i++)
            {
                bus.Commands.Dispatch(new MyChangeCommand(id, "PETER " + i.ToString()));
            }

            theStoredAgg = repository.GetById<MyTestAggregate>(id);

            var a = 1;

        }

    }

    public class MyEventHandler
    {
        public void Handle(CreatedEvent @event)
        {
            var a = 1;
        }
    }

    public class MyTestHandler
    {
        private readonly IAggregateRepository _repo;

        public MyTestHandler(IAggregateRepository repo)
        {
            _repo = repo;
        }

        public void Handle(MyCreateCommand command)
        {
            MyTestAggregate aggregate = new MyTestAggregate(command.Id, command.Name);
            _repo.Save(aggregate);
        }

        public void Handle(MyChangeCommand command)
        {
            MyTestAggregate aggregate = _repo.GetById<MyTestAggregate>(command.Id);
            aggregate.UpdateFromCommand(command);
            _repo.Save(aggregate);
        }

    }

    public class MyChangeCommand : ICommand
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
        public MyChangeCommand(Guid id, string newName)
        {
            this.Id = id;
            this.NewName = newName;
        }
    }

    public class MyCreateCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MyCreateCommand(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class ChangedEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ChangedEvent(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class CreatedEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CreatedEvent(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class MyTestAggregate : AggregateBase
    {
        // Private fields
        private string _name = string.Empty;

        // Constructors

        public MyTestAggregate(Guid id, string name)
        {
            Apply(new CreatedEvent(id, name));
        }

        public MyTestAggregate() { }

        // Public properties
        //public string Name
        //{
        //    get { return _name; }
        //    set
        //    {
        //        _name = value;
        //        Apply(new ChangedEvent(this.Id, _name));
        //    }
        //}

        public void UpdateFromCommand(MyChangeCommand command)
        {
            Apply(new ChangedEvent(this.Id, command.NewName));
        }

        // EventHandlers
        private void UpdateFromEvent(CreatedEvent @event)
        {
            this.Id = @event.Id;
            this._name = @event.Name;
        }

        private void UpdateFromEvent(ChangedEvent @event)
        {
            this._name = @event.Name;
        }



        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }

}
