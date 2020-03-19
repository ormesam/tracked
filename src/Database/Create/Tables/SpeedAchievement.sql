CREATE TABLE [SpeedAchievement] (
    [SpeedAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [Name] nvarchar(200) NOT NULL,
    [MinMph] decimal(4,1) NOT NULL,
)