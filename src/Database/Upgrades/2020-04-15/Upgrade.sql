PRINT N'Altering [dbo].[AccelerometerReading]...';
GO
ALTER TABLE [dbo].[AccelerometerReading] ALTER COLUMN [X] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[AccelerometerReading] ALTER COLUMN [Y] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[AccelerometerReading] ALTER COLUMN [Z] FLOAT (53) NOT NULL;
GO
PRINT N'Altering [dbo].[Jump]...';
GO
ALTER TABLE [dbo].[Jump] ALTER COLUMN [Airtime] FLOAT (53) NOT NULL;
GO
PRINT N'Altering [dbo].[JumpAchievement]...';
GO
ALTER TABLE [dbo].[JumpAchievement] ALTER COLUMN [MinAirtime] FLOAT (53) NOT NULL;
GO
PRINT N'Altering [dbo].[RideLocation]...';
GO
ALTER TABLE [dbo].[RideLocation] ALTER COLUMN [AccuracyInMetres] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[RideLocation] ALTER COLUMN [Altitude] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[RideLocation] ALTER COLUMN [Latitude] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[RideLocation] ALTER COLUMN [Longitude] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[RideLocation] ALTER COLUMN [SpeedMetresPerSecond] FLOAT (53) NOT NULL;
GO
PRINT N'Altering [dbo].[SegmentLocation]...';
GO
ALTER TABLE [dbo].[SegmentLocation] ALTER COLUMN [Latitude] FLOAT (53) NOT NULL;
ALTER TABLE [dbo].[SegmentLocation] ALTER COLUMN [Longitude] FLOAT (53) NOT NULL;
GO
PRINT N'Altering [dbo].[SpeedAchievement]...';
GO
ALTER TABLE [dbo].[SpeedAchievement] ALTER COLUMN [MinMph] FLOAT (53) NOT NULL;
GO
PRINT N'Update complete.';
GO
