using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Warehouse.Data
{
    public class StockRepository
    {
        
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=WarehouseDB;Trusted_Connection=True;";

        #region Методы чтения данных (SELECT)

        /// <summary>
        /// Получает полный отчет о запасах на всех складах.
        /// </summary>
        public List<StockReportItem> GetStockReport()
        {
            var report = new List<StockReportItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetStockReport", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new StockReportItem
                        {
                            StockID = (int)reader["StockID"],
                            ProductName = (string)reader["ProductName"],
                            WarehouseLocation = (string)reader["WarehouseLocation"],
                            Quantity = (int)reader["Quantity"],
                            Category = (string)reader["Category"]
                        });
                    }
                }
            }
            return report;
        }

        /// <summary>
        /// Получает список всех продуктов для использования в выпадающих списках.
        /// </summary>
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetAllProducts", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID = (int)reader["ProductID"],
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }
            return products;
        }

        /// <summary>
        /// Получает список всех складов для использования в выпадающих списках.
        /// </summary>
        public List<Warehouse> GetAllWarehouses()
        {
            var warehouses = new List<Warehouse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetAllWarehouses", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        warehouses.Add(new Warehouse
                        {
                            WarehouseID = (int)reader["WarehouseID"],
                            Location = (string)reader["Location"]
                        });
                    }
                }
            }
            return warehouses;
        }

        /// <summary>
        /// Фильтрует запасы по названию продукта и/или расположению склада.
        /// </summary>
        public List<StockReportItem> FilterStock(string productName, string warehouseLocation)
        {
            var report = new List<StockReportItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_FilterStock", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProductName", string.IsNullOrEmpty(productName) ? (object)DBNull.Value : productName);
                command.Parameters.AddWithValue("@WarehouseLocation", string.IsNullOrEmpty(warehouseLocation) ? (object)DBNull.Value : warehouseLocation);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new StockReportItem
                        {
                            StockID = (int)reader["StockID"],
                            ProductName = (string)reader["ProductName"],
                            WarehouseLocation = (string)reader["WarehouseLocation"],
                            Quantity = (int)reader["Quantity"],
                            Category = (string)reader["Category"]
                        });
                    }
                }
            }
            return report;
        }

        /// <summary>
        /// Получает отчет, сгруппированный по складам, с подсчетом общего количества.
        /// </summary>
        public List<WarehouseStockSummary> GetStockGroupedByWarehouse()
        {
            var summary = new List<WarehouseStockSummary>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_GetStockGroupedByWarehouse", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        summary.Add(new WarehouseStockSummary
                        {
                            WarehouseLocation = (string)reader["WarehouseLocation"],
                            TotalQuantity = (int)reader["TotalQuantity"]
                        });
                    }
                }
            }
            return summary;
        }


        #endregion

        #region Методы изменения данных (INSERT, UPDATE, DELETE)

        /// <summary>
        /// Добавляет новую запись о запасах в таблицу Stock.
        /// </summary>
        public void AddStockItem(int productId, int warehouseId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_AddStockItem", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@WarehouseID", warehouseId);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Обновляет количество для существующей записи о запасах.
        /// </summary>
        public void UpdateStockQuantity(int stockId, int newQuantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_UpdateStockQuantity", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StockID", stockId);
                command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Удаляет запись о запасах из таблицы Stock по ее ID.
        /// </summary>
        public void DeleteStockItem(int stockId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("sp_DeleteStockItem", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@StockID", stockId);
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}