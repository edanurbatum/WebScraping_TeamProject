﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebScraping.Models.Siniflar
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WebScrapingDBEntities : DbContext
    {
        public WebScrapingDBEntities()
            : base("name=WebScrapingDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Homes> Homes { get; set; }
        public virtual DbSet<HomesSahibinden> HomesSahibinden { get; set; }
        public virtual DbSet<il> il { get; set; }
        public virtual DbSet<ilce> ilce { get; set; }
        public virtual DbSet<mahalle> mahalle { get; set; }
    }
}
