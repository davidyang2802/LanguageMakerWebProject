CREATE TABLE [dbo].[Words]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Text] NCHAR(50) NULL, 
    [LanguageId] INT NULL, 
    [ClassificationId] INT NULL, 
    [Description] NCHAR(500) NULL, 
    [Pronounciation] NCHAR(100) NULL, 
    CONSTRAINT [FK_Words_ToLanguages] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id]), 
    CONSTRAINT [FK_Words_ToClassifications] FOREIGN KEY ([ClassificationId]) REFERENCES [Classifications]([Id])
)
