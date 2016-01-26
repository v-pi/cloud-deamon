CREATE TABLE [dbo].[Monitor]
(
	[IdMonitor] INT NOT NULL IDENTITY PRIMARY KEY, 
    [MonitorName] VARCHAR(50) NOT NULL, 
    [Frequency] TIME NOT NULL, 
    [LastRun] DATETIME NOT NULL, 
    [IdProfile] INT NULL,
	[Activated] BIT NOT NULL,
	CONSTRAINT FK_Monitor_Profile FOREIGN KEY (IdProfile) REFERENCES [Profile](IdProfile)
)
