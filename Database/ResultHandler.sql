CREATE TABLE [dbo].[ResultHandler]
(
	[IdResultHandler] INT NOT NULL IDENTITY PRIMARY KEY, 
    [ResultHandlerName] VARCHAR(50) NOT NULL, 
	[Activated] BIT NOT NULL,
    [IdMonitor] INT NOT NULL,
    [IdProfile] INT NULL,
	CONSTRAINT FK_ResultHandler_Monitor FOREIGN KEY (IdMonitor) REFERENCES Monitor(IdMonitor),
	CONSTRAINT FK_ResultHandler_Profile FOREIGN KEY (IdProfile) REFERENCES [Profile](IdProfile)
)
