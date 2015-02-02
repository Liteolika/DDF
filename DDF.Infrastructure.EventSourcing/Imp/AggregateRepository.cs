using DDF.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing.Imp
{
    public class AggregateRepository<T> : IAggregateRepository<T>
    {

        private IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            this._eventStore = eventStore;
        }

    }
}
