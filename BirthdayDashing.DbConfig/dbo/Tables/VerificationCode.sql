CREATE TABLE [dbo].[VerificationCode] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [ExpireDate] DATETIME2 (7)    NOT NULL,
    [Token]      NVARCHAR (50)    NOT NULL,
    [Type]       SMALLINT         NOT NULL,
    CONSTRAINT [PK_VerificationCode] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VerificationCode_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_VerificationCode_UserId_Token]
    ON [dbo].[VerificationCode]([UserId] ASC, [Token] ASC);

