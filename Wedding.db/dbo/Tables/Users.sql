CREATE TABLE [dbo].[Users] (
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [UserName]       NVARCHAR (128)   NOT NULL,
    [HashedPassword] NVARCHAR (MAX)   NOT NULL,
    [IsAdmin]        BIT              CONSTRAINT [DF_Users_IsAdmin] DEFAULT ((0)) NOT NULL,
    [DateCreated]    DATETIME         NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

