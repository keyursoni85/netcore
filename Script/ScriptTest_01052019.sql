USE [Test]
GO
/****** Object:  Table [dbo].[AthleteTestMapping]    Script Date: 05/01/2019 8:48:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AthleteTestMapping](
	[MapId] [int] IDENTITY(1,1) NOT NULL,
	[AthleteTestId] [int] NULL,
	[AthleteId] [int] NULL,
	[AthleteName] [varchar](50) NULL,
	[Distance] [decimal](10, 3) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Athlete] PRIMARY KEY CLUSTERED 
(
	[MapId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Test]    Script Date: 05/01/2019 8:48:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Test](
	[TestId] [int] IDENTITY(1,1) NOT NULL,
	[TestName] [varchar](50) NULL,
	[TestTypeId] [int] NULL,
	[TestDate] [datetime] NULL,
	[TotalParticipats] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestType]    Script Date: 05/01/2019 8:48:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestType](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NULL,
 CONSTRAINT [PK_TestType] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 05/01/2019 8:48:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[UserType] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroups]    Script Date: 05/01/2019 8:48:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroups](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](50) NULL,
	[Created] [datetime] NULL,
	[Changed] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_UserGroups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AthleteTestMapping] ON 
GO
INSERT [dbo].[AthleteTestMapping] ([MapId], [AthleteTestId], [AthleteId], [AthleteName], [Distance], [IsDeleted]) VALUES (34, 19, 4, N'Delicia Ledonne', CAST(1500.000 AS Decimal(10, 3)), 0)
GO
INSERT [dbo].[AthleteTestMapping] ([MapId], [AthleteTestId], [AthleteId], [AthleteName], [Distance], [IsDeleted]) VALUES (35, 19, 3, N'Magen Faye', CAST(2000.000 AS Decimal(10, 3)), 0)
GO
INSERT [dbo].[AthleteTestMapping] ([MapId], [AthleteTestId], [AthleteId], [AthleteName], [Distance], [IsDeleted]) VALUES (36, 20, 2, N'Queen Jacobi', CAST(1000.000 AS Decimal(10, 3)), 1)
GO
INSERT [dbo].[AthleteTestMapping] ([MapId], [AthleteTestId], [AthleteId], [AthleteName], [Distance], [IsDeleted]) VALUES (37, 19, 8, N'Rosario Reuben', CAST(5656.000 AS Decimal(10, 3)), 0)
GO
INSERT [dbo].[AthleteTestMapping] ([MapId], [AthleteTestId], [AthleteId], [AthleteName], [Distance], [IsDeleted]) VALUES (38, 22, 2, N'Queen Jacobi', CAST(44.000 AS Decimal(10, 3)), 0)
GO
SET IDENTITY_INSERT [dbo].[AthleteTestMapping] OFF
GO
SET IDENTITY_INSERT [dbo].[Test] ON 
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (19, N'Cooper test', 1, CAST(N'2019-05-08T00:00:00.000' AS DateTime), 3, 0)
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (20, N'100 meter test', 2, CAST(N'2019-05-22T00:00:00.000' AS DateTime), 0, 1)
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (21, N'Cooper test', 1, CAST(N'2019-05-22T00:00:00.000' AS DateTime), 0, 1)
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (22, N'100 meter test', 2, CAST(N'2019-05-08T00:00:00.000' AS DateTime), 1, 0)
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (23, N'100 meter test', 2, CAST(N'2019-05-01T00:00:00.000' AS DateTime), 0, 1)
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (24, N'Cooper test', 1, CAST(N'2019-05-02T00:00:00.000' AS DateTime), NULL, 0)
GO
INSERT [dbo].[Test] ([TestId], [TestName], [TestTypeId], [TestDate], [TotalParticipats], [IsDeleted]) VALUES (25, N'Cooper test', 1, CAST(N'2019-05-31T00:00:00.000' AS DateTime), NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[TestType] ON 
GO
INSERT [dbo].[TestType] ([TypeId], [TypeName]) VALUES (1, N'Cooper test')
GO
INSERT [dbo].[TestType] ([TypeId], [TypeName]) VALUES (2, N'100 meter test')
GO
SET IDENTITY_INSERT [dbo].[TestType] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (1, N'Mitchel Fausto', 1, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (2, N'Queen Jacobi', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (3, N'Magen Faye', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (4, N'Delicia Ledonne', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (5, N'Marc Voth', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (6, N'Randy Rondon', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (7, N'Delora Saville', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (8, N'Rosario Reuben', 2, 0)
GO
INSERT [dbo].[User] ([UserId], [UserName], [UserType], [IsDeleted]) VALUES (9, N'Lula Uhlman', 2, 0)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserGroups] ON 
GO
INSERT [dbo].[UserGroups] ([GroupId], [GroupName], [Created], [Changed], [IsDeleted]) VALUES (1, N'Coach', CAST(N'2019-05-01T13:11:45.980' AS DateTime), NULL, 0)
GO
INSERT [dbo].[UserGroups] ([GroupId], [GroupName], [Created], [Changed], [IsDeleted]) VALUES (2, N'Athlete', CAST(N'2019-05-01T13:11:54.627' AS DateTime), NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[UserGroups] OFF
GO
