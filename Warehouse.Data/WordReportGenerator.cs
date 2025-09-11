using System.Collections.Generic;
using System.Linq;
using Warehouse.Data;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Warehouse.ConsoleApp
{
    public static class WordReportGenerator
    {
        public static void CreateFullStockReport(List<StockReportItem> data, string queryDescription)
        {
            string fileName = "FullStockReport.docx";
            using (var document = DocX.Create(fileName))
            {
                document.InsertParagraph("Отчет по запасам на складах").Bold().FontSize(16).Alignment = Alignment.center;
                document.InsertParagraph(queryDescription).FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph($"Дата: {System.DateTime.Now:dd.MM.yyyy HH:mm}");
                document.InsertParagraph();

                var table = document.AddTable(data.Count + 1, 5);
                table.Alignment = Alignment.center;
                table.Design = TableDesign.TableGrid;

                table.Rows[0].Cells[0].Paragraphs.First().Append("ID").Bold();
                table.Rows[0].Cells[1].Paragraphs.First().Append("Продукт").Bold();
                table.Rows[0].Cells[2].Paragraphs.First().Append("Категория").Bold();
                table.Rows[0].Cells[3].Paragraphs.First().Append("Склад").Bold();
                table.Rows[0].Cells[4].Paragraphs.First().Append("Количество").Bold();

                for (int i = 0; i < data.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Paragraphs.First().Append(data[i].StockID.ToString());
                    table.Rows[i + 1].Cells[1].Paragraphs.First().Append(data[i].ProductName);
                    table.Rows[i + 1].Cells[2].Paragraphs.First().Append(data[i].Category);
                    table.Rows[i + 1].Cells[3].Paragraphs.First().Append(data[i].WarehouseLocation);
                    table.Rows[i + 1].Cells[4].Paragraphs.First().Append(data[i].Quantity.ToString());
                }
                document.InsertTable(table);
                document.Save();
            }
            System.Console.WriteLine($"\nОтчет '{fileName}' успешно создан.");
        }

        public static void CreateGroupedStockReport(List<WarehouseStockSummary> data)
        {
            string fileName = "GroupedStockReport.docx";
            using (var document = DocX.Create(fileName))
            {
                document.InsertParagraph("Сводный отчет по количеству товаров на складах").Bold().FontSize(16).Alignment = Alignment.center;
                document.InsertParagraph($"Дата: {System.DateTime.Now:dd.MM.yyyy HH:mm}");
                document.InsertParagraph();

                var table = document.AddTable(data.Count + 1, 2);
                table.Alignment = Alignment.center;
                table.Design = TableDesign.TableGrid;

                table.Rows[0].Cells[0].Paragraphs.First().Append("Склад").Bold();
                table.Rows[0].Cells[1].Paragraphs.First().Append("Общее количество товаров").Bold();

                for (int i = 0; i < data.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Paragraphs.First().Append(data[i].WarehouseLocation);
                    table.Rows[i + 1].Cells[1].Paragraphs.First().Append(data[i].TotalQuantity.ToString());
                }

                document.InsertTable(table);
                document.InsertParagraph();

                int totalItems = data.Sum(d => d.TotalQuantity);
                document.InsertParagraph($"Итого по всем складам: {totalItems} шт.").Bold().Alignment = Alignment.right;

                document.Save();
            }
            System.Console.WriteLine($"\nОтчет '{fileName}' успешно создан.");
        }
    }
}