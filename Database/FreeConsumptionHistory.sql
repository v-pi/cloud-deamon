CREATE TABLE [dbo].[FreeConsumptionHistory]
(
	[IdHistory] INT NOT NULL IDENTITY PRIMARY KEY, 
	[IdUser] INT NOT NULL,
    [CheckDate] DATETIME2 NOT NULL, 
    [VoiceConsumption] TIME NOT NULL, 
    [DataConsumption] DECIMAL NOT NULL
)

GO
