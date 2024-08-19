namespace OrderImporter.Application.OrderImport
{
    internal interface IImportOrdersService
    {
        Task ImportOrdersAsync();
    }
}