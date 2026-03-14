using Invoyz.Invoices.Domain.Dtos.InvoiceLines;

namespace Invoyz.Invoices.Domain.Entities
{
    public class InvoiceLine : BaseEntity<InvoiceLineReadDto, InvoiceLineWriteDto>
    {
        public required Guid InvoiceId { get; set; }
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TaxRate { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
        public decimal LineTax => LineTotal * (TaxRate / 100);
        public required Invoice Invoice { get; set; }
        public required Product Product { get; set; }

        public override InvoiceLineReadDto ToReadDto()
        {
            return new InvoiceLineReadDto
            {
                Id = Id,
                InvoiceId = InvoiceId,
                ProductId = ProductId,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                TaxRate = TaxRate,
                LineTotal = LineTotal,
                LineTax = LineTax,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Product = Product?.ToReadDto()
            };
        }

        public override void UpdateFromWriteDto(InvoiceLineWriteDto dto)
        {
            ProductId = dto.ProductId;
            Quantity = dto.Quantity;
            UnitPrice = dto.UnitPrice;
            TaxRate = dto.TaxRate;
        }

        public static InvoiceLine FromWriteDto(InvoiceLineWriteDto dto, Guid invoiceId)
        {
            return new InvoiceLine
            {
                InvoiceId = invoiceId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                TaxRate = dto.TaxRate,
                Invoice = null!,
                Product = null!
            };
        }
    }
}
