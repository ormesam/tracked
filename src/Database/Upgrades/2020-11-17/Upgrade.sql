DROP TABLE [SegmentAttempt]
GO

DROP TABLE [SegmentLocation]
GO

DROP TABLE [Segment]
GO

CREATE TABLE [Trail] (
    [TrailId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_Trail_User] REFERENCES [User],

    [Name] nvarchar(255) NULL,
)

CREATE TABLE [TrailAttempt] (
    [TrailAttemptId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_TrailAttempt_User] REFERENCES [User],
    [TrailId] int NOT NULL CONSTRAINT [FK_TrailAttempt_Trail] REFERENCES [Trail],
    [RideId] int NOT NULL CONSTRAINT [FK_TrailAttempt_Ride] REFERENCES [Ride],

    [StartUtc] datetime NOT NULL,
    [EndUtc] datetime NOT NULL,
    [Medal] int NOT NULL,
)

CREATE TABLE [TrailLocation] (
    [TrailLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [TrailId] int NOT NULL CONSTRAINT [FK_TrailLocation_Trail] REFERENCES [Trail],

    [Order] int NOT NULL,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
)

ALTER TABLE [User] ADD
    [IsAdmin] bit NOT NULL,
	CONSTRAINT [DF_1] DEFAULT 1 FOR [IsAdmin]
GO

ALTER TABLE [User]
DROP [DF_1]
GO