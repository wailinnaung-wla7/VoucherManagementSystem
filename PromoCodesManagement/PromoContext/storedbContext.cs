using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PromoCodesManagement.PromoContext
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

        public virtual DbSet<Evoucher> Evoucher { get; set; }
        public virtual DbSet<EvoucherPurchase> EvoucherPurchase { get; set; }
        public virtual DbSet<Promocodes> Promocodes { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySQL("server=wailinaung;uid=username;pwd=password;database=storedb");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<EvoucherPurchase>(entity =>
            {
                entity.ToTable("evoucher_purchase");

                entity.Property(e => e.EvoucherId).HasColumnName("evoucherId");

                entity.Property(e => e.IsGenerated).HasColumnType("bit(1)");

                entity.Property(e => e.PurchaseAmount).HasColumnType("decimal(10,0)");

                entity.Property(e => e.PurchasePhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,0)");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Promocodes>(entity =>
            {
                entity.ToTable("promocodes");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.PromoCode).HasMaxLength(20);

                entity.Property(e => e.Qrcode).HasColumnName("QRCode");

                entity.Property(e => e.Status).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
