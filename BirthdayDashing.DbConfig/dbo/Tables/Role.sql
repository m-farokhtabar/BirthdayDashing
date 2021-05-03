CREATE TABLE [dbo].[Role] (
    [Id]         UNIQUEIDENTIFIER NOT NULL DEFAULT newID(),
    [Name]       NVARCHAR (50)    NOT NULL,
    [RowVersion] ROWVERSION       NOT NULL ,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Role_Unique_Name]
    ON [dbo].[Role]([Name] ASC);

