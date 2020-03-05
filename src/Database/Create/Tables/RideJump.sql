CREATE TABLE [RideJump] (
    [RideJumpId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [RideId] int NOT NULL CONSTRAINT [FK_RideJump_Ride] REFERENCES [Ride],

    [Number] int NOT NULL,
    [Timestamp] datetime NOT NULL,
    [Airtime] decimal(5,3) NOT NULL,
)