using System.Collections.Generic;

namespace Dormitory.Data.EF.Models
{
    public class Club
    {
        public int ClubID { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}