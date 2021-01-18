﻿CREATE TABLE [dbo].[Letters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(15) NULL, 
    [LanguageId] INT NULL, 
    [Pronounciation] NCHAR(20) NULL, 
    CONSTRAINT [FK_Letters_ToLanguages] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id]) 
)
