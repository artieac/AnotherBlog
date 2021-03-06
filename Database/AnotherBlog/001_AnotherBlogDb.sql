USE [master]
GO
USE [AMForwardDb]
GO
/****** Object:  Table [dbo].[SiteInfo]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteInfo](
	[About] [text] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](50) NOT NULL,
	[ContactEmail] [nvarchar](50) NULL,
	[SiteId] [int] NOT NULL,
	[DefaultTheme] [nvarchar](50) NULL,
	[SiteAnalyticsId] [nvarchar](12) NULL,
 CONSTRAINT [PK_SiteInfo] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollQuestions]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollQuestions](
	[PollQuestionId] [int] IDENTITY(1,1) NOT NULL,
	[QuestionText] [nvarchar](512) NOT NULL,
	[Title] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_PollQuestions] PRIMARY KEY CLUSTERED 
(
	[PollQuestionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollOptions]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollOptions](
	[PollOptionId] [int] IDENTITY(1,1) NOT NULL,
	[OptionText] [nvarchar](512) NOT NULL,
	[PollQuestionId] [int] NOT NULL,
 CONSTRAINT [PK_PollOptions] PRIMARY KEY CLUSTERED 
(
	[PollOptionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointsSpent]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointsSpent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PointEarnerId] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[DateSpent] [datetime] NOT NULL,
	[Amount] [float] NOT NULL,
 CONSTRAINT [PK_PointsSpent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointEarners]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointEarners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[AdministratorId] [int] NOT NULL,
	[PointsEarned] [float] NOT NULL,
	[PointsSpent] [float] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PointEarners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogLists]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BlogId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ShowOrdered] [bit] NOT NULL,
 CONSTRAINT [BlogLists_PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogListItems]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogListItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[RelatedLink] [nvarchar](255) NULL,
	[DisplayOrder] [int] NOT NULL,
	[BlogListId] [int] NULL,
 CONSTRAINT [BlogListItems_PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogExtensions]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogExtensions](
	[AssemblyName] [nvarchar](255) NOT NULL,
	[PageLocation] [int] NOT NULL,
	[ExtensionId] [int] IDENTITY(1,1) NOT NULL,
	[SectionOrder] [int] NOT NULL,
	[ClassName] [nvarchar](100) NOT NULL,
	[AssemblyPath] [nvarchar](1024) NULL,
 CONSTRAINT [BlogExtensions_PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ExtensionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blogs]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blogs](
	[BlogId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [text] NOT NULL,
	[SubFolder] [nvarchar](50) NOT NULL,
	[About] [text] NULL,
	[WelcomeMessage] [text] NULL,
	[ContactEmail] [nvarchar](50) NULL,
	[Theme] [nvarchar](50) NULL,
	[CurrentPollId] [int] NULL,
 CONSTRAINT [PK_Blogs] PRIMARY KEY CLUSTERED 
(
	[BlogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbInfo]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbInfo](
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_DBInfo] PRIMARY KEY CLUSTERED 
(
	[Version] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChartTasks]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChartTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChartId] [int] NOT NULL,
	[TaskId] [int] NOT NULL,
 CONSTRAINT [PK_ChartTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChartTaskInstances]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChartTaskInstances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChartId] [int] NOT NULL,
	[TaskId] [int] NOT NULL,
	[DateCompleted] [datetime] NOT NULL,
	[NumberOfTimesCompleted] [int] NOT NULL,
	[AdministratorId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Charts]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Charts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[AdministratorId] [int] NOT NULL,
	[PointsEarned] [float] NOT NULL,
	[PointsSpent] [float] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[PointEarnerId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Charts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VoterAddresses]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VoterAddresses](
	[VoterAddressId] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](15) NOT NULL,
	[PollOptionId] [int] NOT NULL,
 CONSTRAINT [PK_VoterAddresses] PRIMARY KEY CLUSTERED 
(
	[VoterAddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[ApprovedCommenter] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsSiteAdministrator] [bit] NOT NULL,
	[About] [text] NULL,
	[DisplayName] [nvarchar](100) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_UserName] ON [dbo].[Users] 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Points] [float] NOT NULL,
	[MaxAllowedDaily] [int] NOT NULL,
	[AdministratorId] [int] NOT NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogEntries]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogEntries](
	[EntryId] [int] IDENTITY(1,1) NOT NULL,
	[EntryText] [text] NOT NULL,
	[BlogId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[IsPublished] [bit] NOT NULL,
	[DatePosted] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[TimesViewed] [int] NOT NULL,
 CONSTRAINT [PK_BlogEntries] PRIMARY KEY CLUSTERED 
(
	[EntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogEntries] ON [dbo].[BlogEntries] 
(
	[BlogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 09/15/2014 08:05:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[name] [nvarchar](50) NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	[BlogId] [int] NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Tags_BlogId] ON [dbo].[Tags] 
(
	[BlogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogUsers]    Script Date: 09/15/2014 08:05:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogUsers](
	[BlogId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[BlogUserId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_BlogUsers] PRIMARY KEY NONCLUSTERED 
(
	[BlogUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogUsers_BlogId] ON [dbo].[BlogUsers] 
(
	[BlogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogUsers_UserId] ON [dbo].[BlogUsers] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogRollLinks]    Script Date: 09/15/2014 08:05:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogRollLinks](
	[LinkName] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](50) NOT NULL,
	[BlogId] [int] NOT NULL,
	[BlogRollLinkId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_BlogRollLinks] PRIMARY KEY NONCLUSTERED 
(
	[BlogRollLinkId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogRollLinks] ON [dbo].[BlogRollLinks] 
(
	[BlogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntryComments]    Script Date: 09/15/2014 08:05:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntryComments](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[EntryId] [int] NOT NULL,
	[AuthorEmail] [nvarchar](50) NOT NULL,
	[RelatedComment] [int] NULL,
	[Comment] [text] NOT NULL,
	[AuthorName] [nvarchar](50) NOT NULL,
	[Status] [int] NOT NULL,
	[Link] [nvarchar](100) NULL,
	[DatePosted] [datetime] NOT NULL,
 CONSTRAINT [PK_EntryComments] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_EntryComments_EntryId] ON [dbo].[EntryComments] 
(
	[EntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogEntryTags]    Script Date: 09/15/2014 08:05:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogEntryTags](
	[TagId] [int] NOT NULL,
	[BlogEntryId] [int] NOT NULL,
	[BlogEntryTagId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_BlogEntryTags] PRIMARY KEY NONCLUSTERED 
(
	[BlogEntryTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogEntryTags] ON [dbo].[BlogEntryTags] 
(
	[BlogEntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Default [DF_Blogs_CurrentPollId]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[Blogs] ADD  CONSTRAINT [DF_Blogs_CurrentPollId]  DEFAULT ((-1)) FOR [CurrentPollId]
GO
/****** Object:  Default [DF_Users_ApprovedCommenter]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_ApprovedCommenter]  DEFAULT ((0)) FOR [ApprovedCommenter]
GO
/****** Object:  Default [DF_Users_IsActive]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Users_IsSiteAdministrator]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsSiteAdministrator]  DEFAULT ((0)) FOR [IsSiteAdministrator]
GO
/****** Object:  Default [DF_BlogEntries_IsPublished]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries] ADD  CONSTRAINT [DF_BlogEntries_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
GO
/****** Object:  Default [DF_BlogEntries_DatePosted]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries] ADD  CONSTRAINT [DF_BlogEntries_DatePosted]  DEFAULT (((1)/(1))/(2000)) FOR [DatePosted]
GO
/****** Object:  Default [[dbo]].[BlogEntries]]DateCreatedDefault]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries] ADD  CONSTRAINT [[dbo]].[BlogEntries]]DateCreatedDefault]  DEFAULT (((1)/(1))/(2009)) FOR [DateCreated]
GO
/****** Object:  Default [[dbo]].[BlogEntries]]TimesViewedDefault]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries] ADD  CONSTRAINT [[dbo]].[BlogEntries]]TimesViewedDefault]  DEFAULT ((0)) FOR [TimesViewed]
GO
/****** Object:  Default [DF_BlogUsers_RoleId]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers] ADD  CONSTRAINT [DF_BlogUsers_RoleId]  DEFAULT ((3)) FOR [RoleId]
GO
/****** Object:  Default [DF_EntryComments_Approved]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[EntryComments] ADD  CONSTRAINT [DF_EntryComments_Approved]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_EntryComments_DatePosted]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[EntryComments] ADD  CONSTRAINT [DF_EntryComments_DatePosted]  DEFAULT (((1)/(1))/(2000)) FOR [DatePosted]
GO
/****** Object:  ForeignKey [FK_BlogEntries_Blogs]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntries_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([BlogId])
GO
ALTER TABLE [dbo].[BlogEntries] CHECK CONSTRAINT [FK_BlogEntries_Blogs]
GO
/****** Object:  ForeignKey [FK_BlogEntries_Users]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntries_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[BlogEntries] CHECK CONSTRAINT [FK_BlogEntries_Users]
GO
/****** Object:  ForeignKey [FK_Tags_Blogs]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[Tags]  WITH CHECK ADD  CONSTRAINT [FK_Tags_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([BlogId])
GO
ALTER TABLE [dbo].[Tags] CHECK CONSTRAINT [FK_Tags_Blogs]
GO
/****** Object:  ForeignKey [FK_BlogUsers_Blogs]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlogUsers_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([BlogId])
GO
ALTER TABLE [dbo].[BlogUsers] CHECK CONSTRAINT [FK_BlogUsers_Blogs]
GO
/****** Object:  ForeignKey [FK_BlogUsers_Roles]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlogUsers_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[BlogUsers] CHECK CONSTRAINT [FK_BlogUsers_Roles]
GO
/****** Object:  ForeignKey [FK_BlogUsers_Users]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlogUsers_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[BlogUsers] CHECK CONSTRAINT [FK_BlogUsers_Users]
GO
/****** Object:  ForeignKey [FK_BlogLinks_Blogs]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogRollLinks]  WITH CHECK ADD  CONSTRAINT [FK_BlogLinks_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([BlogId])
GO
ALTER TABLE [dbo].[BlogRollLinks] CHECK CONSTRAINT [FK_BlogLinks_Blogs]
GO
/****** Object:  ForeignKey [FK_EntryComments_BlogEntries]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[EntryComments]  WITH CHECK ADD  CONSTRAINT [FK_EntryComments_BlogEntries] FOREIGN KEY([EntryId])
REFERENCES [dbo].[BlogEntries] ([EntryId])
GO
ALTER TABLE [dbo].[EntryComments] CHECK CONSTRAINT [FK_EntryComments_BlogEntries]
GO
/****** Object:  ForeignKey [FK_BlogEntryTags_BlogEntries]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogEntryTags]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntryTags_BlogEntries] FOREIGN KEY([BlogEntryId])
REFERENCES [dbo].[BlogEntries] ([EntryId])
GO
ALTER TABLE [dbo].[BlogEntryTags] CHECK CONSTRAINT [FK_BlogEntryTags_BlogEntries]
GO
/****** Object:  ForeignKey [FK_BlogEntryTags_Tags]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogEntryTags]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntryTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([id])
GO
ALTER TABLE [dbo].[BlogEntryTags] CHECK CONSTRAINT [FK_BlogEntryTags_Tags]
GO
INSERT INTO [dbo].[Consumers] (ConsumerKey, ConsumerSecret, PublicKey, Name, ContactEmail, AutoGrant, AccessTokenLifetime) 
	VALUES ('708DB394-9CFD-4998-B3B4-4FF94830D505', 'EF4E7875-66F6-47CC-AFC9-7F49C5097ED1', '', 'AlwaysMoveForward Blog', 'artieac@gmail.com', 0, '7800')	
GO