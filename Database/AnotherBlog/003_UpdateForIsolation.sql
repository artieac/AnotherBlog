USE [AMForwardDb]
GO
ALTER TABLE [dbo].[Users] ADD AMFUserId bigint NULL;
GO
UPDATE [dbo].[Users] SET AMFUserId = 1 where UserId = 13;
GO
UPDATE [dbo].[Users] SET AMFUserId = -1 where UserId <> 13;
GO
ALTER TABLE [dbo].[Users] ALTER COLUMN AMFUserId int NOT NULL;
GO
ALTER TABLE [dbo].[Users] ADD FirstName nvarchar(30) NULL;
GO
UPDATE [dbo].[Users] SET FirstName = 'Arthur' where UserId = 13;
GO
UPDATE [dbo].[Users] SET FirstName = 'Foo' where UserId <> 13;
GO
ALTER TABLE [dbo].[Users] ALTER COLUMN FirstName nvarchar(30) NOT NULL;
GO
ALTER TABLE [dbo].[Users] ADD LastName nvarchar(30) NULL;
GO
UPDATE [dbo].[Users] SET LastName = 'Correa' where UserId = 13;
GO
UPDATE [dbo].[Users] SET LastName = 'Foo' where UserId <> 13;
GO
ALTER TABLE [dbo].[Users] ALTER COLUMN LastName nvarchar(30) NOT NULL;
GO
ALTER TABLE [dbo].[Users] ADD AccessToken nvarchar(36) NULL;
GO
UPDATE [dbo].[Users] SET AccessToken = '1';
GO
ALTER TABLE [dbo].[Users] ALTER COLUMN AccessToken nvarchar(36) NOT NULL;
GO
ALTER TABLE [dbo].[Users] ADD AccessTokenSecret nvarchar(36) NULL;
GO
UPDATE [dbo].[Users] SET AccessTokenSecret = '1';
GO
ALTER TABLE [dbo].[Users] ALTER COLUMN AccessTokenSecret nvarchar(36) NOT NULL;
GO
DROP INDEX IX_Users_UserName ON TABLE [dbo].[Users];
GO
ALTER TABLE [dbo].[Users] DROP COLUMN UserName;
GO
ALTER TABLE [dbo].[Users] DROP COLUMN PASSWORD;
GO
ALTER TABLE [dbo].[Users] DROP COLUMN EMAIL;
GO
ALTER TABLE [dbo].[Users] DROP COLUMN IsActive;
GO
Exec sp_rename 'Users.AMFUserId' , 'OAuthServiceUserId', 'COLUMN'
GO