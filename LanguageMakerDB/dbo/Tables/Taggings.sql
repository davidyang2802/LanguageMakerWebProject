CREATE TABLE [dbo].[Taggings]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WordId] INT NULL, 
    [TagId] INT NULL, 
    CONSTRAINT [FK_Taggings_ToWords] FOREIGN KEY ([WordId]) REFERENCES [Words]([Id]), 
    CONSTRAINT [FK_Taggings_ToTags] FOREIGN KEY ([TagId]) REFERENCES [Tags]([Id])
)
