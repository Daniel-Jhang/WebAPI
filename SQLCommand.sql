-- Scaffold-DbContext "Server={Server_Name};Database={Database_Name};Trusted_Connection=True; {TrustServerCertificate=true;}" Microsoft.EntityFrameworkCore.SqlServer {-Context {Context_Name}} {-ContextDir {Folder_Nmae}} -OutputDir Models -Force -Project {Project_Name} -StartupProject {StartupProject_Name}

-- �H�U�O�z���Ѫ��R�O�������G
-- Server={Server_Name}: ���w SQL Server �����A���W�٩M��ҦW�١C
-- Database={Database_Name}: ���w��Ʈw�W�١C
-- Trusted_Connection=True: �ϥ� Windows ���Ҩӫإ߸�Ʈw�s�u�C
-- TrustServerCertificate=true: ��ĳ�����ñ���Ҫ����A���C
-- Microsoft.EntityFrameworkCore.SqlServer: ��Ʈw���Ѫ̡C
-- -OutputDir Models: �N�ͦ����������O��X�� Models �ؿ����C
-- -Force: �j��s�ͦ��������O�A�л\�{�������C
-- -Project {Project_Name}: ���w {Project_Name} �M�סC
-- -StartupProject {StartupProject_Name}: ���w {StartupProject_Name} �M�ק@���ҰʱM�סA�T�O DbContext ���s���]�w���T�C

CREATE DataBase LAB;

USE [LAB]
GO

--CREATE TABLE [LAB].[dbo].[TodoList](
--	[sqlId] [int] IDENTITY(1,1),
--	[TodoId] [uniqueidentifier] PRIMARY KEY,
--	[Status] [bit] NOT NULL,
--	[Editing] [bit] NOT NULL,
--	[Context] [varchar](255) NOT NULL,
--	)
--GO