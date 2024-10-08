﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderImporter.Infrastructure.Persistence.Entities
{
    public class OrderError
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public virtual Order Order { get; set; }
        public string Error { get; set; }
    }

    public sealed class OrderErrorConfiguration : IEntityTypeConfiguration<OrderError>
    {
        public void Configure(EntityTypeBuilder<OrderError> builder)
        {
            builder.ToTable("OrdersErrors");

            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.OrderId)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(o => o.Error)
                .HasColumnType("varchar");

            builder
                .HasOne(o => o.Order)
                .WithMany(oe => oe.Errors)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}