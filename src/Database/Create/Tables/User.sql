CREATE TABLE [User] (
    [UserId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [GoogleUserId] nvarchar(255) NOT NULL UNIQUE,
    [Name] nvarchar(255) NOT NULL,
    [Bio] nvarchar(max) NULL,
    [CreatedUtc] datetime NOT NULL,
    [ProfileImageUrl] nvarchar(255) NOT NULL,
    [IsAdmin] bit NOT NULL,
    [RefreshToken] nvarchar(255) NOT NULL,
)