CREATE TABLE [SegmentAttemptLocation] (
    [SegmentAttemptLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentAttemptId] int NOT NULL CONSTRAINT [FK_SegmentAttemptLocation_SegmentAttempt] REFERENCES [SegmentAttempt],
    [RideLocationId] int NOT NULL CONSTRAINT [FK_SegmentAttemptLocation_RideLocation] REFERENCES [RideLocation],
)