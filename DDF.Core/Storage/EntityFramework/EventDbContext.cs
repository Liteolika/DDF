using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Storage.EntityFramework
{
    public class EventDbContext : DbContext
    {



        public DbSet<DbEvent> Events { get; set; }
        public DbSet<StreamEntry> StreamInfos { get; set; }
        public DbSet<StreamSnapshot> Snapshots { get; set; }

        public EventDbContext()
            : base("EventStore")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

    }

    

    
}
