CREATE TABLE [TrailAttempt] (
    [TrailAttemptId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_TrailAttempt_User] REFERENCES [User],
    [TrailId] int NOT NULL CONSTRAINT [FK_TrailAttempt_Trail] REFERENCES [Trail],
    [RideId] int NOT NULL CONSTRAINT [FK_TrailAttempt_Ride] REFERENCES [Ride],

    [StartUtc] datetime NOT NULL,
    [EndUtc] datetime NOT NULL,
    [Medal] int NOT NULL,
)