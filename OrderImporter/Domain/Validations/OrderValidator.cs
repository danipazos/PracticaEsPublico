using FluentValidation;
using OrderImporter.Domain.Models;

namespace OrderImporter.Domain.Validations
{
    internal class OrderValidator : AbstractValidator<OrderModel>
    {
        public OrderValidator()
        {
            RuleFor(o => o.Id)
                .NotEmpty()
                .WithMessage("No se ha indicado el Id del pedido.");
            RuleFor(o => o.Priority)
                .IsInEnum()
                .WithMessage(o=> $"No se ha indicado una prioridad válida: {o.Priority}");
            RuleFor(o => o.Date)
                .NotEmpty()
                .WithMessage("No se ha indicado la fecha del pedido.");
            RuleFor(o => o.Region)
                .NotEmpty()
                .WithMessage("No se ha indicado la región del pedido.");
            RuleFor(o => o.Country)
                .NotEmpty()
                .WithMessage("No se ha indicado el país del pedido.");
            RuleFor(o => o.ItemType)
                .NotEmpty()
                .WithMessage("No se ha indicado el tipo de item del pedido.");
            RuleFor(o => o.SalesChannel)
                .NotEmpty()
                .WithMessage("No se ha indicado el canal de venta del pedido.");
            RuleFor(o => o.ShipDate)
                .NotEmpty()
                .WithMessage("No se ha indicado la fecha de envío del pedido.");
            RuleFor(o => o.Units)
                .NotNull()
                .WithMessage("No se han indicado las unidades del pedido.");
            RuleFor(o => o.Totals)
                .NotNull()
                .WithMessage("No se han indicado los totales del pedido.");

            When(o => o.Units != null, () =>
            {
                RuleFor(o => o.Units.Sold)
                    .GreaterThan(0)
                    .WithMessage("Las unidades vendidas deben ser superiores a cero.");
                RuleFor(o => o.Units.Price)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("El precio por unidad debe ser superior a cero.");
                RuleFor(o => o.Units.Cost)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("El precio de coste debe ser superior a cero.");
            });

            When(o => o.Units != null && o.Totals != null, () =>
            {
                RuleFor(o => o.Totals.Revenue)
                    .Must((order, revenue) =>
                    {                     
                        return revenue == CalculateRevenue(order.Units);
                    })
                    .WithMessage(order => $"El ingreso total no coincide con el cálculo. Valor actual: {order.Totals.Revenue}, Valor esperado: { CalculateRevenue(order.Units) }");

                RuleFor(o => o.Totals.Cost)
                    .Must((order, cost) =>
                    {                        
                        return cost == CalculateCost(order.Units);
                    })
                    .WithMessage(order => $"El coste total no coincide con el cálculo. Valor actual: {order.Totals.Cost}, Valor esperado: {CalculateCost(order.Units)}");

                RuleFor(o => o.Totals.Profit)
                    .Must((order, profit) =>
                    {                        
                        return profit == CalculateProfit(order.Units);
                    })
                    .WithMessage(order =>
                    {
                        return $"El beneficio total no coincide con el indicado. Valor actual: {order.Totals.Profit}, Valor esperado: {CalculateProfit(order.Units)}";
                    });
            });
        }

        private static decimal CalculateRevenue(UnitDetails units) => units.Sold * units.Price;
        private static decimal CalculateCost(UnitDetails units) => units.Sold * units.Cost;
        private static decimal CalculateProfit(UnitDetails units) => CalculateRevenue(units) - CalculateCost(units);

    }
}
