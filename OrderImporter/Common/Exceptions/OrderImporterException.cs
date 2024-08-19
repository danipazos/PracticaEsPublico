namespace OrderImporter.Common.Exceptions
{
    //Para la finalidad de esta practica creamos una sola excepción personalizada pero podríamos
    //crear tantas como fuesen necesarias en las distintas partes del código.
    public sealed class OrderImporterException(string message) : Exception(message)
    {
    }
}
