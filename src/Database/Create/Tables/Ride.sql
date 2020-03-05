CREATE TABLE [Ride] (
    [RideId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_Ride_User] REFERENCES [User],

    [StartUtc] datetime NOT NULL,
    [EndUtc] datetime NOT NULL,
    [Name] nvarchar(200) NULL,
    [MaxSpeedMph] decimal(4,1) NOT NULL,
    [AverageSpeedMph] decimal(4,1) NOT NULL,
    [DistanceMiles] decimal(4,1) NOT NULL,
)