CREATE TABLE [dbo].[FreeConsumptionHistory]
(
	[IdHistory] INT NOT NULL IDENTITY PRIMARY KEY, 
	[IdUser] INT NOT NULL,
    [CheckDate] DATETIME2 NOT NULL, 
    [VoiceConsumption] TIME NOT NULL, 
    [DataConsumption] DECIMAL(9, 4) NOT NULL,
	CONSTRAINT FK_FreeConsumptionHistory_Profile FOREIGN KEY (IdUser) REFERENCES [Profile](IdProfile)
)

GO
