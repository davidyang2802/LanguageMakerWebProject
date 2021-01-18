CREATE TABLE [dbo].[Languages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(30) NULL, 
    [UserId] INT NULL, 
    CONSTRAINT [FK_Languages_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
