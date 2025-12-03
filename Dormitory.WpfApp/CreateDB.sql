USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'DormitoryDB')
    DROP DATABASE DormitoryDB;
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
('Информационных Технологий', 'Иванов И.И.', '555-0101'),
('Экономический', 'Петров П.П.', '555-0202'),
('Юридический', 'Сидоров С.С.', '555-0303');

INSERT INTO Curators (FullName, Address, Phone, Position) VALUES
('Смирнова Е.А.', 'ул. Ленина 10', '555-1111', 'Старший преподаватель'),
('Кузнецов А.В.', 'пр. Мира 5', '555-2222', 'Доцент'),
('Морозова Т.Н.', 'ул. Гагарина 15', '555-3333', 'Профессор');

INSERT INTO Students (FullName, FacultyID, RoomNumber, GroupNum, CuratorID) VALUES
('Алексеев Алексей', 1, 101, 'ИТ-21', 1),
('Борисов Борис', 1, 101, 'ИТ-21', 1),
('Васильева Василиса', 2, 205, 'ЭК-11', 2),
('Григорьев Григорий', 3, 303, 'ЮР-31', 3),
('Дмитриев Дмитрий', 1, 102, 'ИТ-22', 1);
GO


CREATE PROCEDURE sp_GetStudentReport
AS
BEGIN
    SELECT 
        s.StudentCardID,
        s.FullName,
        f.FacultyName,
        s.RoomNumber,
        s.GroupNum,
        c.FullName AS CuratorName
    FROM Students s
    JOIN Faculties f ON s.FacultyID = f.FacultyID
    JOIN Curators c ON s.CuratorID = c.CuratorID;
END;
GO

CREATE PROCEDURE sp_GetAllFaculties
AS
BEGIN
    SELECT FacultyID, FacultyName, DeanName, DeanPhone FROM Faculties;
END;
GO

CREATE PROCEDURE sp_GetAllCurators
AS
BEGIN
    SELECT CuratorID, FullName, Address, Phone, Position FROM Curators;
END;
GO

CREATE PROCEDURE sp_AddStudent
    @FullName NVARCHAR(100),
    @FacultyID INT,
    @RoomNumber INT,
    @GroupNum NVARCHAR(20),
    @CuratorID INT
AS
BEGIN
    INSERT INTO Students (FullName, FacultyID, RoomNumber, GroupNum, CuratorID)
    VALUES (@FullName, @FacultyID, @RoomNumber, @GroupNum, @CuratorID);
END;
GO

CREATE PROCEDURE sp_UpdateStudent
    @StudentCardID INT,
    @RoomNumber INT,
    @GroupNum NVARCHAR(20)
AS
BEGIN
    UPDATE Students
    SET RoomNumber = @RoomNumber,
        GroupNum = @GroupNum
    WHERE StudentCardID = @StudentCardID;
END;
GO

CREATE PROCEDURE sp_DeleteStudent
    @StudentCardID INT
AS
BEGIN
    DELETE FROM Students WHERE StudentCardID = @StudentCardID;
END;
GO

CREATE PROCEDURE sp_FilterStudents
    @RoomNumber INT = NULL,
    @CuratorName NVARCHAR(100) = NULL,
    @FacultyName NVARCHAR(100) = NULL
AS
BEGIN
    SELECT 
        s.StudentCardID,
        s.FullName,
        f.FacultyName,
        s.RoomNumber,
        s.GroupNum,
        c.FullName AS CuratorName
    FROM Students s
    JOIN Faculties f ON s.FacultyID = f.FacultyID
    JOIN Curators c ON s.CuratorID = c.CuratorID
    WHERE 
        (@RoomNumber IS NULL OR s.RoomNumber = @RoomNumber) AND
        (@CuratorName IS NULL OR c.FullName LIKE '%' + @CuratorName + '%') AND
        (@FacultyName IS NULL OR f.FacultyName LIKE '%' + @FacultyName + '%');
END;
GO

CREATE PROCEDURE sp_GetFacultySummary
AS
BEGIN
    SELECT 
        f.FacultyName,
        c.FullName AS CuratorName,
        COUNT(s.StudentCardID) AS StudentCount
    FROM Faculties f
    LEFT JOIN Students s ON f.FacultyID = s.FacultyID
    LEFT JOIN Curators c ON s.CuratorID = c.CuratorID
    GROUP BY f.FacultyName, c.FullName
    HAVING c.FullName IS NOT NULL
    ORDER BY f.FacultyName, c.FullName;
END;
GO