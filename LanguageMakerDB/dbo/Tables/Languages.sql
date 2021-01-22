CREATE TABLE [dbo].[Languages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(30) NULL, 
    [UserId] INT NULL, 
    [Description] NCHAR(500) NULL, 
    CONSTRAINT [FK_Languages_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
