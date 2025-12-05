USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'DormitoryDB')
BEGIN
    ALTER DATABASE DormitoryDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DormitoryDB;
END
GO

CREATE DATABASE DormitoryDB;
GO

USE DormitoryDB;
GO

CREATE TABLE Faculties (
    FacultyID INT PRIMARY KEY IDENTITY(1,1),
    FacultyName NVARCHAR(100) NOT NULL,
    DeanName NVARCHAR(100),
    DeanPhone NVARCHAR(20)
);

CREATE TABLE Curators (
    CuratorID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200),
    Phone NVARCHAR(20),
    Position NVARCHAR(50)
);

CREATE TABLE Students (
    StudentCardID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    FacultyID INT NOT NULL,
    RoomNumber INT NOT NULL,
    GroupNum NVARCHAR(20),
    CuratorID INT NOT NULL,
    CONSTRAINT FK_Students_Faculties FOREIGN KEY (FacultyID) REFERENCES Faculties(FacultyID),
    CONSTRAINT FK_Students_Curators FOREIGN KEY (CuratorID) REFERENCES Curators(CuratorID)
);
GO

INSERT INTO Faculties (FacultyName, DeanName, DeanPhone) VALUES
(N'Информационных Технологий', N'Иванов И.И.', '555-0101'),
(N'Экономический', N'Петров П.П.', '555-0202'),
(N'Юридический', N'Сидоров С.С.', '555-0303');

INSERT INTO Curators (FullName, Address, Phone, Position) VALUES
(N'Смирнова Е.А.', N'ул. Ленина 10', '555-1111', N'Старший преподаватель'),
(N'Кузнецов А.В.', N'пр. Мира 5', '555-2222', N'Доцент'),
(N'Морозова Т.Н.', N'ул. Гагарина 15', '555-3333', N'Профессор');

INSERT INTO Students (FullName, FacultyID, RoomNumber, GroupNum, CuratorID) VALUES
(N'Алексеев Алексей', 1, 101, N'ИТ-21', 1),
(N'Борисов Борис', 1, 101, N'ИТ-21', 1),
(N'Васильева Василиса', 2, 205, N'ЭК-11', 2),
(N'Григорьев Григорий', 3, 303, N'ЮР-31', 3),
(N'Дмитриев Дмитрий', 1, 102, N'ИТ-22', 1);
GO

-- Процедуры Студенты
CREATE PROCEDURE sp_GetStudentReport AS
BEGIN
    SELECT s.StudentCardID, s.FullName, f.FacultyName, s.RoomNumber, s.GroupNum, c.FullName AS CuratorName
    FROM Students s
    JOIN Faculties f ON s.FacultyID = f.FacultyID
    JOIN Curators c ON s.CuratorID = c.CuratorID;
END;
GO

CREATE PROCEDURE sp_AddStudent
    @FullName NVARCHAR(100), @FacultyID INT, @RoomNumber INT, @GroupNum NVARCHAR(20), @CuratorID INT
AS
BEGIN
    INSERT INTO Students (FullName, FacultyID, RoomNumber, GroupNum, CuratorID)
    VALUES (@FullName, @FacultyID, @RoomNumber, @GroupNum, @CuratorID);
END;
GO

CREATE PROCEDURE sp_UpdateStudent
    @StudentCardID INT, @FullName NVARCHAR(100), @FacultyID INT, @RoomNumber INT, @GroupNum NVARCHAR(20), @CuratorID INT
AS
BEGIN
    UPDATE Students
    SET FullName = @FullName, FacultyID = @FacultyID, RoomNumber = @RoomNumber, GroupNum = @GroupNum, CuratorID = @CuratorID
    WHERE StudentCardID = @StudentCardID;
END;
GO

CREATE PROCEDURE sp_DeleteStudent @StudentCardID INT AS
BEGIN
    DELETE FROM Students WHERE StudentCardID = @StudentCardID;
END;
GO

-- Фильтр студентов
CREATE PROCEDURE sp_FilterStudents
    @RoomNumber INT = NULL, @CuratorName NVARCHAR(100) = NULL, @FacultyName NVARCHAR(100) = NULL
AS
BEGIN
    SELECT s.StudentCardID, s.FullName, f.FacultyName, s.RoomNumber, s.GroupNum, c.FullName AS CuratorName
    FROM Students s
    JOIN Faculties f ON s.FacultyID = f.FacultyID
    JOIN Curators c ON s.CuratorID = c.CuratorID
    WHERE (@RoomNumber IS NULL OR s.RoomNumber = @RoomNumber) 
      AND (@CuratorName IS NULL OR c.FullName LIKE N'%' + @CuratorName + N'%') 
      AND (@FacultyName IS NULL OR f.FacultyName LIKE N'%' + @FacultyName + N'%');
END;
GO

-- Процедуры Факультеты
CREATE PROCEDURE sp_GetAllFaculties AS
BEGIN
    SELECT * FROM Faculties;
END;
GO

CREATE PROCEDURE sp_AddFaculty @FacultyName NVARCHAR(100), @DeanName NVARCHAR(100), @DeanPhone NVARCHAR(20) AS
BEGIN
    INSERT INTO Faculties (FacultyName, DeanName, DeanPhone) VALUES (@FacultyName, @DeanName, @DeanPhone);
END;
GO

CREATE PROCEDURE sp_UpdateFaculty @FacultyID INT, @FacultyName NVARCHAR(100), @DeanName NVARCHAR(100), @DeanPhone NVARCHAR(20) AS
BEGIN
    UPDATE Faculties SET FacultyName = @FacultyName, DeanName = @DeanName, DeanPhone = @DeanPhone WHERE FacultyID = @FacultyID;
END;
GO

CREATE PROCEDURE sp_DeleteFaculty @FacultyID INT AS
BEGIN
    DELETE FROM Faculties WHERE FacultyID = @FacultyID;
END;
GO

-- Процедуры Кураторы
CREATE PROCEDURE sp_GetAllCurators AS
BEGIN
    SELECT * FROM Curators;
END;
GO

CREATE PROCEDURE sp_AddCurator @FullName NVARCHAR(100), @Address NVARCHAR(200), @Phone NVARCHAR(20), @Position NVARCHAR(50) AS
BEGIN
    INSERT INTO Curators (FullName, Address, Phone, Position) VALUES (@FullName, @Address, @Phone, @Position);
END;
GO

CREATE PROCEDURE sp_UpdateCurator @CuratorID INT, @FullName NVARCHAR(100), @Address NVARCHAR(200), @Phone NVARCHAR(20), @Position NVARCHAR(50) AS
BEGIN
    UPDATE Curators SET FullName = @FullName, Address = @Address, Phone = @Phone, Position = @Position WHERE CuratorID = @CuratorID;
END;
GO

CREATE PROCEDURE sp_DeleteCurator @CuratorID INT AS
BEGIN
    DELETE FROM Curators WHERE CuratorID = @CuratorID;
END;
GO

-- Для отчетов
CREATE PROCEDURE sp_GetFacultySummary AS
BEGIN
    SELECT f.FacultyName, c.FullName AS CuratorName, COUNT(s.StudentCardID) AS StudentCount
    FROM Faculties f
    LEFT JOIN Students s ON f.FacultyID = s.FacultyID
    LEFT JOIN Curators c ON s.CuratorID = c.CuratorID
    GROUP BY f.FacultyName, c.FullName
    HAVING c.FullName IS NOT NULL
    ORDER BY f.FacultyName, c.FullName;
END;
GO