CREATE TABLE [SegmentAttempt] (
    [SegmentAttemptId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_SegmentAttempt_User] REFERENCES [User],
    [SegmentId] int NOT NULL CONSTRAINT [FK_SegmentAttempt_Segment] REFERENCES [User],
    [RideId] int NOT NULL CONSTRAINT [FK_SegmentAttempt_Ride] REFERENCES [User],

    [StartUtc] datetime NOT NULL,
    [EndUtc] datetime NOT NULL,
    [Medal] int NULL,
)