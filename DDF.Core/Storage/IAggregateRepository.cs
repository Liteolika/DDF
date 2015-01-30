using DDF.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage
{
    public interface IAggregateRepository
    {
        T GetById<T>(Guid aggregateId) where T : IAggregate;
        void Save(IAggregate aggregate);
    }
}
