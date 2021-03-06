﻿CREATE TABLE [dbo].[Classifications]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(30) NULL, 
    [LanguageId] INT NULL, 
    [Description] NCHAR(500) NULL, 
    CONSTRAINT [FK_Classifications_ToLanguages] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id])
)
