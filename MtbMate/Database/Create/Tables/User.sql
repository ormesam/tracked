CREATE TABLE [User] (
    [UserId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [Email] nvarchar(255) NOT NULL UNIQUE,
    [PasswordHash] binary(20) NOT NULL,
    [PasswordSalt] binary(16) NOT NULL,

    [ApiKey] nvarchar(255) NOT NULL UNIQUE,
)