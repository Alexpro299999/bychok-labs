namespace Dormitory.Data.EF.Models
{
    public class StudentReportItem
    {
        public int StudentCardID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string FacultyName { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public string GroupNum { get; set; } = string.Empty;
        public string CuratorName { get; set; } = string.Empty;
    }
}