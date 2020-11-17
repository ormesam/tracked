CREATE TABLE [Trail] (
    [TrailId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [UserId] int NOT NULL CONSTRAINT [FK_Trail_User] REFERENCES [User],

    [Name] nvarchar(255) NULL,
)