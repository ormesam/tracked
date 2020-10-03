CREATE TABLE [UserDistanceAchievement] (
    [UserDistanceAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_UserDistanceAchievement_User] REFERENCES [User],
    [DistanceAchievementId] int NOT NULL CONSTRAINT [FK_UserDistanceAchievement_DistanceAchievement] REFERENCES [DistanceAchievement],
    [RideId] int NOT NULL CONSTRAINT [FK_UserDistanceAchievement_Ride] REFERENCES [Ride],
)