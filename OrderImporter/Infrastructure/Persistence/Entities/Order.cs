using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderImporter.Infrastructure.Persistence.Entities
{
    public class Order
    {
        public long Id { get; init; }
        public long OrderId { get; init; }
        public string Uuid { get; init; }
        public string Priority { get; init; }
        public DateTime? Date { get; init; }
        public string Region { get; init; }
        public string Country { get; init; }
        public string ItemType { get; init; }
        public string SalesChannel { get; init; }
        public DateTime? ShipDate { get; init; }
        public int UnitsSold { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal UnitCost { get; init; }
        public decimal TotalRevenue { get; init; }
        public decimal TotalCost { get; init; }
        public decimal TotalProfit { get; init; }

        public virtual ICollection<OrderError> Errors { get; set; }

        public static Order FromModel(Domain.Models.OrderModel order)
        {
            return new Order
            {
                OrderId = order.Id,
                Uuid = order.Uuid,
                Priority = order.Priority,
                Date = order.Date,
                Region = order.Region,
                Country = order.Country,
                ItemType = order.ItemType,
                SalesChannel = order.SalesChannel,
                ShipDate = order.ShipDate,
                UnitsSold = order.Units.Sold,
                UnitPrice = order.Units.Price,
                UnitCost = order.Units.Cost,
                TotalRevenue = order.Totals.Revenue,
                TotalCost = order.Totals.Cost,
                TotalProfit = order.Totals.Profit
            };
        }

        public object GetTypeValue(string property)
        {
            var propertyInfo = GetType().GetProperty(property);

            return propertyInfo?.GetValue(this) ?? null;
        }    
    }

    public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {            
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();
            
            builder.Property(o => o.OrderId)
              .IsRequired()
              .HasColumnType("int");

            builder.Property(o => o.Uuid)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(o => o.Priority)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(1); ;

            builder.Property(o => o.Date)
                .HasColumnType("date");

            builder.Property(o => o.Region)
                .HasColumnType("varchar")
                .HasMaxLength(255);

            builder.Property(o => o.Country)
                .HasColumnType("varchar")
                .HasMaxLength(255);

            builder.Property(o => o.ItemType)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(o => o.SalesChannel)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(o => o.ShipDate)
                .HasColumnType("date");

            builder.Property(o => o.UnitsSold)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.UnitCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.TotalRevenue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.TotalCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.TotalProfit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasMany(oe => oe.Errors)
                .WithOne(o => o.Order)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.Uuid).IsUnique();
        }
    }
}
