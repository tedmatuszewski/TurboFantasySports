﻿CREATE TABLE [dbo].[League]
(
	[Guid] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Type] VARCHAR(127) NOT NULL,
	[ManagerGuid] UNIQUEIDENTIFIER NOT NULL,
	[Name] VARCHAR(511) NOT NULL,
	[Year] INT NOT NULL, 
    CONSTRAINT [FK_League_Manager] FOREIGN KEY ([ManagerGuid]) REFERENCES [User]([Guid])
)
