﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AddressAPITest001
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GUKVEntities : DbContext
    {
        public GUKVEntities()
            : base("name=GUKVEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<adresesUrbio_address> adresesUrbio_address { get; set; }
        public virtual DbSet<adresesUrbio_address_geolocation> adresesUrbio_address_geolocation { get; set; }
        public virtual DbSet<adresesUrbio_address_history> adresesUrbio_address_history { get; set; }
        public virtual DbSet<adresesUrbio_address_incorrect> adresesUrbio_address_incorrect { get; set; }
        public virtual DbSet<adresesUrbio_street> adresesUrbio_street { get; set; }
        public virtual DbSet<adresesUrbio_street_district> adresesUrbio_street_district { get; set; }
        public virtual DbSet<adresesUrbio_street_geolocation> adresesUrbio_street_geolocation { get; set; }
        public virtual DbSet<adresesUrbio_street_history> adresesUrbio_street_history { get; set; }
        public virtual DbSet<adresesUrbio_street_incorrect> adresesUrbio_street_incorrect { get; set; }
    }
}