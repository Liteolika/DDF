using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage.EntityFramework
{
    public class EventDbFactory : IDbContextFactory<EventDbContext>
    {

        public EventDbContext Create()
        {
            return new EventDbContext();
        }
    }
}
