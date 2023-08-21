-- Scaffold-DbContext "Server={Server_Name};Database={Database_Name};Trusted_Connection=True; {TrustServerCertificate=true;}" Microsoft.EntityFrameworkCore.SqlServer {-Context {Context_Name}} {-ContextDir {Folder_Nmae}} -OutputDir Models -Force -Project {Project_Name} -StartupProject {StartupProject_Name}

-- 以下是您提供的命令的解釋：
-- Server={Server_Name}: 指定 SQL Server 的伺服器名稱和實例名稱。
-- Database={Database_Name}: 指定資料庫名稱。
-- Trusted_Connection=True: 使用 Windows 驗證來建立資料庫連線。
-- TrustServerCertificate=true: 建議應對自簽憑證的伺服器。
-- Microsoft.EntityFrameworkCore.SqlServer: 資料庫提供者。
-- -OutputDir Models: 將生成的實體類別輸出到 Models 目錄中。
-- -Force: 強制重新生成實體類別，覆蓋現有的文件。
-- -Project {Project_Name}: 指定 {Project_Name} 專案。
-- -StartupProject {StartupProject_Name}: 指定 {StartupProject_Name} 專案作為啟動專案，確保 DbContext 的連結設定正確。

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