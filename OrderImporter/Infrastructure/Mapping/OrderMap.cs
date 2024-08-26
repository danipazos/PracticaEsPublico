using CsvHelper.Configuration;
using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Mapping
{
    public sealed class OrderClassMap : ClassMap<Order>
    {
        public OrderClassMap()
        {
            Map(m => m.OrderId).Name("ID");
            Map(m => m.Uuid).Name("UUID");
            Map(m => m.Priority).Name("Prioirdad");
            Map(m => m.Date).Name("Fecha").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Region).Name("Region");
            Map(m => m.Country).Name("Pais");
            Map(m => m.ItemType).Name("Tipo");
            Map(m => m.SalesChannel).Name("Canal de venta");
            Map(m => m.ShipDate).Name("Fecha de envío").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.UnitsSold).Name("Unidades vendidas");
            Map(m => m.UnitPrice).Name("Precio unidad");
            Map(m => m.UnitCost).Name("Coste unidad");
            Map(m => m.TotalRevenue).Name("Ingresos totales");
            Map(m => m.TotalCost).Name("Coste total");
            Map(m => m.TotalProfit).Name("Beneficio total");
            Map(m => m.Errors).Name("Error");
        }
    }
}