namespace OrderImporter.Domain.Models
{
    internal class OrderModel
    {
        internal long Id { get; init; }
        internal string Uuid { get; init; }
        internal string Priority { get; init; }
        internal DateTime? Date { get; init; }
        internal string Region { get; init; }
        internal string Country { get; init; }
        internal string ItemType { get; init; }
        internal string SalesChannel { get; init; }
        internal DateTime? ShipDate { get; init; }
        internal UnitDetails Units { get; init; }
        internal TotalDetails Totals { get; init; }

        private OrderModel() { }
                
        internal static OrderModel Create(OrderDTO orderResponse)
        {
            return new OrderModel
            {
                Id = long.Parse(orderResponse.Id ?? "0"),
                Uuid = orderResponse.Uuid,
                Priority = orderResponse.Priority,
                Date = orderResponse.Date,
                Region = orderResponse.Region,
                Country = orderResponse.Country,
                ItemType = orderResponse.ItemType,
                SalesChannel = orderResponse.SalesChannel,
                ShipDate = orderResponse.ShipDate,
                Units = new UnitDetails(orderResponse.UnitsSold, orderResponse.UnitPrice, orderResponse.UnitCost),
                Totals = new TotalDetails(orderResponse.TotalRevenue, orderResponse.TotalCost, orderResponse.TotalProfit)
            };
        }
    }

    internal record UnitDetails(int Sold, decimal Price, decimal Cost);

    internal record TotalDetails(decimal Revenue, decimal Cost, decimal Profit);
}
