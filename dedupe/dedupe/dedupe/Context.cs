using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;

namespace dedupe
{
    class Context: DbContext
    {
        private static string GetConnectionStringName() {
            return ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;            
        }

        public Context()
            : base(GetConnectionStringName())
        { }


        public DbSet<Street> Streets { get; set; }
        public DbSet<BuildingFull> BuildingsFull { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);            
        }
    }
}
