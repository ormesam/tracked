CREATE TABLE [TraceMessage] (
    [TraceMessageId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [DateUtc] datetime NOT NULL,
    [Message] nvarchar(max) NOT NULL,
)