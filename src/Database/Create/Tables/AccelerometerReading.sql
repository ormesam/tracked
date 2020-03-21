CREATE TABLE [AccelerometerReading] (
    [AccelerometerReadingId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [RideId] int NOT NULL CONSTRAINT [FK_AccelerometerReading_Ride] REFERENCES [Ride],

    [Time] datetime NOT NULL,
    [X] float NOT NULL,
    [Y] float NOT NULL,
    [Z] float NOT NULL,
)