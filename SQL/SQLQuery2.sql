-- Создание базы данных (Настольные игры)
CREATE DATABASE BoardGamesClub;
GO
USE BoardGamesClub;
GO



-- Создание таблиц
-- Игроки
CREATE TABLE Players (
    PlayerID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    BirthDate DATE,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Игры
CREATE TABLE Games (
    GameID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(150) NOT NULL,
    MinPlayers INT,
    MaxPlayers INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Игровые сессии
CREATE TABLE GameSessions (
    SessionID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    GameID UNIQUEIDENTIFIER NOT NULL,
    SessionDate DATETIME NOT NULL,
    DurationMinutes INT,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Результаты игроков (связующая для N:N между <Игроки> и <Игровые сессии>)
CREATE TABLE PlayerScores (
    PlayerID UNIQUEIDENTIFIER NOT NULL,
    SessionID UNIQUEIDENTIFIER NOT NULL,
    Score INT NOT NULL,
    Winner BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (PlayerID, SessionID)
);

-- Рейтинги игр
CREATE TABLE GameRatings (
    RatingID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    GameID UNIQUEIDENTIFIER NOT NULL,
    RatingValue DECIMAL(3,2) CHECK (RatingValue BETWEEN 1 AND 5),
    IsRecommended BIT,
    RatingDate DATETIME DEFAULT GETDATE()
);



-- Связи 1:N между таблицами
-- Игра -> Сессии
ALTER TABLE GameSessions
ADD CONSTRAINT FK_GameSessions_Games
FOREIGN KEY (GameID) REFERENCES Games(GameID);

-- Игра -> Рейтинги
ALTER TABLE GameRatings
ADD CONSTRAINT FK_GameRatings_Games
FOREIGN KEY (GameID) REFERENCES Games(GameID);

-- Игрок -> Результаты
ALTER TABLE PlayerScores
ADD CONSTRAINT FK_PlayerScores_Players
FOREIGN KEY (PlayerID) REFERENCES Players(PlayerID);

-- Сессия -> Результаты
ALTER TABLE PlayerScores
ADD CONSTRAINT FK_PlayerScores_Sessions
FOREIGN KEY (SessionID) REFERENCES GameSessions(SessionID);



-- Уникальные индекс (Реализован через Email, так как должен быть уникален)
CREATE UNIQUE INDEX UQ_Players_Email ON Players(Email);



-- Вставка данных
INSERT INTO Players (PlayerID, FullName, Email, BirthDate, RegistrationDate, IsActive) VALUES
    (NEWID(), N'Алексей Смирнов', N'alexey@example.com', '1995-03-15', GETDATE(), 1),
    (NEWID(), N'Мария Иванова', N'maria@example.com', '1998-07-22', GETDATE(), 1),
    (NEWID(), N'Дмитрий Петров', N'dmitry@example.com', '2000-11-05', GETDATE(), 0),
    (NEWID(), N'Елена Сидорова', N'elena@example.com', '1997-01-30', GETDATE(), 1);

INSERT INTO Games (GameID, Title, MinPlayers, MaxPlayers, CreatedDate, IsActive) VALUES
    (NEWID(), N'Ветераны', 2, 5, GETDATE(), 1),
    (NEWID(), N'Манчкин', 3, 6, GETDATE(), 1),
    (NEWID(), N'Колонизаторы', 3, 4, GETDATE(), 0),
    (NEWID(), N'Имаджинариум', 4, 7, GETDATE(), 1);

INSERT INTO GameSessions (SessionID, GameID, SessionDate, DurationMinutes, IsActive) VALUES
    (NEWID(), (SELECT TOP 1 GameID FROM Games WHERE Title = N'Ветераны'), '20240220 18:00:00', 90, 1),
    (NEWID(), (SELECT TOP 1 GameID FROM Games WHERE Title = N'Манчкин'), '20240221 19:30:00', 120, 1),
    (NEWID(), (SELECT TOP 1 GameID FROM Games WHERE Title = N'Имаджинариум'), '20240222 17:00:00', 75, 1);

INSERT INTO PlayerScores (PlayerID, SessionID, Score, Winner) VALUES
    ((SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Алексей Смирнов'), (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '20240220 18:00:00'), 45, 1),
    ((SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Мария Иванова'), (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '20240220 18:00:00'), 38, 0),
    ((SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Дмитрий Петров'), (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '20240221 19:30:00'), 52, 0),
    ((SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Елена Сидорова'), (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '20240221 19:30:00'), 67, 1),
    ((SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Алексей Смирнов'), (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '20240222 17:00:00'), 28, 0),
    ((SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Мария Иванова'), (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '20240222 17:00:00'), 31, 1);

INSERT INTO GameRatings (RatingID, GameID, RatingValue, IsRecommended, RatingDate) VALUES
    (NEWID(), (SELECT TOP 1 GameID FROM Games WHERE Title = N'Ветераны'), 4.80, 1, GETDATE()),
    (NEWID(), (SELECT TOP 1 GameID FROM Games WHERE Title = N'Манчкин'), 4.20, 1, GETDATE()),
    (NEWID(), (SELECT TOP 1 GameID FROM Games WHERE Title = N'Имаджинариум'), 4.90, 1, GETDATE());


/*
-- Экспорт Бэкапа
BACKUP DATABASE BoardGamesClub
TO DISK = 'C:\Backup\BoardGamesClub.bak'
WITH FORMAT, INIT, NAME = 'Full Backup';

-- Импорт Бэкапа
RESTORE DATABASE BoardGamesClub
FROM DISK = 'C:\Backup\BoardGamesClub.bak'
WITH REPLACE,
     RECOVERY;
GO
*/

-- Работа с данными
-- Выборка с фильтрацией и сортировкой (Игроки старше 25 лет, отсортированные по имени)
SELECT FullName, Email, BirthDate, RegistrationDate
FROM Players
WHERE DATEDIFF(YEAR, BirthDate, GETDATE()) > 25
ORDER BY FullName;

-- Изменение данных (счет победителя в конкретной сессии)
UPDATE PlayerScores
SET Score = 55, Winner = 1
WHERE PlayerID = (SELECT TOP 1 PlayerID FROM Players WHERE FullName = N'Мария Иванова')
  AND SessionID = (SELECT TOP 1 SessionID FROM GameSessions WHERE SessionDate = '2024-02-22 17:00:00');

-- Удаление данных (неактивные игроки и их результаты)
DELETE FROM PlayerScores
WHERE PlayerID IN (SELECT PlayerID FROM Players WHERE IsActive = 0);

DELETE FROM Players
WHERE IsActive = 0;

-- Выборка с группировкой (средний счет и количество побед по каждому игроку)
SELECT p.FullName, 
       AVG(ps.Score) AS AvgScore, 
       COUNT(ps.SessionID) AS GamesPlayed, 
       SUM(CASE WHEN ps.Winner = 1 THEN 1 ELSE 0 END) AS Wins
FROM Players p
JOIN PlayerScores ps ON p.PlayerID = ps.PlayerID
GROUP BY p.FullName
ORDER BY AvgScore DESC;

-- Выборки из нескольких связанных таблиц
-- LEFT JOIN (все игроки и их результаты)
SELECT p.FullName, g.Title, ps.Score, ps.Winner
FROM Players p
LEFT JOIN PlayerScores ps ON p.PlayerID = ps.PlayerID
LEFT JOIN GameSessions gs ON ps.SessionID = gs.SessionID
LEFT JOIN Games g ON gs.GameID = g.GameID
ORDER BY p.FullName;

-- RIGHT JOIN (все сессии и участники)
SELECT gs.SessionDate, g.Title, p.FullName, ps.Score
FROM Players p
RIGHT JOIN PlayerScores ps ON p.PlayerID = ps.PlayerID
RIGHT JOIN GameSessions gs ON ps.SessionID = gs.SessionID
RIGHT JOIN Games g ON gs.GameID = g.GameID
ORDER BY gs.SessionDate;

-- INNER JOIN (победители в играх с рейтингом выше 4.5)
SELECT DISTINCT p.FullName, g.Title, ps.Score, gr.RatingValue
FROM Players p
INNER JOIN PlayerScores ps ON p.PlayerID = ps.PlayerID
INNER JOIN GameSessions gs ON ps.SessionID = gs.SessionID
INNER JOIN Games g ON gs.GameID = g.GameID
INNER JOIN GameRatings gr ON g.GameID = gr.GameID
WHERE ps.Winner = 1 AND gr.RatingValue > 4.5
ORDER BY gr.RatingValue DESC;