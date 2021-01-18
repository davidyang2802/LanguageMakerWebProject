CREATE TABLE [dbo].[WordPatterns]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(30) NULL, 
    [Pattern] NCHAR(500) NULL, 
    [LanguageId] INT NULL, 
    CONSTRAINT [FK_WordPatterns_ToLanguages] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id])
)
