using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.EventSourcing.EntityFramework
{
    public class EfDbContextFactory : IDbContextFactory<EventContext>
    {

        private readonly string _connectionStringOrName;

        public EfDbContextFactory(string connectionStringOrName)
        {
            this._connectionStringOrName = connectionStringOrName;
        }

        public EventContext Create()
        {
            return new EventContext(_connectionStringOrName);
        }
    }
}
