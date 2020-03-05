CREATE TABLE [SegmentAttemptJump] (
    [SegmentAttemptJumpId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentAttemptId] int NOT NULL CONSTRAINT [FK_SegmentAttemptJump_SegmentAttempt] REFERENCES [SegmentAttempt],

    [Number] int NOT NULL,
    [Timestamp] datetime NOT NULL,
    [Airtime] decimal(5,3) NOT NULL,
)