namespace OrderImporter.Common.Exceptions
{
    //Para la finalidad de esta practica creamos una sola excepción personalizada pero podríamos
    //crear tantas como fuesen necesarias en las distintas partes del código.
    internal class OrderImporterException : Exception
    {
        public OrderImporterException(string message) : base(message) { }
    }
}
