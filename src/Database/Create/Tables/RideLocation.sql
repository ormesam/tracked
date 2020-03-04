CREATE TABLE [RideLocation] (
    [RideLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [RideId] int NOT NULL CONSTRAINT [FK_RideLocation_Ride] REFERENCES [Ride],

    [Timestamp] datetime NOT NULL,
    [Latitude] decimal(25,20) NOT NULL,
    [Longitude] decimal(25,20) NOT NULL,
    [AccuracyInMetres] decimal(6,3) NOT NULL,
    [SpeedMetresPerSecond] decimal(6,3) NOT NULL,
    [Altitude] decimal(6,3) NOT NULL,
)