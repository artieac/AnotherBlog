USE [AlwaysMoveForward]
GO
/****** Object:  Table [dbo].[SiteInfo]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[SiteInfo] ADD CONSTRAINT [PK_SiteInfo] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT PK_Roles
GO
ALTER TABLE dbo.BlogUsers DROP CONSTRAINT FK_BlogUsers_Roles
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN RoleId
GO
ALTER TABLE [dbo].[Roles] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointsSpent]    Script Date: 09/15/2014 08:05:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[BlogLists] DROP CONSTRAINT BlogLists_PrimaryKey
GO
ALTER TABLE [dbo].[BlogLists] DROP COLUMN Id
GO
ALTER TABLE [dbo].[BlogLists] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogLists] ADD CONSTRAINT [BlogLists_PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogListItems] DROP CONSTRAINT BlogListItems_PrimaryKey
GO
ALTER TABLE [dbo].[BlogListItems] DROP COLUMN Id
GO
ALTER TABLE [dbo].[BlogListItems] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogListItems] ADD CONSTRAINT [BlogListItems_PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogExtensions] DROP CONSTRAINT BlogExtensions_PrimaryKey
GO
ALTER TABLE [dbo].[BlogExtensions] DROP COLUMN ExtensionId
GO
ALTER TABLE [dbo].[BlogExtensions] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogExtensions] ADD CONSTRAINT [BlogExtensions_PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogRollLinks] DROP CONSTRAINT FK_BlogLinks_Blogs
GO
ALTER TABLE [dbo].[BlogUsers] DROP CONSTRAINT FK_BlogUsers_Blogs
GO
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT FK_Tags_Blogs
GO
ALTER TABLE [dbo].[BlogEntries] DROP CONSTRAINT FK_BlogEntries_Blogs
GO
ALTER TABLE [dbo].[Blogs] DROP CONSTRAINT PK_Blogs
GO
ALTER TABLE [dbo].[Blogs] DROP COLUMN BlogId
GO
ALTER TABLE [dbo].[Blogs] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[Blogs] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[Blogs] ADD CONSTRAINT [PK_Blogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogUsers] DROP CONSTRAINT FK_BlogUsers_Users
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT PK_Users
GO
ALTER TABLE [dbo].[Users] DROP COLUMN UserId
GO
ALTER TABLE [dbo].[Users] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[Users] ADD [Id] [bigint] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogEntries] DROP COLUMN Id
GO
ALTER TABLE [dbo].[BlogEntryTags] DROP CONSTRAINT FK_BlogEntryTags_BlogEntries
GO
ALTER TABLE [dbo].[EntryComments] DROP CONSTRAINT FK_EntryComments_BlogEntries
GO
ALTER TABLE [dbo].[BlogEntries] DROP CONSTRAINT PK_BlogEntries
GO
ALTER TABLE [dbo].[BlogEntries] DROP COLUMN EntryId
GO
ALTER TABLE [dbo].[BlogEntries] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogEntries] ALTER COLUMN UserId [bigint] NOT NULL
GO
ALTER TABLE [dbo].[BlogEntries] ADD CONSTRAINT [PK_BlogEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
DROP INDEX IX_BlogEntries ON [dbo].[BlogEntries]
GO
CREATE NONCLUSTERED INDEX [IX_BlogEntries] ON [dbo].[BlogEntries] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
DROP INDEX [IX_Tags_BlogId] ON [dbo].[Tags]
GO
CREATE NONCLUSTERED INDEX [IX_Tags_BlogId] ON [dbo].[Tags] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogUsers]    Script Date: 09/15/2014 08:05:14 ******/
GO
ALTER TABLE [dbo].[BlogUsers] DROP CONSTRAINT PK_BlogUsers
GO
ALTER TABLE [dbo].[BlogUsers] DROP COLUMN BlogUserId
GO
ALTER TABLE [dbo].[BlogUsers] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[BlogUsers] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogUsers] ADD CONSTRAINT [PK_BlogUsers] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
ALTER TABLE [dbo].[BlogRollLinks] DROP CONSTRAINT PK_BlogRollLinks
GO
ALTER TABLE [dbo].[BlogRollLinks] DROP COLUMN BlogRollLinkId
GO
ALTER TABLE [dbo].[BlogRollLinks] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[BlogRollLinks] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogRollLinks] ADD CONSTRAINT [PK_BlogRollLinks] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogRollLinks] ON [dbo].[BlogRollLinks] 
(
	[BlogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EntryComments] DROP CONSTRAINT PK_EntryComments
GO
ALTER TABLE [dbo].[EntryComments] DROP COLUMN CommentId
GO
ALTER TABLE [dbo].[EntryComments] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[EntryComments] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[EntryComments] ADD CONSTRAINT [PK_EntryComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_EntryComments_EntryId] ON [dbo].[EntryComments] 
(
	[EntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogEntryTags] DROP CONSTRAINT [PK_BlogEntryTags]
GO
ALTER TABLE [dbo].[BlogEntryTags] DROP COLUMN BlogEntryTagId
GO
ALTER TABLE [dbo].[BlogEntryTags] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[BlogEntryTags] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[BlogEntryTags] ADD CONSTRAINT [PK_BlogEntryTags] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BlogEntryTags] ON [dbo].[BlogEntryTags] 
(
	[BlogEntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [PK_Tags]
GO
DROP INDEX IX_Tags_BlogId on [dbo].[Tags]
GO
ALTER TABLE [dbo].[Tags] DROP COLUMN [Id]
GO
ALTER TABLE [dbo].[Tags] ADD [Id] [int] IDENTITY(1,1)
GO
ALTER TABLE [dbo].[Tags] ADD CONSTRAINT [PK_Tags] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
REFERENCES [dbo].[Blogs] ([Id])
GO
ALTER TABLE [dbo].[BlogEntries] CHECK CONSTRAINT [FK_BlogEntries_Blogs]
GO
/****** Object:  ForeignKey [FK_BlogEntries_Users]    Script Date: 09/15/2014 08:05:13 ******/
ALTER TABLE [dbo].[BlogEntries]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntries_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BlogEntries] CHECK CONSTRAINT [FK_BlogEntries_Users]
GO
/****** Object:  ForeignKey [FK_Tags_Blogs]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[Tags]  WITH CHECK ADD  CONSTRAINT [FK_Tags_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
GO
ALTER TABLE [dbo].[Tags] CHECK CONSTRAINT [FK_Tags_Blogs]
GO
/****** Object:  ForeignKey [FK_BlogUsers_Blogs]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlogUsers_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
GO
ALTER TABLE [dbo].[BlogUsers] CHECK CONSTRAINT [FK_BlogUsers_Blogs]
GO
/****** Object:  ForeignKey [FK_BlogUsers_Roles]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlogUsers_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[BlogUsers] CHECK CONSTRAINT [FK_BlogUsers_Roles]
GO
/****** Object:  ForeignKey [FK_BlogUsers_Users]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlogUsers_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BlogUsers] CHECK CONSTRAINT [FK_BlogUsers_Users]
GO
/****** Object:  ForeignKey [FK_BlogLinks_Blogs]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogRollLinks]  WITH CHECK ADD  CONSTRAINT [FK_BlogLinks_Blogs] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
GO
ALTER TABLE [dbo].[BlogRollLinks] CHECK CONSTRAINT [FK_BlogLinks_Blogs]
GO
/****** Object:  ForeignKey [FK_EntryComments_BlogEntries]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[EntryComments]  WITH CHECK ADD  CONSTRAINT [FK_EntryComments_BlogEntries] FOREIGN KEY([EntryId])
REFERENCES [dbo].[BlogEntries] ([Id])
GO
ALTER TABLE [dbo].[EntryComments] CHECK CONSTRAINT [FK_EntryComments_BlogEntries]
GO
/****** Object:  ForeignKey [FK_BlogEntryTags_BlogEntries]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogEntryTags]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntryTags_BlogEntries] FOREIGN KEY([BlogEntryId])
REFERENCES [dbo].[BlogEntries] ([Id])
GO
ALTER TABLE [dbo].[BlogEntryTags] CHECK CONSTRAINT [FK_BlogEntryTags_BlogEntries]
GO
/****** Object:  ForeignKey [FK_BlogEntryTags_Tags]    Script Date: 09/15/2014 08:05:14 ******/
ALTER TABLE [dbo].[BlogEntryTags]  WITH CHECK ADD  CONSTRAINT [FK_BlogEntryTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([id])
GO
ALTER TABLE [dbo].[BlogEntryTags] CHECK CONSTRAINT [FK_BlogEntryTags_Tags]
GO
