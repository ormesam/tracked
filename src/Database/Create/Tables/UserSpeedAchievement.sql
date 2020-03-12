CREATE TABLE [UserSpeedAchievement] (
    [UserSpeedAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_UserSpeedAchievement_User] REFERENCES [User],
    [SpeedAchievementId] int NOT NULL CONSTRAINT [FK_UserSpeedAchievement_SpeedAchievement] REFERENCES [SpeedAchievement],
    [RideId] int NOT NULL CONSTRAINT [FK_UserSpeedAchievement_Ride] REFERENCES [Ride],
)