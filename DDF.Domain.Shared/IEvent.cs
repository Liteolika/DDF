using System;
namespace DDF.Domain.Shared
{
    public interface IEvent
    {
        Guid AggregateId { get; set; }
        Guid EventId { get; set; }
        DateTimeOffset EventTimestamp { get; set; }
    }
}
