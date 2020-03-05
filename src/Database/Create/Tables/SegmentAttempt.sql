CREATE TABLE [SegmentAttempt] (
    [SegmentAttemptId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_SegmentAttempt_User] REFERENCES [User],
    [SegmentId] int NOT NULL CONSTRAINT [FK_SegmentAttempt_Segment] REFERENCES [Segment],
    [RideId] int NOT NULL CONSTRAINT [FK_SegmentAttempt_Ride] REFERENCES [Ride],

    [StartUtc] datetime NOT NULL,
    [EndUtc] datetime NOT NULL,
    [Medal] int NOT NULL,
    [MaxSpeedMph] decimal(4,1) NOT NULL,
    [AverageSpeedMph] decimal(4,1) NOT NULL,
    [DistanceMiles] decimal(4,1) NOT NULL,
)