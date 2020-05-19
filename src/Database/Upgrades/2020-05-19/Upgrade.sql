ALTER TABLE [Ride] ADD
    [DistanceMiles] float NULL,
    [MaxSpeedMph] float NULL,
    [AverageSpeedMph] float NULL
GO

UPDATE [Ride] SET [DistanceMiles] = 0, [MaxSpeedMph] = 0, [AverageSpeedMph] = 0
GO

ALTER TABLE [Ride]
ALTER COLUMN [DistanceMiles] float NOT NULL
GO

ALTER TABLE [Ride]
ALTER COLUMN [MaxSpeedMph] float NOT NULL
GO

ALTER TABLE [Ride]
ALTER COLUMN [AverageSpeedMph] float NOT NULL
GO

ALTER TABLE [RideLocation]
ADD [Mph] float NULL
GO

UPDATE [RideLocation] SET [Mph] = [SpeedMetresPerSecond] * 2.23694
GO

ALTER TABLE [RideLocation]
DROP COLUMN [SpeedMetresPerSecond]
GO

ALTER TABLE [RideLocation]
ALTER COLUMN [Mph] float NOT NULL
GO