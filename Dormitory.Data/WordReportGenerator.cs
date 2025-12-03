using System.Collections.Generic;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Dormitory.Data
{
    public static class WordReportGenerator
    {
        public static void CreateFullStudentReport(List<StudentReportItem> data, string queryDescription)
        {
            string fileName = "StudentReport.docx";
            using (var document = DocX.Create(fileName))
            {
                document.InsertParagraph("Отчет по студентам общежития").Bold().FontSize(16).Alignment = Alignment.center;
                document.InsertParagraph(queryDescription).FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph($"Дата: {System.DateTime.Now:dd.MM.yyyy HH:mm}");
                document.InsertParagraph();

                var table = document.AddTable(data.Count + 1, 6);
                table.Alignment = Alignment.center;
                table.Design = TableDesign.TableGrid;

                string[] headers = { "ID", "ФИО", "Факультет", "Комната", "Группа", "Куратор" };
                for (int i = 0; i < headers.Length; i++)
                {
                    table.Rows[0].Cells[i].Paragraphs.First().Append(headers[i]).Bold();
                }

                for (int i = 0; i < data.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Paragraphs.First().Append(data[i].StudentCardID.ToString());
                    table.Rows[i + 1].Cells[1].Paragraphs.First().Append(data[i].FullName);
                    table.Rows[i + 1].Cells[2].Paragraphs.First().Append(data[i].FacultyName);
                    table.Rows[i + 1].Cells[3].Paragraphs.First().Append(data[i].RoomNumber.ToString());
                    table.Rows[i + 1].Cells[4].Paragraphs.First().Append(data[i].GroupNum);
                    table.Rows[i + 1].Cells[5].Paragraphs.First().Append(data[i].CuratorName);
                }
                document.InsertTable(table);
                document.Save();
            }
            System.Console.WriteLine($"\nОтчет '{fileName}' успешно создан.");
        }

        public static void CreateGroupedFacultyReport(List<FacultySummary> data)
        {
            string fileName = "FacultyGroupedReport.docx";
            using (var document = DocX.Create(fileName))
            {
                document.InsertParagraph("Сводный отчет по факультетам и кураторам").Bold().FontSize(16).Alignment = Alignment.center;
                document.InsertParagraph($"Дата: {System.DateTime.Now:dd.MM.yyyy HH:mm}");
                document.InsertParagraph();

                var groupedByFaculty = data.GroupBy(x => x.FacultyName);
                int grandTotal = 0;

                foreach (var facultyGroup in groupedByFaculty)
                {
                    var p = document.InsertParagraph($"Факультет: {facultyGroup.Key}");
                    p.Bold().FontSize(14).Color(Xceed.Drawing.Color.Blue);
                    var table = document.AddTable(facultyGroup.Count() + 1, 2);
                    table.Design = TableDesign.TableGrid;
                    table.Rows[0].Cells[0].Paragraphs.First().Append("Куратор").Bold();
                    table.Rows[0].Cells[1].Paragraphs.First().Append("Кол-во студентов").Bold();

                    int facultyTotal = 0;
                    int row = 1;
                    foreach (var item in facultyGroup)
                    {
                        table.Rows[row].Cells[0].Paragraphs.First().Append(item.CuratorName);
                        table.Rows[row].Cells[1].Paragraphs.First().Append(item.StudentCount.ToString());
                        facultyTotal += item.StudentCount;
                        row++;
                    }

                    document.InsertTable(table);
                    document.InsertParagraph($"Итого по факультету: {facultyTotal}").Bold().Italic().Alignment = Alignment.right;
                    document.InsertParagraph();
                    grandTotal += facultyTotal;
                }

                document.InsertParagraph(new string('-', 50));
                document.InsertParagraph($"ВСЕГО СТУДЕНТОВ В ОБЩЕЖИТИИ: {grandTotal}").Bold().FontSize(12).Alignment = Alignment.right;

                document.Save();
            }
            System.Console.WriteLine($"\nОтчет '{fileName}' успешно создан.");
        }
    }
}