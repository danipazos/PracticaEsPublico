using CsvHelper.Configuration;
using OrderImporter.Domain.Models;

namespace OrderImporter.Infrastructure.Mapping
{
    internal class OrderTotalClassMap : ClassMap<OrderTotal>
    {
        public OrderTotalClassMap()
        {
            Map(m => m.Property).Name("Propiedad");
            Map(m => m.Value).Name("Valor");
            Map(m => m.Total).Name("Pedidos totales");
        }
    }
}