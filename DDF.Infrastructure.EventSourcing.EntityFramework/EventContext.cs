using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing.EntityFramework
{
    public class EventContext : DbContext
    {

        public EventContext(string connectionStringOrName)
            : base(connectionStringOrName)
        {

        }

    }
}
