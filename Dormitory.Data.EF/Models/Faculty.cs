using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dormitory.Data.EF.Models
{
    public class Faculty
    {
        public int FacultyID { get; set; }

        [Required]
        public string FacultyName { get; set; } = null!;
        public string? DeanName { get; set; }
        public string? DeanPhone { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
