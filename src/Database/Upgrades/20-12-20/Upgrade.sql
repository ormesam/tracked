ALTER TABLE [User] ADD
    [RefreshToken] nvarchar(255) NOT NULL,
    CONSTRAINT [DF_1] DEFAULT NewId() FOR [RefreshToken]
GO

ALTER TABLE [User] DROP [DF_1]
GO