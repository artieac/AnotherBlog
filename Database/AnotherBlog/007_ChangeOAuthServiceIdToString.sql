ALTER TABLE [dbo].[Users] ALTER COLUMN OAuthServiceUserId nvarchar(50) NULL;
GO
ALTER TABLE [dbo].[Users] ADD Email nvarchar(50) NULL;
GO

