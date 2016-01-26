CREATE TABLE [dbo].[AvailableTennisSlotsHistory]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [AvailableDate] DATETIME2 NOT NULL, 
    [CourtNumber] TINYINT NOT NULL, 
    [AddedDate] DATETIME2 NOT NULL
)
