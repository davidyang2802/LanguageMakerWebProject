CREATE TABLE [dbo].[LetterTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(30) NULL, 
    [Description] NCHAR(50) NULL, 
    [LanguageId] INT NULL, 
    CONSTRAINT [FK_LetterTypes_ToLetters] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id])
)
