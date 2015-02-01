using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCF.Core.Events
{
    public class EventRepository
    {
        private readonly EventStore _store;

        public EventRepository(EventStore store)
        {
            this._store = store;
        }

    }
}
