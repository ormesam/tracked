CREATE TABLE [Jump] (
    [JumpId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [RideId] int NOT NULL CONSTRAINT [FK_Jump_Ride] REFERENCES [Ride],

    [Number] int NOT NULL,
    [Timestamp] datetime NOT NULL,
    [Airtime] float NOT NULL,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
)