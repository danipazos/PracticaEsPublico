namespace OrderImporter.Application.OrderImport
{
    public interface IImportOrdersService
    {
        Task ImportOrdersAsync();
    }
}