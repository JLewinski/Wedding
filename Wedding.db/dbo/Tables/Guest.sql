CREATE TABLE [dbo].[Guest]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[GuestName] VARCHAR(256) NOT NULL,
	[Quantity] INT NOT NULL,
	[Confirmed] BIT NOT NULL DEFAULT(0), 
	[IsGoing] BIT NULL DEFAULT(0), 
    [Email] NVARCHAR(256) NULL, 
    [Phone] VARCHAR(16) NULL,
	[Address] VARCHAR(512) NULL
)
