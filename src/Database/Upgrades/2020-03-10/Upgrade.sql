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