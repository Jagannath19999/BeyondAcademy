USE [master]
GO
/****** Object:  Database [BeyondAcademy]    Script Date: 11/7/2024 1:44:42 AM ******/
CREATE DATABASE [BeyondAcademy]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BeyondAcademy', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BeyondAcademy.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BeyondAcademy_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\BeyondAcademy_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BeyondAcademy] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BeyondAcademy].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BeyondAcademy] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BeyondAcademy] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BeyondAcademy] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BeyondAcademy] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BeyondAcademy] SET ARITHABORT OFF 
GO
ALTER DATABASE [BeyondAcademy] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BeyondAcademy] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BeyondAcademy] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BeyondAcademy] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BeyondAcademy] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BeyondAcademy] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BeyondAcademy] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BeyondAcademy] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BeyondAcademy] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BeyondAcademy] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BeyondAcademy] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BeyondAcademy] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BeyondAcademy] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BeyondAcademy] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BeyondAcademy] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BeyondAcademy] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BeyondAcademy] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BeyondAcademy] SET RECOVERY FULL 
GO
ALTER DATABASE [BeyondAcademy] SET  MULTI_USER 
GO
ALTER DATABASE [BeyondAcademy] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BeyondAcademy] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BeyondAcademy] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BeyondAcademy] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BeyondAcademy] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BeyondAcademy] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BeyondAcademy', N'ON'
GO
ALTER DATABASE [BeyondAcademy] SET QUERY_STORE = ON
GO
ALTER DATABASE [BeyondAcademy] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BeyondAcademy]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 11/7/2024 1:44:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AcId] [uniqueidentifier] NOT NULL,
	[RegdId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[UserId] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK_Account_7EFCCBA73CA59D8E] PRIMARY KEY CLUSTERED 
(
	[AcId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountRole]    Script Date: 11/7/2024 1:44:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountRole](
	[ARId] [uniqueidentifier] NOT NULL,
	[AcId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ARId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registration]    Script Date: 11/7/2024 1:44:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registration](
	[RegdId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](12) NOT NULL,
	[MobileNo] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[DateOfBirth] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK_Registra_D2B0D7AF4397252C] PRIMARY KEY CLUSTERED 
(
	[RegdId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/7/2024 1:44:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([AcId], [RegdId], [Name], [Email], [UserId], [Password], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'f0144766-4aab-4d71-9b84-57a1f8752097', N'8a711f7b-2254-443e-8fea-3ba1302315b8', N'Khohula', N'rhaj@gmail.com', N'student', N'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'Student', CAST(N'2024-07-10T01:12:38.730' AS DateTime), N'Khohula')
INSERT [dbo].[Account] ([AcId], [RegdId], [Name], [Email], [UserId], [Password], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'3b62c4ea-4981-4891-b93b-ab30a6859945', N'203adb31-02c1-4a39-9048-dd4e91e7433e', N'Admin', N'ganeshwaran@gmail.com', N'admin', N'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'System', NULL, NULL)
INSERT [dbo].[Account] ([AcId], [RegdId], [Name], [Email], [UserId], [Password], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'2ebeff24-cf90-43d6-afc3-c9146c51a7a7', N'203adb31-02c1-4a39-9048-dd4e91e7444f', N'John', N'email@gmail.com', N'teacher', N'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'Admin', NULL, NULL)
INSERT [dbo].[Account] ([AcId], [RegdId], [Name], [Email], [UserId], [Password], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'88bf6e7d-be82-4300-b69f-eb714ad63855', N'60bd2658-5203-4097-ac10-ad21a61129a1', N'James', N'jamessmith@gmail.com', N'jamessmith6789', N'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', 1, CAST(N'2024-07-08T22:22:06.163' AS DateTime), N'Student', CAST(N'2024-07-09T23:52:22.613' AS DateTime), N'James')
GO
INSERT [dbo].[AccountRole] ([ARId], [AcId], [RoleId], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'8a2f8523-e3f9-4461-9a40-5a76827dd6d7', N'3b62c4ea-4981-4891-b93b-ab30a6859945', N'0c88ea54-f1d2-47ac-b8da-3b9261abd7cc', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'System', NULL, NULL)
INSERT [dbo].[AccountRole] ([ARId], [AcId], [RoleId], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'2a62c670-975f-443f-a9f6-77cc4c0b5b8a', N'2ebeff24-cf90-43d6-afc3-c9146c51a7a7', N'0c88ea54-f1d2-47ac-b8da-3b9261abd7dd', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'Admin', NULL, NULL)
INSERT [dbo].[AccountRole] ([ARId], [AcId], [RoleId], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'473f42a2-9526-40b7-a902-e8ff7f1f7649', N'f0144766-4aab-4d71-9b84-57a1f8752097', N'b2c80f03-2bea-41d7-a637-d369cdf5a18d', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'Student', NULL, NULL)
INSERT [dbo].[AccountRole] ([ARId], [AcId], [RoleId], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'3d5b3778-8f15-43c8-9151-f5cbc1728f3c', N'88bf6e7d-be82-4300-b69f-eb714ad63855', N'b2c80f03-2bea-41d7-a637-d369cdf5a18d', 1, CAST(N'2024-07-08T22:22:07.030' AS DateTime), N'Student', NULL, NULL)
GO
INSERT [dbo].[Registration] ([RegdId], [FirstName], [LastName], [MobileNo], [Email], [DateOfBirth], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'8a711f7b-2254-443e-8fea-3ba1302315b8', N'Khohula', N'Rhaj', N'01355694885', N'rhaj@gmail.com', N'1999-01-31', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'Student', NULL, NULL)
INSERT [dbo].[Registration] ([RegdId], [FirstName], [LastName], [MobileNo], [Email], [DateOfBirth], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'60bd2658-5203-4097-ac10-ad21a61129a1', N'James', N'Smith', N'0123456789', N'jamessmith@gmail.com', N'7/7/2024 12:00:00 AM', 1, CAST(N'2024-07-08T22:22:06.160' AS DateTime), N'Student', NULL, NULL)
INSERT [dbo].[Registration] ([RegdId], [FirstName], [LastName], [MobileNo], [Email], [DateOfBirth], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'203adb31-02c1-4a39-9048-dd4e91e7433e', N'Ganeswaran', N'Selvarajah', N'0123456779', N'ganeshwaran11@gmail.com', N'1999-07-31', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'System', NULL, NULL)
INSERT [dbo].[Registration] ([RegdId], [FirstName], [LastName], [MobileNo], [Email], [DateOfBirth], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'203adb31-02c1-4a39-9048-dd4e91e7444f', N'John', N'Smith', N'01025341477', N'email@gmail.com', N'1989-07-29', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'Admin', NULL, NULL)
GO
INSERT [dbo].[Role] ([RoleId], [RoleName], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'0c88ea54-f1d2-47ac-b8da-3b9261abd7cc', N'Admin', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'System', NULL, NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'0c88ea54-f1d2-47ac-b8da-3b9261abd7dd', N'Teacher', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'System', NULL, NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName], [IsActive], [CreatedOn], [CreatedBy], [Modified], [ModifiedBy]) VALUES (N'b2c80f03-2bea-41d7-a637-d369cdf5a18d', N'Student', 1, CAST(N'2024-07-03T00:00:00.000' AS DateTime), N'System', NULL, NULL)
GO
USE [master]
GO
ALTER DATABASE [BeyondAcademy] SET  READ_WRITE 
GO
