CREATE TABLE [dbo].[Comment] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NULL,
    [DashingId]    UNIQUEIDENTIFIER NOT NULL,
    [ParentId]     UNIQUEIDENTIFIER NULL,
    [Content]      NVARCHAR (1024)  NOT NULL,
    [Type]         TINYINT          NOT NULL,
    [MediaUrl]     NVARCHAR (1024)  NULL,
    [Active]       BIT              NOT NULL,
    [CreatedDate]  DATETIME2 (7)    NOT NULL,
    [CreatedById]  UNIQUEIDENTIFIER NOT NULL,
    [LastEditById] UNIQUEIDENTIFIER NULL,
    [LastEditDate] DATETIME2 (7)    NULL,
    [RowVersion]   ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Comment_Comment] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Comment] ([Id]),
    CONSTRAINT [FK_Comment_Dashing] FOREIGN KEY ([DashingId]) REFERENCES [dbo].[Dashing] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comment_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

