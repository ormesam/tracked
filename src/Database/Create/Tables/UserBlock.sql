CREATE TABLE [UserBlock] (
    [UserBlockId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    
    [UserId] int NOT NULL CONSTRAINT [FK_UserBlock_User] REFERENCES [User],
    [BlockUserId] int NOT NULL CONSTRAINT [FK_UserBlock_BlockUser] REFERENCES [User],

    [BlockedUtc] datetime NOT NULL,
)