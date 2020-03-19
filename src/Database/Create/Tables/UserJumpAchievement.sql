CREATE TABLE [UserJumpAchievement] (
    [UserJumpAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_UserJumpAchievement_User] REFERENCES [User],
    [JumpAchievementId] int NOT NULL CONSTRAINT [FK_UserJumpAchievement_JumpAchievement] REFERENCES [JumpAchievement],
    [RideId] int NOT NULL CONSTRAINT [FK_UserJumpAchievement_Ride] REFERENCES [Ride],
)