USE DormitoryDB;
GO

-- Запрос 3: Список комнат каждого факультета с количеством студентов
CREATE PROCEDURE sp_Query_RoomStats
AS
BEGIN
    SELECT 
        f.FacultyName AS [Факультет], 
        s.RoomNumber AS [Комната], 
        COUNT(s.StudentCardID) AS [Количество студентов]
    FROM Students s
    JOIN Faculties f ON s.FacultyID = f.FacultyID
    GROUP BY f.FacultyName, s.RoomNumber
    ORDER BY f.FacultyName, s.RoomNumber;
END;
GO

-- Запрос 4: Список кураторов заданного факультета со студентами и комнатами
CREATE PROCEDURE sp_Query_CuratorsByFaculty
    @FacultyName NVARCHAR(100)
AS
BEGIN
    SELECT 
        f.FacultyName AS [Факультет],
        c.FullName AS [Куратор],
        s.FullName AS [Студент],
        s.RoomNumber AS [Комната]
    FROM Students s
    JOIN Faculties f ON s.FacultyID = f.FacultyID
    JOIN Curators c ON s.CuratorID = c.CuratorID
    WHERE f.FacultyName LIKE N'%' + @FacultyName + N'%'
    ORDER BY c.FullName, s.FullName;
END;
GO