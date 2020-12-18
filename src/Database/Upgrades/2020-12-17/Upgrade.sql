ALTER TABLE [Jump] ADD
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,

    CONSTRAINT [DF_1] DEFAULT 0 FOR [Latitude],
    CONSTRAINT [DF_2] DEFAULT 0 FOR [Longitude]
GO

ALTER TABLE [Jump]
DROP [DF_1], [DF_2]
GO