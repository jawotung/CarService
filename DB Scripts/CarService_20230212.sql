USE [ÇarService]
GO
/****** Object:  Table [dbo].[mCustomer_ServicesPhoto]    Script Date: 12/02/2023 3:06:37 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mCustomer_ServicesPhoto](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileType] [nvarchar](5) NOT NULL,
	[Filename] [nvarchar](255) NOT NULL,
	[File] [varbinary](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateID] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateID] [int] NOT NULL,
	[IsDeleted] [int] NOT NULL,
 CONSTRAINT [PK_mCustomer_ServicesPhoto] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
