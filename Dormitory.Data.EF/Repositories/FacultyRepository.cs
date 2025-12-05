using Dormitory.Data.EF.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dormitory.Data.EF.Repositories
{
    public class FacultyRepository
    {
        private readonly DormitoryContext _context;

        public FacultyRepository(DormitoryContext context) { _context = context; }

        public List<Faculty> GetAll() => _context.Faculties.ToList();
        public void Add(Faculty entity) { _context.Faculties.Add(entity); _context.SaveChanges(); }
        public void Update(Faculty entity) { _context.Faculties.Update(entity); _context.SaveChanges(); }
        public void Delete(int id) { var e = _context.Faculties.Find(id); if (e != null) { _context.Faculties.Remove(e); _context.SaveChanges(); } }

        public List<FacultySummary> GetSummary()
        {

            return _context.Students
               .GroupBy(s => new { s.Faculty.FacultyName, s.Curator.FullName })
               .Select(g => new FacultySummary
               {
                   FacultyName = g.Key.FacultyName,
                   CuratorName = g.Key.FullName,
                   StudentCount = g.Count()
               })
               .ToList();
        }
    }
}