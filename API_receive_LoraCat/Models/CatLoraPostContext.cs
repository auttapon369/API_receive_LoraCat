using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_receive_LoraCat.Models
{
    public partial class CatLoraPostContext : DbContext
    {
        public CatLoraPostContext()
        {
        }

        public CatLoraPostContext(DbContextOptions<CatLoraPostContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collecteddata> Collecteddata { get; set; }
        public virtual DbSet<Collecteddataevent> Collecteddataevent { get; set; }
        public virtual DbSet<Station> Station { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=******;port=3306;user=****;password=****;database=CatLoraPost");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collecteddata>(entity =>
            {
                entity.ToTable("collecteddata");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ack)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Beacon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Channel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DevEui)
                    .IsRequired()
                    .HasColumnName("DevEUI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FcntDw).HasColumnName("FCntDw");

                entity.Property(e => e.FcntUp).HasColumnName("FCntUp");

                entity.Property(e => e.Fport).HasColumnName("FPort");

                entity.Property(e => e.Lrr)
                    .IsRequired()
                    .HasColumnName("LRR")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rssi).HasColumnName("RSSI");

                entity.Property(e => e.Rx1)
                    .HasColumnName("RX1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rx2)
                    .HasColumnName("RX2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SensorId)
                    .IsRequired()
                    .HasColumnName("sensorID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Snr).HasColumnName("SNR");

                entity.Property(e => e.SubBand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Collecteddataevent>(entity =>
            {
                entity.ToTable("collecteddataevent");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ack)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Beacon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Channel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DevEui)
                    .IsRequired()
                    .HasColumnName("DevEUI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FcntDw).HasColumnName("FCntDw");

                entity.Property(e => e.FcntUp).HasColumnName("FCntUp");

                entity.Property(e => e.Fport).HasColumnName("FPort");

                entity.Property(e => e.Lrr)
                    .IsRequired()
                    .HasColumnName("LRR")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rssi).HasColumnName("RSSI");

                entity.Property(e => e.Rx1)
                    .HasColumnName("RX1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rx2)
                    .HasColumnName("RX2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SensorId)
                    .IsRequired()
                    .HasColumnName("sensorID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Snr).HasColumnName("SNR");

                entity.Property(e => e.SubBand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("station");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddressStation)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DevEui)
                    .IsRequired()
                    .HasColumnName("DevEUI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameStation)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
