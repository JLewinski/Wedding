CREATE TABLE [dbo].[Guests] (
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [NumberAdults]   TINYINT          NOT NULL,
    [NumberChildren] TINYINT          NOT NULL,
    [IsGoing]        BIT              NULL,
    [GuestName]      NVARCHAR (128)   NOT NULL,
    [Email]          NVARCHAR (128)   NOT NULL,
    [PhoneNumber]    NVARCHAR (128)   NOT NULL,
    [GuestId]        INT              IDENTITY (1, 1) NOT NULL,
    [DateModified]   DATETIME         NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [IX_Guests] UNIQUE NONCLUSTERED ([UserId] ASC)
);

