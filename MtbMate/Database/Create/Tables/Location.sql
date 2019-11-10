CREATE TABLE [Location] (
    [LocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

	[RideId] int NOT NULL CONSTRAINT [FK_Location_Ride] REFERENCES [Ride],

	[Latitude] decimal NOT NULL,
	[Longitude] decimal NOT NULL,
	[AccuracyInMetres] decimal NOT NULL,
	[SpeedMetresPerSecond] decimal NOT NULL,
	[Altitude] decimal NOT NULL,
)