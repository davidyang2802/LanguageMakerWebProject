CREATE TABLE [dbo].[Tags]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(30) NULL, 
    [LanguageId] INT NULL, 
    CONSTRAINT [FK_Tags_ToLanguages] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id])
)
