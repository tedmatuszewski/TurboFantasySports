CREATE TABLE [dbo].[LeagueMemberPlayer]
(
	[Guid] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[LeagueMemberGuid] UNIQUEIDENTIFIER NOT NULL,
	[PlayerGuid] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_Team_LeagueMember] FOREIGN KEY ([LeagueMemberGuid]) REFERENCES [LeagueMember]([Guid]), 
    CONSTRAINT [FK_Team_Player] FOREIGN KEY ([PlayerGuid]) REFERENCES [Player]([Guid])
)
