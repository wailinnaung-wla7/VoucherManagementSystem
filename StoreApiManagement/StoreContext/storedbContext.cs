using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StoreApiManagement.StoreContext
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

        public virtual DbSet<CusUser> CusUser { get; set; }
        public virtual DbSet<CusUserrefreshtokens> CusUserrefreshtokens { get; set; }
        public virtual DbSet<Evoucher> Evoucher { get; set; }
        public virtual DbSet<EvoucherPurchase> EvoucherPurchase { get; set; }
        public virtual DbSet<Paymentmethods> Paymentmethods { get; set; }
        public virtual DbSet<PaymentHistory> PaymentHistory { get; set; }
        public virtual DbSet<Promocodes> Promocodes { get; set; }

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
            modelBuilder.Entity<CusUser>(entity =>
            {
                entity.ToTable("cus_user");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("Full Name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<CusUserrefreshtokens>(entity =>
            {
                entity.ToTable("cus_userrefreshtokens");

                entity.Property(e => e.CreatedByIp)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ReplacedByToken).HasMaxLength(200);

                entity.Property(e => e.RevokedByIp).HasMaxLength(20);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(200);
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

            modelBuilder.Entity<Promocodes>(entity =>
            {
                entity.ToTable("promocodes");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.PromoCode).HasMaxLength(20);

                entity.Property(e => e.Qrcode).HasColumnName("QRCode");

                entity.Property(e => e.Status).HasMaxLength(10);
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

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,0)");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Paymentmethods>(entity =>
            {
                entity.ToTable("paymentmethods");

                entity.Property(e => e.PaymentMethodName)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.ToTable("payment_history");

                entity.Property(e => e.AccountNumber).HasMaxLength(45);

                entity.Property(e => e.Amount).HasColumnType("decimal(10,0)");

                entity.Property(e => e.Cvv).HasColumnName("CVV");

                entity.Property(e => e.EvoucherpurchaseId).HasColumnName("evoucherpurchaseId");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
