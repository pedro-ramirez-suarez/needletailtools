USE [master]
GO
/****** Object:  Database [MVC5]    Script Date: 06/10/2014 10:14:15 a. m. ******/
CREATE DATABASE [MVC5]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MVC5', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\MVC5.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MVC5_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\MVC5_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MVC5] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MVC5].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MVC5] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MVC5] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MVC5] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MVC5] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MVC5] SET ARITHABORT OFF 
GO
ALTER DATABASE [MVC5] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MVC5] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MVC5] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MVC5] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MVC5] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MVC5] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MVC5] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MVC5] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MVC5] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MVC5] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MVC5] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MVC5] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MVC5] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MVC5] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MVC5] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MVC5] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MVC5] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MVC5] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MVC5] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MVC5] SET  MULTI_USER 
GO
ALTER DATABASE [MVC5] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MVC5] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MVC5] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MVC5] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [MVC5]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 06/10/2014 10:14:15 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Address](
	[Id] [uniqueidentifier] NOT NULL,
	[Street] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Phone] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department]    Script Date: 06/10/2014 10:14:15 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [uniqueidentifier] NOT NULL,
	[DepartmentName] [varchar](50) NULL,
	[ParentDepartmentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Project]    Script Date: 06/10/2014 10:14:15 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Budget] [decimal](18, 0) NULL,
	[StartDate] [date] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 06/10/2014 10:14:15 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
	[Age] [int] NULL,
	[SubscriptionDate] [date] NULL,
	[Password] [varchar](50) NULL,
	[DepartmentId] [uniqueidentifier] NULL,
	[AddressId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserProject]    Script Date: 06/10/2014 10:14:15 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProject](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[ProjectId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserProject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Address] ([Id], [Street], [ZipCode], [Phone]) VALUES (N'3c724e1a-d92b-4c8b-adaa-92c5b4fe6a25', N'Morelia, Mich.', N'58096', N'443452565')
INSERT [dbo].[Address] ([Id], [Street], [ZipCode], [Phone]) VALUES (N'9ec839b0-c291-49e7-a79c-c3f6b3fb8a7e', N'Morelia, Mich.', N'58096', N'443452565')
INSERT [dbo].[Address] ([Id], [Street], [ZipCode], [Phone]) VALUES (N'526c8afc-655b-44ca-b993-f573935e65bb', N'Moroleon, Gto.', N'38800', N'445452565')
INSERT [dbo].[Address] ([Id], [Street], [ZipCode], [Phone]) VALUES (N'93d22220-8658-4f1f-bbda-5e7e7ea1e666', N'Guadalajara', N'96096', N'495452565')
INSERT [dbo].[Department] ([Id], [DepartmentName], [ParentDepartmentId]) VALUES (N'fc6c8e62-babb-4228-86dc-8aaf6309cdcf', N'SfwDev', NULL)
INSERT [dbo].[Department] ([Id], [DepartmentName], [ParentDepartmentId]) VALUES (N'2daebab8-4f64-4aea-8847-8cdeed91cf8d', N'IT', NULL)
INSERT [dbo].[Department] ([Id], [DepartmentName], [ParentDepartmentId]) VALUES (N'89032560-d231-4d08-bf4f-b923ed8d38f4', N'QA', N'fc6c8e62-babb-4228-86dc-8aaf6309cdcf')
INSERT [dbo].[Department] ([Id], [DepartmentName], [ParentDepartmentId]) VALUES (N'028aa20e-9117-443c-96a1-f2722bb468ae', N'DEV', N'fc6c8e62-babb-4228-86dc-8aaf6309cdcf')
INSERT [dbo].[Project] ([Id], [Name], [Budget], [StartDate]) VALUES (N'122f372a-4c4d-4f4e-9104-b972dd0c7431', N'Project 1', CAST(6 AS Decimal(18, 0)), CAST(N'2014-02-02' AS Date))
INSERT [dbo].[Project] ([Id], [Name], [Budget], [StartDate]) VALUES (N'89b1377a-cef7-40ce-8f3e-5f3c61ed9f1e', N'Project 2', CAST(4 AS Decimal(18, 0)), CAST(N'2014-03-03' AS Date))
INSERT [dbo].[Project] ([Id], [Name], [Budget], [StartDate]) VALUES (N'12a96d46-3736-4be5-aaa0-5b57dde4f32c', N'Project 3', CAST(7 AS Decimal(18, 0)), CAST(N'2014-04-04' AS Date))
INSERT [dbo].[Project] ([Id], [Name], [Budget], [StartDate]) VALUES (N'134b02f1-fed4-48a6-96d6-f18326e27eb7', N'Project 4', CAST(2 AS Decimal(18, 0)), CAST(N'2014-05-05' AS Date))
INSERT [dbo].[User] ([Id], [Name], [Email], [Age], [SubscriptionDate], [Password], [DepartmentId], [AddressId]) VALUES (N'00000000-0000-0000-0000-000000000000', N'jc', N'jcreyes@sciodev.com', 45, CAST(N'6666-06-06' AS Date), N'password', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[User] ([Id], [Name], [Email], [Age], [SubscriptionDate], [Password], [DepartmentId], [AddressId]) VALUES (N'459e6924-e67c-4dc8-8471-3176d912694e', N'Luis', N'luis@sciodev.com', 30, CAST(N'2013-02-02' AS Date), N'Scio123', N'028aa20e-9117-443c-96a1-f2722bb468ae', N'3c724e1a-d92b-4c8b-adaa-92c5b4fe6a25')
INSERT [dbo].[User] ([Id], [Name], [Email], [Age], [SubscriptionDate], [Password], [DepartmentId], [AddressId]) VALUES (N'1d238deb-b8f8-4669-9a19-cdc6d08febf7', N'Beto', N'beto@sciodev.com', 45, CAST(N'2013-03-03' AS Date), N'Scio123', N'89032560-d231-4d08-bf4f-b923ed8d38f4', N'93d22220-8658-4f1f-bbda-5e7e7ea1e666')
INSERT [dbo].[User] ([Id], [Name], [Email], [Age], [SubscriptionDate], [Password], [DepartmentId], [AddressId]) VALUES (N'477029f9-0785-48ed-8884-dd3ca5212de3', N'Rosy', N'rosy@sciodev.com', 36, CAST(N'2013-04-04' AS Date), N'Scio123', N'2daebab8-4f64-4aea-8847-8cdeed91cf8d', N'526c8afc-655b-44ca-b993-f573935e65bb')
INSERT [dbo].[UserProject] ([Id], [UserId], [ProjectId]) VALUES (N'e6762a42-abef-41fc-b445-298afa444548', N'459e6924-e67c-4dc8-8471-3176d912694e', N'89b1377a-cef7-40ce-8f3e-5f3c61ed9f1e')
INSERT [dbo].[UserProject] ([Id], [UserId], [ProjectId]) VALUES (N'f9ef0b0d-0962-4136-9494-2f75d9c10794', N'1d238deb-b8f8-4669-9a19-cdc6d08febf7', N'12a96d46-3736-4be5-aaa0-5b57dde4f32c')
INSERT [dbo].[UserProject] ([Id], [UserId], [ProjectId]) VALUES (N'88d9e964-0997-4524-ace2-3aa2eff31d31', N'459e6924-e67c-4dc8-8471-3176d912694e', N'122f372a-4c4d-4f4e-9104-b972dd0c7431')
INSERT [dbo].[UserProject] ([Id], [UserId], [ProjectId]) VALUES (N'8237d1c3-ad02-40a2-8362-8f5d20e6eec6', N'477029f9-0785-48ed-8884-dd3ca5212de3', N'134b02f1-fed4-48a6-96d6-f18326e27eb7')
USE [master]
GO
ALTER DATABASE [MVC5] SET  READ_WRITE 
GO
