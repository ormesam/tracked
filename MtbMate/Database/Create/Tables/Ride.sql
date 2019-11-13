CREATE TABLE [Ride] (
    [RideId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_Ride_User] REFERENCES [User],

    [Name] nvarchar(255) NULL,
    [StartUtc] datetime NOT NULL,
    [EndUtc] datetime NOT NULL,
)