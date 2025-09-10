using DP_BurLida.Data.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DP_BurLida.Data
{
    //Необхид данный класс для работы с баззой данных через прил
    public class ByrlidaContext : DbContext
    {
        // определние таблиц с моделью
        public DbSet<OrderModelData> OrderModelData { get; set; }
        public DbSet<BrigadeModelData> BrigadeModelData { get; set; }
        public DbSet<UserModelData> UserModelData { get; set; }
        public DbSet<SkladModelData> SkladModelData { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderModelData>(entity =>
            {
                entity.ToTable("Заказы");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameClient) //работа со столбцом
                .HasColumnName("Имя") //название столбца
                .IsRequired() //значение должно быть обязательно
                .HasMaxLength(30); //Макс длинна 
                entity.Property(e => e.SurnameClient)
                .HasColumnName("Фамилия")
                .IsRequired()
                .HasMaxLength(30);
                entity.Property(e => e.Phone)
                .HasColumnName("Номер телефона")
                .IsRequired()
                .HasMaxLength(14);
                entity.Property(e => e.Area)
                .HasColumnName("Область")
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.District)
                .HasColumnName("Район")
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.City)
                .HasColumnName("Населеный пункт")
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.Diameter)
                .HasColumnName("Диаметр скважины")
                .IsRequired()
                .HasMaxLength(4);
                entity.Property(e => e.PricePerMeter)
                .HasColumnName("Цена за метр")
                .IsRequired()
                .HasMaxLength(4);
                entity.Property(e => e.Pump)
                .HasColumnName("Насос с монтажом")
                .IsRequired()
                .HasMaxLength(5);
                entity.Property(e => e.Arrangement)
                .HasColumnName("Обустройство")
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.Info)
                .HasColumnName("Дополнительная информация")
                .IsRequired()
                .HasMaxLength(200);
                entity.Property(e => e.CreationTimeData)
                .HasColumnName("Дата создания")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.WorkDate)
                .HasColumnName("Дата работы")
                .IsRequired(false);
            });

            modelBuilder.Entity<BrigadeModelData>(entity =>
            {
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
