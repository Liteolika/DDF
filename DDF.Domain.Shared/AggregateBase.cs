using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Domain.Shared
{
    public abstract class AggregateBase : IAggregate
    {
        
        public Guid Id { get; set; }
        


        
    }
}
