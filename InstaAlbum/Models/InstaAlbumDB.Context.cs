﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InstaAlbum.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InstaAlbumEntities : DbContext
    {
        public InstaAlbumEntities()
            : base("name=InstaAlbumEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblBranch> tblBranches { get; set; }
        public virtual DbSet<tblCategory> tblCategories { get; set; }
        public virtual DbSet<tblCity> tblCities { get; set; }
        public virtual DbSet<tblCustomer> tblCustomers { get; set; }
        public virtual DbSet<tblFeedback> tblFeedbacks { get; set; }
        public virtual DbSet<tblGallery> tblGalleries { get; set; }
        public virtual DbSet<tblOrder> tblOrders { get; set; }
        public virtual DbSet<tblPackage> tblPackages { get; set; }
        public virtual DbSet<tblPhotographer> tblPhotographers { get; set; }
        public virtual DbSet<tblSelfiePoint> tblSelfiePoints { get; set; }
        public virtual DbSet<tblState> tblStates { get; set; }
        public virtual DbSet<tblStudioAdmin> tblStudioAdmins { get; set; }
    }
}