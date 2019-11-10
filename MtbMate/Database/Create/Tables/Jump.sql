CREATE TABLE [Jump] (
    [JumpId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

	[RideId] int NOT NULL CONSTRAINT [FK_Jump_Ride] REFERENCES [Ride],

	[Number] int NOT NULL,
	[Time] datetime NOT NULL,
	[Airtime] decimal NOT NULL,
	[LandingGForce] decimal NOT NULL,
)