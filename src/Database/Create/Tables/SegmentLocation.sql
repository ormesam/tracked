CREATE TABLE [SegmentLocation] (
    [SegmentLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentId] int NOT NULL CONSTRAINT [FK_SegmentLocation_Segment] REFERENCES [Segment],

    [Order] int NOT NULL,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
)