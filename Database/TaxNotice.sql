CREATE TABLE [dbo].[TaxNotice]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [Amount] DECIMAL(9, 4) NOT NULL, 
    [PaymentDate] DATETIME2 NOT NULL
)
