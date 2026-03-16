using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Services;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Invoyz.Invoices.Domain.Services
{
    public class PdfGeneratorService() : IPdfGeneratorService
    {
        public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice)
        {
            Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Invoice #{invoice.InvoiceNumber}")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Text(text =>
                            {
                                text.Span("Customer: ").SemiBold();
                                text.Span(invoice.Customer?.Name ?? "N/A");
                            });

                            if (!string.IsNullOrEmpty(invoice.Customer?.Email))
                            {
                                column.Item().Text(text =>
                                {
                                    text.Span("Email: ").SemiBold();
                                    text.Span(invoice.Customer.Email);
                                });
                            }

                            column.Item().Text(text =>
                            {
                                text.Span("Issue Date: ").SemiBold();
                                text.Span(invoice.IssueDate.ToString("dd/MM/yyyy"));
                            });

                            column.Item().Text(text =>
                            {
                                text.Span("Due Date: ").SemiBold();
                                text.Span(invoice.DueDate.ToString("dd/MM/yyyy"));
                            });

                            column.Item().Text(text =>
                            {
                                text.Span("Status: ").SemiBold();
                                text.Span(invoice.Status.ToString());
                            });

                            column.Item().PaddingTop(20).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Product").SemiBold();
                                    header.Cell().Element(CellStyle).Text("Description").SemiBold();
                                    header.Cell().Element(CellStyle).AlignRight().Text("Qty").SemiBold();
                                    header.Cell().Element(CellStyle).AlignRight().Text("Price").SemiBold();
                                    header.Cell().Element(CellStyle).AlignRight().Text("Total").SemiBold();

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                    }
                                });

                                if (invoice.Lines != null)
                                {
                                    foreach (var line in invoice.Lines)
                                    {
                                        table.Cell().Element(CellStyle).Text(line.Product?.Name ?? "N/A");
                                        table.Cell().Element(CellStyle).Text(line.Product?.Description ?? "N/A");
                                        table.Cell().Element(CellStyle).AlignRight().Text(line.Quantity.ToString("N2"));
                                        table.Cell().Element(CellStyle).AlignRight().Text(line.UnitPrice.ToString("C2"));
                                        table.Cell().Element(CellStyle).AlignRight().Text(line.LineTotal.ToString("C2"));
                                    }
                                }

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(5);
                                }
                            });

                            column.Item().PaddingTop(20).AlignRight().Column(totalColumn =>
                            {
                                totalColumn.Item().Text(text =>
                                {
                                    text.Span("Subtotal: ").SemiBold();
                                    text.Span(invoice.SubTotal.ToString("C2"));
                                });

                                totalColumn.Item().Text(text =>
                                {
                                    text.Span("Tax: ").SemiBold();
                                    text.Span(invoice.TotalTax.ToString("C2"));
                                });

                                totalColumn.Item().Text(text =>
                                {
                                    text.Span("Grand Total: ").SemiBold().FontSize(14);
                                    text.Span(invoice.GrandTotal.ToString("C2")).FontSize(14);
                                });
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated on ");
                            x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
