USE [WarehouseDB];
GO

-- ===== ШАГ 1: УДАЛЕНИЕ СУЩЕСТВУЮЩИХ ОБЪЕКТОВ =====

PRINT 'Удаление существующих процедур и таблиц...';
GO

IF OBJECT_ID('dbo.sp_GetStockGroupedByWarehouse', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetStockGroupedByWarehouse;
IF OBJECT_ID('dbo.sp_FilterStock', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_FilterStock;
IF OBJECT_ID('dbo.sp_DeleteStockItem', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_DeleteStockItem;
IF OBJECT_ID('dbo.sp_UpdateStockQuantity', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_UpdateStockQuantity;
IF OBJECT_ID('dbo.sp_AddStockItem', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_AddStockItem;
IF OBJECT_ID('dbo.sp_GetAllWarehouses', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetAllWarehouses;
IF OBJECT_ID('dbo.sp_GetAllProducts', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetAllProducts;
IF OBJECT_ID('dbo.sp_GetStockReport', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetStockReport;
GO

IF OBJECT_ID('dbo.Stock', 'U') IS NOT NULL DROP TABLE dbo.Stock;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Warehouses', 'U') IS NOT NULL DROP TABLE dbo.Warehouses;
GO


-- ===== ШАГ 2: СОЗДАНИЕ ТАБЛИЦ =====
PRINT 'Создание структуры таблиц...';
GO

CREATE TABLE Warehouses (
    WarehouseID INT PRIMARY KEY IDENTITY(1,1),
    Location NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50)
);
GO

CREATE TABLE Stock (
    StockID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    WarehouseID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID) ON DELETE CASCADE
);
GO

-- ===== ШАГ 3: ЗАПОЛНЕНИЕ НАЧАЛЬНЫМИ ДАННЫМИ =====
PRINT 'Заполнение таблиц начальными данными...';
GO

INSERT INTO Warehouses (Location) VALUES 
(N'Центральный склад (Москва)'), 
(N'Филиал (Санкт-Петербург)');

INSERT INTO Products (Name, Category) VALUES 
(N'Клавиатура G Pro X', N'Периферия'), 
(N'Мышь MX Master 3S', N'Периферия'), 
(N'Монитор Odyssey G7', N'Мониторы');

INSERT INTO Stock (ProductID, WarehouseID, Quantity) VALUES 
(1, 1, 150),
(2, 1, 200),
(3, 2, 80),
(1, 2, 75);
GO

-- ===== ШАГ 4: СОЗДАНИЕ ХРАНИМЫХ ПРОЦЕДУР =====
PRINT 'Создание хранимых процедур...';
GO

CREATE PROCEDURE sp_GetStockReport
AS
BEGIN
    SELECT 
        s.StockID,
        p.Name AS ProductName,
        w.Location AS WarehouseLocation,
        s.Quantity,
        p.Category
    FROM Stock s
    JOIN Products p ON s.ProductID = p.ProductID
    JOIN Warehouses w ON s.WarehouseID = w.WarehouseID;
END
GO

CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT ProductID, Name FROM Products ORDER BY Name;
END
GO

CREATE PROCEDURE sp_GetAllWarehouses
AS
BEGIN
    SELECT WarehouseID, Location FROM Warehouses ORDER BY Location;
END
GO

CREATE PROCEDURE sp_AddStockItem
    @ProductID INT,
    @WarehouseID INT,
    @Quantity INT
AS
BEGIN
    INSERT INTO Stock (ProductID, WarehouseID, Quantity)
    VALUES (@ProductID, @WarehouseID, @Quantity);
END
GO

CREATE PROCEDURE sp_UpdateStockQuantity
    @StockID INT,
    @NewQuantity INT
AS
BEGIN
    UPDATE Stock SET Quantity = @NewQuantity WHERE StockID = @StockID;
END
GO

CREATE PROCEDURE sp_DeleteStockItem
    @StockID INT
AS
BEGIN
    DELETE FROM Stock WHERE StockID = @StockID;
END
GO

CREATE PROCEDURE sp_FilterStock
    @ProductName NVARCHAR(100) = NULL,
    @WarehouseLocation NVARCHAR(100) = NULL
AS
BEGIN
    SELECT 
        s.StockID,
        p.Name AS ProductName,
        w.Location AS WarehouseLocation,
        s.Quantity,
        p.Category
    FROM Stock s
    JOIN Products p ON s.ProductID = p.ProductID
    JOIN Warehouses w ON s.WarehouseID = w.WarehouseID
    WHERE 
        (@ProductName IS NULL OR p.Name LIKE '%' + @ProductName + '%')
        AND
        (@WarehouseLocation IS NULL OR w.Location LIKE '%' + @WarehouseLocation + '%');
END
GO

CREATE PROCEDURE sp_GetStockGroupedByWarehouse
AS
BEGIN
    SELECT 
        w.Location AS WarehouseLocation,
        SUM(s.Quantity) AS TotalQuantity
    FROM Stock s
    JOIN Warehouses w ON s.WarehouseID = w.WarehouseID
    GROUP BY w.Location
    ORDER BY TotalQuantity DESC;
END
GO

PRINT '==================================================';
PRINT 'Скрипт успешно выполнен. База данных готова к работе.';
PRINT '==================================================';
GO