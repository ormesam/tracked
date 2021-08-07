ALTER TABLE [RideLocation] ADD
    [IsRemoved] bit NOT NULL,
    [RemovalReason] nvarchar(255) NULL,
    CONSTRAINT [DF_Temp1] DEFAULT 0 FOR [IsRemoved]
GO

ALTER TABLE [RideLocation]
DROP [DF_Temp1]
GO

ALTER TABLE [Ride] ADD
    [AnalyserVersion] int NOT NULL,
    CONSTRAINT [DF_Temp2] DEFAULT 1 FOR [AnalyserVersion]
GO

ALTER TABLE [Ride]
DROP [DF_Temp2]
GO