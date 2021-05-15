CREATE TABLE [dbo].[User] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Email]            NVARCHAR (100)   NOT NULL,
    [Password]         VARCHAR (64)     NOT NULL,
    [Birthday]         DATETIME2 (7)    NOT NULL,
    [PostalCode]       NVARCHAR (20)    NOT NULL,
    [FirstName]        NVARCHAR (50)    NULL,
    [LastName]         NVARCHAR (50)    NULL,
    [PhoneNumber]      VARCHAR (50)     NULL,
    [imageUrl]         NVARCHAR (2048)  NULL,
    [IsApproved]       BIT              NOT NULL,
    [LastLoginDate]    DATETIME2 (7)    NULL,
    [LockOutThreshold] TINYINT          NOT NULL,
    [RowVersion]       ROWVERSION       NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);












GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Unique_Email]
    ON [dbo].[User]([Email] ASC);

