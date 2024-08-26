using Newtonsoft.Json;
using OrderImporter.Common.Helpers;

namespace OrderImporter.Domain.Models
{
    public sealed class OrderApiResponse
    {
        public List<OrderDTO> Content { get; set; }
        public Links Links { get; set; }
    }

    public sealed class Links
    {
        public string Next { get; set; }
    }

    public sealed class OrderDTO
    {
        public string Uuid { get; set; }

        public string Id { get; set; }

        public string Region { get; set; }
        public string Country { get; set; }

        [JsonProperty("item_type")]
        public string ItemType { get; set; }

        [JsonProperty("sales_channel")]
        public string SalesChannel { get; set; }

        public string Priority { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty("ship_date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime ShipDate { get; set; }

        [JsonProperty("units_sold")]
        public int UnitsSold { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("unit_cost")]
        public decimal UnitCost { get; set; }

        [JsonProperty("total_revenue")]
        public decimal TotalRevenue { get; set; }

        [JsonProperty("total_cost")]
        public decimal TotalCost { get; set; }

        [JsonProperty("total_profit")]
        public decimal TotalProfit { get; set; }

        [JsonIgnore]
        public Links Links { get; set; }
    }
}
