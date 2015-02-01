using LCF.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCF.Core.Domain
{
    public abstract class AggregateBase
    {

        public Guid Id { get; private set; }


        public AggregateBase(Guid id)
        {

        }


    }
}
