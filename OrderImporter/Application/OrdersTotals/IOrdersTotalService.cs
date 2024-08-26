namespace OrderImporter.Application.OrdersTotals
{
    internal interface IOrdersTotalService
    {
        Dictionary<string, Dictionary<string, int>> GetTotals();
    }
}