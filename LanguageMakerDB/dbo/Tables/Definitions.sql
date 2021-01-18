﻿CREATE TABLE [dbo].[Definitions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Text] NCHAR(500) NULL, 
    [WordId] INT NULL, 
    CONSTRAINT [FK_Definitions_ToWords] FOREIGN KEY ([WordId]) REFERENCES [Words]([Id])
)
