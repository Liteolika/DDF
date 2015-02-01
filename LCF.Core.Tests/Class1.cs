using LCF.Core.Domain;
using LCF.Core.Events;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCF.Core.Tests
{
    [TestFixture]
    public class AggregateAndEventTests
    {

        [Test]
        public void TestName()
        {

            MyDomainAggregate o = new MyDomainAggregate(Guid.NewGuid());

            o.Title = "Hej";

            EventRepository repo = new EventRepository(new EventStore());

            

        }

    }


    public class MyDomainAggregate : AggregateBase
    {

        public MyDomainAggregate(Guid id)
            : base(id)
        {

        }

        public string Title { get; private set; }


        
        
        

    }

    public class TitleChangedEvent : IDomainEvent
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }

        public TitleChangedEvent(Guid id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
    }


}
