ALTER TABLE [Ride]
ADD [Name] nvarchar(200) NULL
GO

CREATE TABLE [SegmentAttemptJump] (
    [SegmentAttemptJumpId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentAttemptId] int NOT NULL CONSTRAINT [FK_SegmentAttemptJump_SegmentAttempt] REFERENCES [SegmentAttempt],
    [JumpId] int NOT NULL CONSTRAINT [FK_SegmentAttemptJump_Jump] REFERENCES [Jump],

    [Number] int NOT NULL,
)

CREATE TABLE [SegmentAttemptLocation] (
    [SegmentAttemptLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentAttemptId] int NOT NULL CONSTRAINT [FK_SegmentAttemptLocation_SegmentAttempt] REFERENCES [SegmentAttempt],
    [RideLocationId] int NOT NULL CONSTRAINT [FK_SegmentAttemptLocation_RideLocation] REFERENCES [RideLocation],
)

CREATE TABLE [JumpAchievement] (
    [JumpAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [Name] nvarchar(200) NOT NULL,
    [MinAirtime] decimal(5,3) NOT NULL,
)

CREATE TABLE [SpeedAchievement] (
    [SpeedAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [Name] nvarchar(200) NOT NULL,
    [MinMph] decimal(4,1) NOT NULL,
)

CREATE TABLE [UserJumpAchievement] (
    [UserJumpAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_UserJumpAchievement_User] REFERENCES [User],
    [JumpAchievementId] int NOT NULL CONSTRAINT [FK_UserJumpAchievement_JumpAchievement] REFERENCES [JumpAchievement],
    [RideId] int NOT NULL CONSTRAINT [FK_UserJumpAchievement_Ride] REFERENCES [Ride],
)

CREATE TABLE [UserSpeedAchievement] (
    [UserSpeedAchievementId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_UserSpeedAchievement_User] REFERENCES [User],
    [SpeedAchievementId] int NOT NULL CONSTRAINT [FK_UserSpeedAchievement_SpeedAchievement] REFERENCES [SpeedAchievement],
    [RideId] int NOT NULL CONSTRAINT [FK_UserSpeedAchievement_Ride] REFERENCES [Ride],
)