namespace OrderImporter.Domain.Models
{
    public sealed class OrderModel
    {
        public long Id { get; init; }
        public string Uuid { get; init; }
        public string Priority { get; init; }
        public DateTime? Date { get; init; }
        public string Region { get; init; }
        public string Country { get; init; }
        public string ItemType { get; init; }
        public string SalesChannel { get; init; }
        public DateTime? ShipDate { get; init; }
        public UnitDetails Units { get; init; }
        public TotalDetails Totals { get; init; }

        private OrderModel() { }

        public static OrderModel Create(OrderDTO orderResponse)
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

    public record UnitDetails(int Sold, decimal Price, decimal Cost);

    public record TotalDetails(decimal Revenue, decimal Cost, decimal Profit);
}
