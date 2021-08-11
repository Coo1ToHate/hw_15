drop table [dbo].[Departments]
drop table [dbo].[Clients]
drop table [dbo].[BankAccounts]
drop table [dbo].[DepositAccounts]
drop table [dbo].[Credits]

CREATE TABLE [dbo].[Departments] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Clients] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [DepartmentId] INT           NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[BankAccounts] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [ClientId] INT             NOT NULL,
    [Amount]   DECIMAL (12, 4) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[DepositAccounts] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [ClientId]       INT             NOT NULL,
    [Amount]         DECIMAL (12, 4) NOT NULL,
    [Percent]        DECIMAL (5, 2)  NOT NULL,
    [Capitalization] BIT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Credits] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [ClientId] INT             NOT NULL,
    [Amount]   DECIMAL (12, 4) NOT NULL,
    [Percent]  DECIMAL (5, 2)  NOT NULL,
    [Period]   INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

SET IDENTITY_INSERT [dbo].[Departments] ON
INSERT INTO [dbo].[Departments] ([id], [Name]) VALUES (1, N'Физические лица')
INSERT INTO [dbo].[Departments] ([id], [Name]) VALUES (2, N'Vip клиенты')
INSERT INTO [dbo].[Departments] ([id], [Name]) VALUES (3, N'Юридические лица')
SET IDENTITY_INSERT [dbo].[Departments] OFF

SET IDENTITY_INSERT [dbo].[Clients] ON
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (1, 1, N'Имя1')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (2, 2, N'Имя2')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (3, 3, N'Имя3')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (4, 1, N'Имя4')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (5, 2, N'Имя5')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (6, 3, N'Имя6')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (7, 1, N'Имя7')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (8, 2, N'Имя8')
INSERT INTO [dbo].[Clients] ([id], [DepartmentId], [Name]) VALUES (9, 3, N'Имя9')
SET IDENTITY_INSERT [dbo].[Clients] OFF

SET IDENTITY_INSERT [dbo].[BankAccounts] ON
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (1, 1, 1000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (2, 2, 2000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (3, 3, 3000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (4, 4, 3000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (5, 5, 4000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (6, 6, 5000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (7, 7, 6000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (8, 8, 7000)
INSERT INTO [dbo].[BankAccounts] ([id], [ClientId], [Amount]) VALUES (9, 9, 8000)
SET IDENTITY_INSERT [dbo].[BankAccounts] OFF

SET IDENTITY_INSERT [dbo].[DepositAccounts] ON
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (1, 1, 10000, 5, 0)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (2, 2, 20000, 8, 1)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (3, 3, 30000, 7, 0)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (4, 4, 40000, 5.5, 1)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (5, 5, 50000, 11.1, 0)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (6, 6, 40000, 4, 1)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (7, 7, 30000, 6, 0)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (8, 8, 20000, 8.2, 1)
INSERT INTO [dbo].[DepositAccounts] ([id], [ClientId], [Amount], [Percent], [Capitalization]) VALUES (9, 9, 10000, 9, 0)
SET IDENTITY_INSERT [dbo].[DepositAccounts] OFF

SET IDENTITY_INSERT [dbo].[Credits] ON
INSERT INTO [dbo].[Credits] ([id], [ClientId], [Amount], [Percent], [Period]) VALUES (1, 1, 1000000, 5.5, 24)
INSERT INTO [dbo].[Credits] ([id], [ClientId], [Amount], [Percent], [Period]) VALUES (2, 3, 1000000, 9.1, 60)
INSERT INTO [dbo].[Credits] ([id], [ClientId], [Amount], [Percent], [Period]) VALUES (3, 6, 1000000, 6.2, 48)
INSERT INTO [dbo].[Credits] ([id], [ClientId], [Amount], [Percent], [Period]) VALUES (4, 9, 1000000, 7.5, 36)
SET IDENTITY_INSERT [dbo].[Credits] OFF