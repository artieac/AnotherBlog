USE [AMForwardDb]
GO

INSERT INTO DbInfo(Version) VALUES (1);
INSERT INTO Users(UserName, Password, Email, IsSiteAdministrator) VALUES ('ABAdmin', '5F4DCC3B5AA765D61D8327DEB882CF99', 'acorrea@alwaysmoveforward.com', 1);
INSERT INTO Users(UserName, Password, Email, IsSiteAdministrator) VALUES ('Guest', '5F4DCC3B5AA765D61D8327DEB882CF99', 'guest@alwaysmoveforward.com', 0);
INSERT INTO Roles(Name) VALUES ('Administrator');
INSERT INTO Roles(Name) VALUES ('Blogger');
INSERT INTO Roles(Name) VALUES ('Reader');
