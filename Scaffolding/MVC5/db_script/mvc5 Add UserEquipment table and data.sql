USE [MVC5]
GO
/****** Object:  Table [dbo].[UserEquipment]    Script Date: 10/06/2014 12:25:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserEquipment](
	[Id] [uniqueidentifier] NOT NULL,
	[EquipmentName] [varchar](50) NOT NULL,
	[Price] [money] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserEquipment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[UserEquipment] ([Id], [EquipmentName], [Price], [UserId]) VALUES (N'97019a6e-cfeb-4b3e-8f15-837372bca58d', N'Laptop', 1200.0000, N'459e6924-e67c-4dc8-8471-3176d912694e')
INSERT [dbo].[UserEquipment] ([Id], [EquipmentName], [Price], [UserId]) VALUES (N'e0bcde2c-f184-4be0-99f3-e11f295afa96', N'Nexus 5', 300.0000, N'459e6924-e67c-4dc8-8471-3176d912694e')
/****** Object:  ForeignKey [FK_UserEquipment_User]    Script Date: 10/06/2014 12:25:23 ******/
ALTER TABLE [dbo].[UserEquipment]  WITH CHECK ADD  CONSTRAINT [FK_UserEquipment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserEquipment] CHECK CONSTRAINT [FK_UserEquipment_User]
GO
