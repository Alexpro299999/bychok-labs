DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM Products;
DELETE FROM Categories;

DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);

INSERT INTO Categories (Name) VALUES (N'Кольца');
INSERT INTO Categories (Name) VALUES (N'Серьги');
INSERT INTO Categories (Name) VALUES (N'Ожерелья');
INSERT INTO Categories (Name) VALUES (N'Браслеты');

INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, Material)
VALUES (N'Золотое кольцо "Элегант"', N'Классическое кольцо из желтого золота 585 пробы.', 15000, '/images/ring1.jpg', 1, N'Золото');

INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, Material)
VALUES (N'Серебряное кольцо с фианитом', N'Утонченное серебряное кольцо.', 3500, '/images/ring2.jpg', 1, N'Серебро');

INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, Material)
VALUES (N'Серьги с изумрудами', N'Роскошные серьги для вечернего выхода.', 45000, '/images/earrings1.jpg', 2, N'Золото + Изумруд');

INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, Material)
VALUES (N'Серьги-гвоздики', N'Повседневные пусеты.', 2000, '/images/earrings2.jpg', 2, N'Белое золото');

INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, Material)
VALUES (N'Колье "Сердце океана"', N'Подвеска с синим камнем.', 12000, '/images/necklace1.jpg', 3, N'Серебро + Сапфир');