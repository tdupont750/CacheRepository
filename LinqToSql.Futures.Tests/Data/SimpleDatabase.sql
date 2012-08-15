USE [Simple]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Object:  Table [dbo].[Person] ******/
CREATE TABLE [dbo].[Person](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](500) NOT NULL,
	[LastName] [nvarchar](500) NOT NULL,
	[Age] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[Species] ******/
CREATE TABLE [dbo].[Species](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Species] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[Pet] ******/
CREATE TABLE [dbo].[Pet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[SpeciesId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Age] [int] NOT NULL,
 CONSTRAINT [PK_Pet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pet]  WITH CHECK ADD  CONSTRAINT [FK_Pet_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
GO

ALTER TABLE [dbo].[Pet] CHECK CONSTRAINT [FK_Pet_Person]
GO

ALTER TABLE [dbo].[Pet]  WITH CHECK ADD  CONSTRAINT [FK_Pet_Species] FOREIGN KEY([SpeciesId])
REFERENCES [dbo].[Species] ([Id])
GO

ALTER TABLE [dbo].[Pet] CHECK CONSTRAINT [FK_Pet_Species]
GO



/****** Insert Data ******/
INSERT INTO [Simple].[dbo].[Person] ([FirstName], [LastName] ,[Age])
VALUES ('Tom', 'DuPont', 26)
GO

INSERT INTO [Simple].[dbo].[Person] ([FirstName], [LastName] ,[Age])
VALUES ('Cat', 'Fox', 26)
GO

INSERT INTO [Simple].[dbo].[Person] ([FirstName], [LastName] ,[Age])
VALUES ('Jordan', 'Irwin', 28)
GO

INSERT INTO [Simple].[dbo].[Species] ([Name], [Description])
VALUES ('Dog', 'The dog says "woof!"')
GO

INSERT INTO [Simple].[dbo].[Species] ([Name], [Description])
VALUES ('Cat', 'The cat says "meow."')
GO

INSERT INTO [Simple].[dbo].[Pet] ([PersonId], [SpeciesId], [Name], [Age])
VALUES (1, 1, 'Taboo', 1)
GO

INSERT INTO [Simple].[dbo].[Pet] ([PersonId], [SpeciesId], [Name], [Age])
VALUES (2, 2, 'Linq', 3)
GO

INSERT INTO [Simple].[dbo].[Pet] ([PersonId], [SpeciesId], [Name], [Age])
VALUES (2, 2, 'Sql', 3)
GO

INSERT INTO [Simple].[dbo].[Pet] ([PersonId], [SpeciesId], [Name], [Age])
VALUES (3, 1, 'Zen', 4)
GO
