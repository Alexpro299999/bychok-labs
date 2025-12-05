using System;
using System.Collections.Generic;
using System.Linq;
using Dormitory.Data;

namespace Dormitory.ConsoleApp
{
    class Program
    {
        private static readonly DormitoryRepository _repository = new DormitoryRepository();
        private static List<StudentReportItem> _lastQueryResult = new List<StudentReportItem>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Управление Общежитием (Вариант 16) =====");
                DisplayStudents();

                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Заселить студента (Добавить)");
                Console.WriteLine("2. Изменить данные студента (Полное редактирование)");
                Console.WriteLine("3. Выселить студента (Удалить)");
                Console.WriteLine("4. Поиск и фильтрация");
                Console.WriteLine("5. Сортировка");
                Console.WriteLine("6. Создать отчеты в Word");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": AddStudent(); break;
                    case "2": UpdateStudent(); break;
                    case "3": DeleteStudent(); break;
                    case "4": FilterStudents(); break;
                    case "5": SortStudents(); break;
                    case "6": GenerateReports(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void DisplayStudents(List<StudentReportItem> list = null)
        {
            if (list == null)
            {
                _lastQueryResult = _repository.GetStudentReport();
                list = _lastQueryResult;
            }

            Console.WriteLine("\n--- Список студентов ---");
            Console.WriteLine("{0,-5} {1,-25} {2,-20} {3,-10} {4,-10} {5,-20}", "ID", "ФИО", "Факультет", "Комн.", "Группа", "Куратор");
            Console.WriteLine(new string('-', 100));
            foreach (var item in list)
            {
                Console.WriteLine("{0,-5} {1,-25} {2,-20} {3,-10} {4,-10} {5,-20}",
                    item.StudentCardID, item.FullName, item.FacultyName, item.RoomNumber, item.GroupNum, item.CuratorName);
            }
        }

        static int GetValidIntInput(string message)
        {
            int result;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Ошибка: Введите корректное число.");
                Console.Write(message);
            }
            return result;
        }

        static void AddStudent()
        {
            try
            {
                Console.Write("\nВведите ФИО студента: ");
                string fullName = Console.ReadLine();

                var faculties = _repository.GetAllFaculties();
                Console.WriteLine("\nВыберите факультет:");
                faculties.ForEach(f => Console.WriteLine($"  {f.FacultyID}. {f.FacultyName}"));
                int facultyId = GetValidIntInput("ID факультета: ");

                int room = GetValidIntInput("Номер комнаты: ");

                Console.Write("Группа: ");
                string group = Console.ReadLine();

                var curators = _repository.GetAllCurators();
                Console.WriteLine("\nВыберите куратора:");
                curators.ForEach(c => Console.WriteLine($"  {c.CuratorID}. {c.FullName}"));
                int curatorId = GetValidIntInput("ID куратора: ");

                _repository.AddStudent(fullName, facultyId, room, group, curatorId);
                Console.WriteLine("Студент успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void UpdateStudent()
        {
            try
            {
                int id = GetValidIntInput("\nВведите ID студента для изменения: ");

                Console.Write("Новое ФИО: ");
                string fullName = Console.ReadLine();

                var faculties = _repository.GetAllFaculties();
                Console.WriteLine("\nВыберите новый факультет:");
                faculties.ForEach(f => Console.WriteLine($"  {f.FacultyID}. {f.FacultyName}"));
                int facultyId = GetValidIntInput("ID факультета: ");

                int room = GetValidIntInput("Новый номер комнаты: ");

                Console.Write("Новая группа: ");
                string group = Console.ReadLine();

                var curators = _repository.GetAllCurators();
                Console.WriteLine("\nВыберите нового куратора:");
                curators.ForEach(c => Console.WriteLine($"  {c.CuratorID}. {c.FullName}"));
                int curatorId = GetValidIntInput("ID куратора: ");

                _repository.UpdateStudent(id, fullName, facultyId, room, group, curatorId);
                Console.WriteLine("Данные обновлены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void DeleteStudent()
        {
            try
            {
                int id = GetValidIntInput("\nВведите ID студента для удаления: ");
                Console.Write($"Вы уверены? (y/n): ");
                if (Console.ReadLine().Trim().ToLower() == "y")
                {
                    _repository.DeleteStudent(id);
                    Console.WriteLine("Студент выселен.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void FilterStudents()
        {
            try
            {
                Console.WriteLine("\n--- Фильтрация ---");
                Console.WriteLine("Оставьте поле пустым, чтобы не использовать фильтр.");

                Console.Write("Номер комнаты (число): ");
                string roomStr = Console.ReadLine();
                int? room = string.IsNullOrEmpty(roomStr) ? (int?)null : int.Parse(roomStr);

                Console.Write("Имя куратора (частично): ");
                string curator = Console.ReadLine();

                Console.Write("Название факультета: ");
                string faculty = Console.ReadLine();

                _lastQueryResult = _repository.FilterStudents(room, curator, faculty);
                if (_lastQueryResult.Any())
                    DisplayStudents(_lastQueryResult);
                else
                    Console.WriteLine("\nНичего не найдено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        static void SortStudents()
        {
            Console.WriteLine("\n--- Сортировка ---");
            Console.WriteLine("1. По ФИО");
            Console.WriteLine("2. По Номеру комнаты");
            Console.WriteLine("3. По Факультету");
            Console.Write("Выбор: ");

            switch (Console.ReadLine())
            {
                case "1": _lastQueryResult = _lastQueryResult.OrderBy(x => x.FullName).ToList(); break;
                case "2": _lastQueryResult = _lastQueryResult.OrderBy(x => x.RoomNumber).ToList(); break;
                case "3": _lastQueryResult = _lastQueryResult.OrderBy(x => x.FacultyName).ToList(); break;
                default: return;
            }
            DisplayStudents(_lastQueryResult);
        }

        static void GenerateReports()
        {
            Console.WriteLine("\n--- Отчеты ---");
            Console.WriteLine("1. Полный список студентов");
            Console.WriteLine("2. Отчет по текущей выборке (фильтр)");
            Console.WriteLine("3. Группирующий отчет (Факультеты/Кураторы/Количество)");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        var all = _repository.GetStudentReport();
                        WordReportGenerator.CreateFullStudentReport(all, "Полный список студентов");
                        break;
                    case "2":
                        WordReportGenerator.CreateFullStudentReport(_lastQueryResult, "Отчет по выборке");
                        break;
                    case "3":
                        var grouped = _repository.GetFacultySummary();
                        WordReportGenerator.CreateGroupedFacultyReport(grouped);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания отчета: {ex.Message}");
            }
        }
    }
}