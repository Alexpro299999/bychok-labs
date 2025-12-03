using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Dormitory.Data
{
    public class DormitoryRepository
    {
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DormitoryDB;Trusted_Connection=True;";

        public List<StudentReportItem> GetStudentReport()
        {
            var report = new List<StudentReportItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetStudentReport", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new StudentReportItem
                        {
                            StudentCardID = (int)reader["StudentCardID"],
                            FullName = (string)reader["FullName"],
                            FacultyName = (string)reader["FacultyName"],
                            RoomNumber = (int)reader["RoomNumber"],
                            GroupNum = (string)reader["GroupNum"],
                            CuratorName = (string)reader["CuratorName"]
                        });
                    }
                }
            }
            return report;
        }

        public List<Faculty> GetAllFaculties()
        {
            var list = new List<Faculty>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetAllFaculties", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Faculty
                        {
                            FacultyID = (int)reader["FacultyID"],
                            FacultyName = (string)reader["FacultyName"],
                            DeanName = reader["DeanName"] as string,
                            DeanPhone = reader["DeanPhone"] as string
                        });
                    }
                }
            }
            return list;
        }

        public List<Curator> GetAllCurators()
        {
            var list = new List<Curator>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetAllCurators", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Curator
                        {
                            CuratorID = (int)reader["CuratorID"],
                            FullName = (string)reader["FullName"],
                            Address = (string)reader["Address"],
                            Phone = (string)reader["Phone"],
                            Position = (string)reader["Position"]
                        });
                    }
                }
            }
            return list;
        }

        public List<StudentReportItem> FilterStudents(int? roomNumber, string curatorName, string facultyName)
        {
            var report = new List<StudentReportItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_FilterStudents", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@RoomNumber", roomNumber.HasValue ? (object)roomNumber.Value : DBNull.Value);
                command.Parameters.AddWithValue("@CuratorName", string.IsNullOrEmpty(curatorName) ? (object)DBNull.Value : curatorName);
                command.Parameters.AddWithValue("@FacultyName", string.IsNullOrEmpty(facultyName) ? (object)DBNull.Value : facultyName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new StudentReportItem
                        {
                            StudentCardID = (int)reader["StudentCardID"],
                            FullName = (string)reader["FullName"],
                            FacultyName = (string)reader["FacultyName"],
                            RoomNumber = (int)reader["RoomNumber"],
                            GroupNum = (string)reader["GroupNum"],
                            CuratorName = (string)reader["CuratorName"]
                        });
                    }
                }
            }
            return report;
        }

        public List<FacultySummary> GetFacultySummary()
        {
            var summary = new List<FacultySummary>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetFacultySummary", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        summary.Add(new FacultySummary
                        {
                            FacultyName = (string)reader["FacultyName"],
                            CuratorName = (string)reader["CuratorName"],
                            StudentCount = (int)reader["StudentCount"]
                        });
                    }
                }
            }
            return summary;
        }

        public void AddStudent(string fullName, int facultyId, int roomNumber, string groupNum, int curatorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_AddStudent", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@FacultyID", facultyId);
                command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                command.Parameters.AddWithValue("@GroupNum", groupNum);
                command.Parameters.AddWithValue("@CuratorID", curatorId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStudent(int studentId, int roomNumber, string groupNum)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_UpdateStudent", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StudentCardID", studentId);
                command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                command.Parameters.AddWithValue("@GroupNum", groupNum);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_DeleteStudent", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StudentCardID", studentId);
                command.ExecuteNonQuery();
            }
        }
    }
}