CREATE TABLE [dbo].[Dashing] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    [Birthday]            DATETIME2 (7)    NOT NULL,
    [Title]               NVARCHAR (256)   NOT NULL,
    [PostalCode]          NVARCHAR (20)    NOT NULL,
    [DashingAmount]       DECIMAL (18)     NULL,
    [Active]              BIT              NOT NULL,
    [Deleted]             BIT              NOT NULL,
    [LastEditById]        UNIQUEIDENTIFIER NULL,
    [LastEditDate]        DATETIME2 (7)    NULL,
    [CreatedById]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]         DATETIME2 (7)    NOT NULL,
    [BackgroundUUID]      NVARCHAR (2048)  NULL,
    [Name]                NVARCHAR (256)   NULL,
    [CurrentYearBirthday] DATETIME2 (7)    NULL,
    [City]                VARCHAR (45)     NULL,
    [State]               VARCHAR (45)     NULL,
    [TitleUpdated]        BIT              NOT NULL,
    [RowVersion]          ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Dashing] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Dashing_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Dashing_CreateDate]
    ON [dbo].[Dashing]([CreatedDate] DESC);


GO
CREATE NONCLUSTERED INDEX [IX_Dashing_UserId_Deleted_CreateDate]
    ON [dbo].[Dashing]([UserId] ASC, [Deleted] ASC, [CreatedDate] DESC);

