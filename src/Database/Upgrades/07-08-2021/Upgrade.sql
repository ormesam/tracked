ALTER TABLE [RideLocation] ADD
    [IsRemoved] bit NOT NULL,
    [RemovalReason] nvarchar(255) NULL,
    CONSTRAINT [DF_Temp1] DEFAULT 0 FOR [IsRemoved]
GO

ALTER TABLE [RideLocation]
DROP [DF_Temp1]
GO

ALTER TABLE [Ride] ADD
    [AnalyserVersion] int NOT NULL,
    CONSTRAINT [DF_Temp2] DEFAULT 1 FOR [AnalyserVersion]
GO

ALTER TABLE [Ride]
DROP [DF_Temp2]
GO

CREATE TABLE [UserBlock] (
    [UserBlockId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    
    [UserId] int NOT NULL CONSTRAINT [FK_UserBlock_User] REFERENCES [User],
    [BlockUserId] int NOT NULL CONSTRAINT [FK_UserBlock_BlockUser] REFERENCES [User],

    [BlockedUtc] datetime NOT NULL,
)

CREATE UNIQUE INDEX [IX_UserBlock] ON [UserBlock]([UserId], [BlockUserId]) 

CREATE TABLE [UserFollow] (
    [UserFollowId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    
    [UserId] int NOT NULL CONSTRAINT [FK_UserFollow_User] REFERENCES [User],
    [FollowUserId] int NOT NULL CONSTRAINT [FK_UserFollow_FollowUser] REFERENCES [User],

    [FollowedUtc] datetime NOT NULL,
)

CREATE UNIQUE INDEX [IX_UserFollow] ON [UserFollow]([UserId], [FollowUserId]) 