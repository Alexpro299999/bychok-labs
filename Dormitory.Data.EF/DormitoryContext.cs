using Dormitory.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dormitory.Data.EF
{
    public class DormitoryContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Curator> Curators { get; set; }
        public DbSet<Club> Clubs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=DormitoryEFDB;Trusted_Connection=True;");
        }
    }
}