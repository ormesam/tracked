USE [master]
GO

IF EXISTS(select * from sys.databases where name = 'TrackedDev')
BEGIN
    ALTER DATABASE TrackedDev SET SINGLE_USER WITH ROLLBACK IMMEDIATE

    DROP DATABASE TrackedDev
END

CREATE DATABASE TrackedDev
GO

USE TrackedDev
GO