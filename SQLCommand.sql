-- EF Core PM Console Command Template
-- Scaffold-DbContext "Server={Server_Name};Database={Database_Name};Trusted_Connection=True; {TrustServerCertificate=true;}" Microsoft.EntityFrameworkCore.SqlServer {-Context {Context_Name}} {-ContextDir {Folder_Nmae}} -OutputDir Models -Force

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