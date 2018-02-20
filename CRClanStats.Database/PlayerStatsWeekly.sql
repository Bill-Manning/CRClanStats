CREATE TABLE [dbo].[PlayerStatsWeekly]
(
    [RecordDate] DATETIME2 NOT NULL, 
    [PlayerId] INT NOT NULL,
    [Trophies] INT NOT NULL, 
    [DonationsCount] INT NOT NULL, 
    [DonationsReceivedCount] INT NOT NULL, 
    [ClanChestCrownsCount] INT NULL, 
    [IsClanChestEligible] BIT NOT NULL DEFAULT 1, 
    [1v1MatchCount] INT NULL , 
    [2v2MatchCount] INT NULL, 
    [FriendlyMatchCount] INT NULL, 
    [TournamentMatch] INT NULL, 
    [ChallengeMatchCount] INT NULL, 
    [SharedCount] INT NULL, 
    [ChatLogPostCount] INT NULL, 
    [PlayerRoleId] INT NOT NULL, 
    PRIMARY KEY ([RecordDate], [PlayerId]), 
    CONSTRAINT [FK_PlayerStatsWeekly_ToPlayer] FOREIGN KEY ([PlayerId]) REFERENCES [Player]([PlayerId]),
    CONSTRAINT [FK_PlayerStatsWeekly_ToPlayerRole] FOREIGN KEY ([PlayerRoleId]) REFERENCES [PlayerRole]([PlayerRoleId])
)
