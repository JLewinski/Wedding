CREATE TABLE [dbo].[Notes] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [NoteText]    VARCHAR (MAX)    NOT NULL,
    [GuestId]     UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATETIME         DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

