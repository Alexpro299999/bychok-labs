using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dormitory.Data.EF.Models
{
    public class Curator
    {
        public int CuratorID { get; set; }

        [Required]
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
