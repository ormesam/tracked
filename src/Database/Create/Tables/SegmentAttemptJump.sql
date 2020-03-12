CREATE TABLE [SegmentAttemptJump] (
    [SegmentAttemptJumpId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentAttemptId] int NOT NULL CONSTRAINT [FK_SegmentAttemptJump_SegmentAttempt] REFERENCES [SegmentAttempt],
    [JumpId] int NOT NULL CONSTRAINT [FK_SegmentAttemptJump_Jump] REFERENCES [Jump],

    [Number] int NOT NULL,
)