namespace Dormitory.Data.EF.Models
{
    public class FacultySummary
    {
        public string FacultyName { get; set; } = string.Empty;
        public string CuratorName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}