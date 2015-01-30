using DDF.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Tests.Domain
{
    [TestFixture]
    public class creating_object_derived_from_aggregatebase
    {

        [Test]
        public void it_has_default_guid()
        {
            AggClass a = new AggClass();
            Assert.AreEqual(Guid.Empty, a.Id);
        }

        [Test]
        public void it_has_version_0()
        {
            AggClass a = new AggClass();
            Assert.AreEqual(0, a.AggregateVersion);
        }

        [Test]
        public void it_can_apply_eventhistory()
        {
            AggClass a = new AggClass();

            List<IDomainEvent> events = new List<IDomainEvent>();
            events.Add(new SomeEvent() { Name = "A" });
            events.Add(new SomeEvent() { Name = "B" });
            a.LoadFromHistory(events);

            Assert.AreEqual("B", a.Name);
        }

        [Test]
        public void it_throws_on_events_not_handled()
        {
            AggClass a = new AggClass();

            List<IDomainEvent> events = new List<IDomainEvent>();
            events.Add(new SomeOtherEvent() { Name = "A" });

            Assert.Throws<MissingMethodException>(() =>
            {
                a.LoadFromHistory(events);
            });
            
        }



    }

    public class SomeOtherEvent : IDomainEvent
    {
        public string Name { get; set; }
    }

    public class SomeEvent : IDomainEvent
    {
        public string Name { get; set; }
    }

    public class AggClass : AggregateBase
    {
        public string Name;
        
        private void UpdateFromEvent(SomeEvent @event)
        {
            this.Name = @event.Name;
        }


        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }

    

}
