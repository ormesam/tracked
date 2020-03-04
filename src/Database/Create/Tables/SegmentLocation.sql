CREATE TABLE [SegmentLocation] (
    [SegmentLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [SegmentId] int NOT NULL CONSTRAINT [FK_SegmentLocation_Segment] REFERENCES [Segment],

    [Order] int NOT NULL,
    [Latitude] decimal(25,20) NOT NULL,
    [Longitude] decimal(25,20) NOT NULL,
)