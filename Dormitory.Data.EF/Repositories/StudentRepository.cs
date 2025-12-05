using Dormitory.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Dormitory.Data.EF.Repositories
{
    public class StudentRepository
    {
        private readonly DormitoryContext _context;

        public StudentRepository(DormitoryContext context)
        {
            _context = context;
        }

        public List<StudentReportItem> GetReport()
        {
            return _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Curator)
                .Select(s => new StudentReportItem
                {
                    StudentCardID = s.StudentID,
                    FullName = s.FullName,
                    FacultyName = s.Faculty.FacultyName,
                    RoomNumber = s.RoomNumber,
                    GroupNum = s.GroupNum,
                    CuratorName = s.Curator.FullName
                })
                .ToList();
        }

        public void Add(Student s) { _context.Students.Add(s); _context.SaveChanges(); }

        public void Update(Student s) { _context.Students.Update(s); _context.SaveChanges(); }

        public void Delete(int id)
        {
            var s = _context.Students.Find(id);
            if (s != null) { _context.Students.Remove(s); _context.SaveChanges(); }
        }

        public List<StudentReportItem> Filter(int? room, string? cur, string? fac)
        {
            var query = _context.Students.AsQueryable();

            if (room.HasValue) query = query.Where(s => s.RoomNumber == room.Value);
            if (!string.IsNullOrEmpty(cur)) query = query.Where(s => s.Curator.FullName.Contains(cur));
            if (!string.IsNullOrEmpty(fac)) query = query.Where(s => s.Faculty.FacultyName.Contains(fac));

            return query.Select(s => new StudentReportItem
            {
                StudentCardID = s.StudentID,
                FullName = s.FullName,
                FacultyName = s.Faculty.FacultyName,
                RoomNumber = s.RoomNumber,
                GroupNum = s.GroupNum,
                CuratorName = s.Curator.FullName
            }).ToList();
        }

        public object GetRoomStats()
        {
            return _context.Students
                .GroupBy(s => s.RoomNumber)
                .Select(g => new { Room = g.Key, Count = g.Count() })
                .ToList();
        }
    }
}