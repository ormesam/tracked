ALTER TABLE [RideLocation] ADD
    [IsRemoved] bit NOT NULL,
    [RemovalReason] nvarchar(255) NULL,
    CONSTRAINT [DF_Temp1] DEFAULT 0 FOR [IsRemoved]
GO

ALTER TABLE [RideLocation]
DROP [DF_Temp1]
GO

ALTER TABLE [TrailAttempt] ADD
    [TrailAnalyserVersion] int NOT NULL,
    CONSTRAINT [DF_Temp2] DEFAULT 1 FOR [TrailAnalyserVersion]
GO

ALTER TABLE [TrailAttempt]
DROP [DF_Temp2]
GO