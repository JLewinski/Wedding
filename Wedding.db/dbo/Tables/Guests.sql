--Was going to allow every guest to sign in to see and change their RSVP, but decided against it.
--I didn't want the users to have to worry about creating a username / password
--Because of this, there's not really a need for the User table to be connected to the Guest table
--I just didn't want to change other things that were already working, so I left it as is.
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

