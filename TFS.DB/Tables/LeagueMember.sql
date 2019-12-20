CREATE TABLE [dbo].[LeagueMember]
(
	[Guid] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[LeagueGuid] UNIQUEIDENTIFIER NOT NULL,
	[UserGuid] UNIQUEIDENTIFIER NOT NULL, 
    [Name] VARCHAR(511) NOT NULL, 
    CONSTRAINT [FK_LeagueMember_League] FOREIGN KEY ([LeagueGuid]) REFERENCES [League]([Guid]),
    CONSTRAINT [FK_LeagueMember_User] FOREIGN KEY ([UserGuid]) REFERENCES [User]([Guid])
)
