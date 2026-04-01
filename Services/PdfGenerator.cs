using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaFarmacia.Models;

namespace SistemaFarmacia.Services
{
    public class PdfGenerator
    {
        public static byte[] GenerarReporte(string titulo, List<Medicamento> data)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header().Text(titulo)
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.ConstantColumn(100);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(90);
                        });

                        // Encabezados
                        table.Header(header =>
                        {
                            header.Cell().Text("Nombre").Bold();
                            header.Cell().Text("Categoría").Bold();
                            header.Cell().Text("Stock").Bold();
                            header.Cell().Text("Precio").Bold();
                            header.Cell().Text("Vence").Bold();
                        });

                        // Filas
                        foreach (var d in data)
                        {
                            table.Cell().Text(d.Nombre);
                            table.Cell().Text(d.Categoria?.Nombre ?? "");
                            table.Cell().Text(d.Stock.ToString());
                            table.Cell().Text(d.Precio.ToString("0.00"));
                            table.Cell().Text(d.FechaVencimiento.ToShortDateString());
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Reporte generado por SistemaFarmacia");
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}