using DDF.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing
{
    public interface IAggregateRepository<T>
        where T : AggregateBase
    {
        void SaveAggregate(T aggregate);
        T GetAggregate(Guid AggregateId);
    }
}
