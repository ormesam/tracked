CREATE TABLE [SegmentAttemptLocation] (
    [SegmentAttemptLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentAttemptId] int NOT NULL CONSTRAINT [FK_SegmentAttemptLocation_SegmentAttempt] REFERENCES [SegmentAttempt],

    [Timestamp] datetime NOT NULL,
    [Latitude] decimal(25,20) NOT NULL,
    [Longitude] decimal(25,20) NOT NULL,
    [AccuracyInMetres] decimal(6,3) NOT NULL,
    [SpeedMetresPerSecond] decimal(6,3) NOT NULL,
    [Altitude] decimal(6,3) NOT NULL,
)