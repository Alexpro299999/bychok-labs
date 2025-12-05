using Dormitory.Data.EF.Models;
using System.Linq;

namespace Dormitory.Data.EF
{
    public static class DbInitializer
    {
        public static void Initialize(DormitoryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return;
            }

            var f1 = new Faculty { FacultyName = "ИТиАД", DeanName = "Иванов И.И.", DeanPhone = "123-45-67" };
            var f2 = new Faculty { FacultyName = "Экономический", DeanName = "Петрова А.А.", DeanPhone = "987-65-43" };
            context.Faculties.AddRange(f1, f2);
            context.SaveChanges();

            var c1 = new Curator { FullName = "Смирнов С.С.", Address = "ул. Ленина 1", Phone = "111-222", Position = "Доцент" };
            var c2 = new Curator { FullName = "Сидоров В.В.", Address = "ул. Мира 5", Phone = "333-444", Position = "Старший преподаватель" };
            context.Curators.AddRange(c1, c2);
            context.SaveChanges();
            var students = new Student[]
            {
                new Student { FullName = "Алексеев А.А.", RoomNumber = 101, GroupNum = "И-21", FacultyID = f1.FacultyID, CuratorID = c1.CuratorID },
                new Student { FullName = "Борисов Б.Б.", RoomNumber = 101, GroupNum = "И-21", FacultyID = f1.FacultyID, CuratorID = c1.CuratorID },
                new Student { FullName = "Васильев В.В.", RoomNumber = 102, GroupNum = "Э-15", FacultyID = f2.FacultyID, CuratorID = c2.CuratorID },
                new Student { FullName = "Григорьев Г.Г.", RoomNumber = 102, GroupNum = "Э-15", FacultyID = f2.FacultyID, CuratorID = c1.CuratorID }, 
                new Student { FullName = "Дмитриев Д.Д.", RoomNumber = 103, GroupNum = "И-22", FacultyID = f1.FacultyID, CuratorID = c2.CuratorID }
            };

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}