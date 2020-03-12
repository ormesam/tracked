CREATE TABLE [JumpAchievement] (
    [JumpAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [Name] nvarchar(200) NOT NULL,
    [MinAirtime] decimal(5,3) NOT NULL,
)