CREATE TABLE [dbo].[ImageLocations]
(
  [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  [ImageSource] VARCHAR(64) NOT NULL,
  [Height] INT NOT NULL,
  [Width] INT NOT NULL
)
