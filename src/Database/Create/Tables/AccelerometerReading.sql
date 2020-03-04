CREATE TABLE [AccelerometerReading] (
    [AccelerometerReadingId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [RideId] int NOT NULL CONSTRAINT [FK_AccelerometerReading_Ride] REFERENCES [Ride],

    [Time] datetime NOT NULL,
    [X] decimal(5,3) NOT NULL,
    [Y] decimal(5,3) NOT NULL,
    [Z] decimal(5,3) NOT NULL,
)