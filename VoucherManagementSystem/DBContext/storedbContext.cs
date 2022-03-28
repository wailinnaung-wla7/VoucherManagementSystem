using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VoucherManagementSystem.DBContext
{
    public partial class storedbContext : DbContext
    {
        public storedbContext()
        {
        }

        public storedbContext(DbContextOptions<storedbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CmsUserrefreshtokens> CmsUserrefreshtokens { get; set; }
        public virtual DbSet<Evoucher> Evoucher { get; set; }
        public virtual DbSet<CmsUser> CmsUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySQL("server=wailinaung;uid=username;pwd=password;database=storedb");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CmsUser>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PRIMARY");

                entity.ToTable("cms_user");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(45);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(100);

                entity.Property(e => e.Phonenum)
                    .IsRequired()
                    .HasColumnName("phonenum")
                    .HasMaxLength(45);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(45);
            });
            modelBuilder.Entity<CmsUserrefreshtokens>(entity =>
            {
                entity.ToTable("cms_userrefreshtokens");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Createdbyip)
                    .IsRequired()
                    .HasColumnName("createdbyip")
                    .HasMaxLength(45);

                entity.Property(e => e.Expires).HasColumnName("expires");

                entity.Property(e => e.Replacedbytoken)
                    .HasColumnName("replacedbytoken")
                    .HasMaxLength(200);

                entity.Property(e => e.Revoked).HasColumnName("revoked");

                entity.Property(e => e.Revokedbyip)
                    .HasColumnName("revokedbyip")
                    .HasMaxLength(45);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasMaxLength(200);

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Evoucher>(entity =>
            {
                entity.ToTable("evoucher");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,0)");

                entity.Property(e => e.BuyType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Image).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("bit(1)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
