using Dormitory.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Dormitory.Data.EF.Repositories
{
    public class CuratorRepository
    {
        private readonly DormitoryContext _context;

        public CuratorRepository(DormitoryContext context)
        {
            _context = context;
        }

        public List<Curator> GetAll() => _context.Curators.ToList();

        public void Add(Curator entity)
        {
            _context.Curators.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Curator entity)
        {
            _context.Curators.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Curators.Find(id);
            if (entity != null)
            {
                _context.Curators.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Curator> GetByFaculty(string facultyName)
        {
            return _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Curator)
                .Where(s => s.Faculty.FacultyName == facultyName)
                .Select(s => s.Curator)
                .Distinct()
                .ToList();
        }
    }
}