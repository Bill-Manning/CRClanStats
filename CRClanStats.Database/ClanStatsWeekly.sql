CREATE TABLE [dbo].[ClanStatsWeekly]
(
    [RecordDate] DATETIME2 NOT NULL, 
    [TotalTrophies] INT NOT NULL, 
    [TotalDonations] INT NOT NULL, 
    [ClanMemberCount] INT NOT NULL, 
    [ClanChestCrownsCount] INT NOT NULL, 
    [ClanChestsCompletedCount] INT NOT NULL, 
    [ClanChestNonParticipationCount] INT NOT NULL, 
    [ClanChestFullParticipationCount] INT NOT NULL, 
    [TopDonationsPlayer] INT NOT NULL, 
    [TopCrownsPlayer] INT NOT NULL, 
    PRIMARY KEY ([RecordDate]), 
    CONSTRAINT [FK_ClanStatsWeekly_TopDonationsPlayer_ToPlayer] FOREIGN KEY ([TopDonationsPlayer]) REFERENCES [Player]([PlayerId]),
    CONSTRAINT [FK_ClanStatsWeekly_TopCrownsPlayer_ToPlayer] FOREIGN KEY ([TopCrownsPlayer]) REFERENCES [Player]([PlayerId])
)
