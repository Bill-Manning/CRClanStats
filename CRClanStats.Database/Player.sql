CREATE TABLE [dbo].[Player]
(
    [PlayerId] INT NOT NULL PRIMARY KEY, 
    [Tag] VARCHAR(10) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [FormerName] NVARCHAR(50) NULL, 
    [JoinCount] INT NOT NULL DEFAULT 1,
    [IsCurrentMember] BIT NOT NULL DEFAULT 1, 
    [Notes] TEXT NULL
)
