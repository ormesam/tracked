CREATE TABLE [TrailLocation] (
    [TrailLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [TrailId] int NOT NULL CONSTRAINT [FK_TrailLocation_Trail] REFERENCES [Trail],

    [Order] int NOT NULL,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
)