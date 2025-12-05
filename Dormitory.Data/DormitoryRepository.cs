using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Dormitory.Data
{
    public class DormitoryRepository
    {
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DormitoryDB;Trusted_Connection=True;";


        public DataTable ExecuteQuery(string queryType, string param = null)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = null;

                switch (queryType)
                {
                    case "StudentsInRoom": // Студенты в комнате
                        cmd = new SqlCommand("sp_FilterStudents", conn) { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.AddWithValue("@RoomNumber", int.TryParse(param, out int r) ? r : DBNull.Value);
                        break;

                    case "StudentsByCurator": // Студенты куратора
                        cmd = new SqlCommand("sp_FilterStudents", conn) { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.AddWithValue("@CuratorName", param);
                        break;

                    case "RoomStats": // Статистика по комнатам (Запрос 3)
                        cmd = new SqlCommand("sp_Query_RoomStats", conn) { CommandType = CommandType.StoredProcedure };
                        break;

                    case "CuratorsByFaculty": // Кураторы факультета (Запрос 4)
                        cmd = new SqlCommand("sp_Query_CuratorsByFaculty", conn) { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.AddWithValue("@FacultyName", param);
                        break;

                    case "FacultySummary": // Итоговый отчет (Запрос 5)
                        cmd = new SqlCommand("sp_GetFacultySummary", conn) { CommandType = CommandType.StoredProcedure };
                        break;
                }

                if (cmd != null)
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }


        public List<StudentReportItem> GetStudentReport()
        {
            var list = new List<StudentReportItem>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetStudentReport", conn) { CommandType = CommandType.StoredProcedure };
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new StudentReportItem
                        {
                            StudentCardID = (int)r["StudentCardID"],
                            FullName = (string)r["FullName"],
                            FacultyName = (string)r["FacultyName"],
                            RoomNumber = (int)r["RoomNumber"],
                            GroupNum = (string)r["GroupNum"],
                            CuratorName = (string)r["CuratorName"]
                        });
                    }
                }
            }
            return list;
        }

        public void AddStudent(string fio, int facId, int room, string grp, int curId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_AddStudent", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@FullName", fio);
                cmd.Parameters.AddWithValue("@FacultyID", facId);
                cmd.Parameters.AddWithValue("@RoomNumber", room);
                cmd.Parameters.AddWithValue("@GroupNum", grp);
                cmd.Parameters.AddWithValue("@CuratorID", curId);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStudent(int id, string fio, int facId, int room, string grp, int curId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_UpdateStudent", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@StudentCardID", id);
                cmd.Parameters.AddWithValue("@FullName", fio);
                cmd.Parameters.AddWithValue("@FacultyID", facId);
                cmd.Parameters.AddWithValue("@RoomNumber", room);
                cmd.Parameters.AddWithValue("@GroupNum", grp);
                cmd.Parameters.AddWithValue("@CuratorID", curId);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStudent(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_DeleteStudent", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@StudentCardID", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<StudentReportItem> FilterStudents(int? room, string cur, string fac)
        {
            var list = new List<StudentReportItem>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_FilterStudents", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@RoomNumber", room.HasValue ? (object)room.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@CuratorName", string.IsNullOrEmpty(cur) ? (object)DBNull.Value : cur);
                cmd.Parameters.AddWithValue("@FacultyName", string.IsNullOrEmpty(fac) ? (object)DBNull.Value : fac);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new StudentReportItem
                        {
                            StudentCardID = (int)r["StudentCardID"],
                            FullName = (string)r["FullName"],
                            FacultyName = (string)r["FacultyName"],
                            RoomNumber = (int)r["RoomNumber"],
                            GroupNum = (string)r["GroupNum"],
                            CuratorName = (string)r["CuratorName"]
                        });
                    }
                }
            }
            return list;
        }

        public List<Faculty> GetAllFaculties()
        {
            var list = new List<Faculty>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetAllFaculties", conn) { CommandType = CommandType.StoredProcedure };
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Faculty
                        {
                            FacultyID = (int)r["FacultyID"],
                            FacultyName = (string)r["FacultyName"],
                            DeanName = r["DeanName"] as string,
                            DeanPhone = r["DeanPhone"] as string
                        });
                    }
                }
            }
            return list;
        }

        public void AddFaculty(string name, string dean, string phone)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_AddFaculty", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@FacultyName", name);
                cmd.Parameters.AddWithValue("@DeanName", dean);
                cmd.Parameters.AddWithValue("@DeanPhone", phone);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateFaculty(int id, string name, string dean, string phone)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_UpdateFaculty", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@FacultyID", id);
                cmd.Parameters.AddWithValue("@FacultyName", name);
                cmd.Parameters.AddWithValue("@DeanName", dean);
                cmd.Parameters.AddWithValue("@DeanPhone", phone);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFaculty(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_DeleteFaculty", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@FacultyID", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Curator> GetAllCurators()
        {
            var list = new List<Curator>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetAllCurators", conn) { CommandType = CommandType.StoredProcedure };
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Curator
                        {
                            CuratorID = (int)r["CuratorID"],
                            FullName = (string)r["FullName"],
                            Address = r["Address"] as string,
                            Phone = r["Phone"] as string,
                            Position = r["Position"] as string
                        });
                    }
                }
            }
            return list;
        }

        public void AddCurator(string name, string addr, string phone, string pos)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_AddCurator", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@FullName", name);
                cmd.Parameters.AddWithValue("@Address", addr);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Position", pos);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCurator(int id, string name, string addr, string phone, string pos)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_UpdateCurator", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@CuratorID", id);
                cmd.Parameters.AddWithValue("@FullName", name);
                cmd.Parameters.AddWithValue("@Address", addr);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Position", pos);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCurator(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_DeleteCurator", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@CuratorID", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<FacultySummary> GetFacultySummary()
        {
            var list = new List<FacultySummary>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetFacultySummary", conn) { CommandType = CommandType.StoredProcedure };
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new FacultySummary
                        {
                            FacultyName = (string)r["FacultyName"],
                            CuratorName = (string)r["CuratorName"],
                            StudentCount = (int)r["StudentCount"]
                        });
                    }
                }
            }
            return list;
        }
    }
}