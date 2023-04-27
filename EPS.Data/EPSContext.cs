using System;
using System.ComponentModel.DataAnnotations.Schema;
using EPS.Data.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EPS.Data
{
    public partial class EPSContext : DbContext, IDataProtectionKeyContext
    {
        public EPSContext()
        {
        }

        public EPSContext(DbContextOptions<EPSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public virtual DbSet<IdentityClient> IdentityClients { get; set; }
        public virtual DbSet<IdentityRefreshToken> IdentityRefreshTokens { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePrivilege> RolePrivileges { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPrivilege> UserPrivileges { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<hotel> hotels { get; set; }
        public virtual DbSet<tour> tours { get; set; }
        public virtual DbSet<detail_tour> detail_tours { get; set; }
        public virtual DbSet<tour_connect_hotel> tour_connect_hotels { get; set; }
        public virtual DbSet<evaluate_tour> evaluate_tours { get; set; }
        public virtual DbSet<register_tour> register_tours { get; set; }
        public virtual DbSet<image_tour> image_tours { get; set; }
        public virtual DbSet<v_detail_tour_register> v_detail_tour_register { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=DESKTOP-S9FNGVF\\SQLEXPRESS;User ID=quangle;Password=07081999;Database=TourDuLich;Integrated Security=false;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<UserPrivilege>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PrivilegeId });
            });

            modelBuilder.Entity<RolePrivilege>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PrivilegeId });
            });

            modelBuilder.Entity<tour_connect_hotel>(entity =>
            {
                entity.HasKey(e => new { e.id_tour, e.id_hotel});
            });

            modelBuilder.Entity<hotel>()
            .HasOne<category>(s => s.category)
            .WithMany(g => g.hotels)
            .HasForeignKey(s => s.category_id);

            modelBuilder.Entity<tour>()
            .HasOne<category>(s => s.category)
            .WithMany(g => g.tours)
            .HasForeignKey(s => s.category_id);

            modelBuilder.Entity<evaluate_tour>()
            .HasOne<tour>(s => s.tour)
            .WithMany(g => g.evaluate_tours)
            .HasForeignKey(s => s.id_tour);

            modelBuilder.Entity<register_tour>()
            .HasOne<tour>(s => s.tour)
            .WithMany(g => g.register_tours)
            .HasForeignKey(s => s.id_tour);

            modelBuilder.Entity<image_tour>()
            .HasOne<tour>(s => s.tour)
            .WithMany(g => g.image_tours)
            .HasForeignKey(s => s.id_tour);

            modelBuilder.Entity<tour>()
            .HasOne<detail_tour>(s => s.detail_tour)
            .WithOne(ad => ad.tour)
            .HasForeignKey<detail_tour>(ad => ad.id_tour);

        }
    }
}
