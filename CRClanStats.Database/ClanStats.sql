CREATE TABLE [dbo].[ClanStats]
(
    [RecordDate] DATETIME2 NOT NULL, 
    [Trophies] INT NOT NULL, 
    [Donations] INT NOT NULL, 
    [ClanChestCrowns] INT NOT NULL, 
    [ClanChestsCompleted] INT NOT NULL, 
    [ClanMemberCount] INT NOT NULL, 
    PRIMARY KEY ([RecordDate]), 
)
