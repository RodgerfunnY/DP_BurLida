using DP_BurLida.Data.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DP_BurLida.Data
{
    public class ByrlidaContext : DbContext
    {
        public DbSet<OrderModelData> OrderModelData { get; set; }
        public DbSet<BrigadeModelData> BrigadeModelData { get; set; }
        public DbSet<UserModelData> UserModelData { get; set; }
        public DbSet<SkladModelData> SkladModelData { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderModelData>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameClient) //работа со столбцом
                .IsRequired() //значение должно быть обязательно
                .HasMaxLength(30); //Макс длинна 
                entity.Property(e => e.SurnameClient)
                .IsRequired()
                .HasMaxLength(30);
                entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(14);
                entity.Property(e => e.Area)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.District)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.Diameter)
                .IsRequired()
                .HasMaxLength(4);
                entity.Property(e => e.PricePerMeter)
                .IsRequired()
                .HasMaxLength(4);
                entity.Property(e => e.Pump)
                .IsRequired()
                .HasMaxLength(5);
                entity.Property(e => e.Arrangement)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200);
                entity.Property(e => e.CreationTimeData)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.WorkDate)
                .IsRequired(false);
                entity.HasOne(e => e.DrillingBrigade)
                .WithMany()
                .HasForeignKey(e => e.DrillingBrigadeId)
                .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(e => e.ArrangementBrigade)
                .WithMany()
                .HasForeignKey(e => e.ArrangementBrigadeId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<BrigadeModelData>(entity =>
            {
                entity.ToTable("BrigadeModelData");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameBrigade)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(e => e.Technic)
                .HasMaxLength(200);
                entity.Property(e => e.Info)
                .HasMaxLength(500);
                entity.HasOne(e => e.ResponsibleUser)
                .WithMany()
                .HasForeignKey(e => e.ResponsibleUserId)
                .OnDelete(DeleteBehavior.SetNull);
            });

        }

        public ByrlidaContext(DbContextOptions<ByrlidaContext> options)
        : base(options) { }

    }
}
