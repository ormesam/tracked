USE [master]
GO

IF EXISTS(select * from sys.databases where name = 'MtbMateDev')
BEGIN
    ALTER DATABASE MtbMateDev SET SINGLE_USER WITH ROLLBACK IMMEDIATE

    DROP DATABASE MtbMateDev
END

CREATE DATABASE MtbMateDev
GO

USE MtbMateDev
GO