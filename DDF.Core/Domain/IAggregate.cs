using System;
using System.Collections.Generic;
namespace DDF.Core.Domain
{
    public interface IAggregate
    {
        long AggregateVersion { get; }
        IEnumerable<IDomainEvent> GetUncommittedChanges();
        Guid Id { get; }
        void LoadFromHistory(IEnumerable<IDomainEvent> events);
        void ClearUncommittedChanges();
    }
}
