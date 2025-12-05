using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dormitory.Data.EF.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        public int RoomNumber { get; set; }
        public string GroupNum { get; set; } = null!;

        public int FacultyID { get; set; }
        [ForeignKey("FacultyID")]
        public virtual Faculty Faculty { get; set; } = null!;

        public int CuratorID { get; set; }
        [ForeignKey("CuratorID")]
        public virtual Curator Curator { get; set; } = null!;

        public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
    }
}