CREATE TABLE [UserFollow] (
    [UserFollowId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    
    [UserId] int NOT NULL CONSTRAINT [FK_UserFollow_User] REFERENCES [User],
    [FollowUserId] int NOT NULL CONSTRAINT [FK_UserFollow_FollowUser] REFERENCES [User],

    [FollowedUtc] datetime NOT NULL,
)

CREATE UNIQUE INDEX [IX_UserFollow] ON [UserFollow]([UserId], [FollowUserId]) 