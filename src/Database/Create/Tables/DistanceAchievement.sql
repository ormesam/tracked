CREATE TABLE [DistanceAchievement] (
    [DistanceAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [Name] nvarchar(200) NOT NULL,
    [MinDistanceMiles] float NOT NULL,
)