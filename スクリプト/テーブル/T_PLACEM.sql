USE [GROUPWAREDB]
GO

/****** Object: Table [dbo].[T_PLACEM] Script Date: 1/17/2024 6:31:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[T_PLACEM];


GO
CREATE TABLE [dbo].[T_PLACEM] (
    [place_cd]   INT           NOT NULL,
    [place_name] NVARCHAR (10) NULL,
    [sort]       INT           NOT NULL
CONSTRAINT [PK_T_PLACEM] PRIMARY KEY CLUSTERED 
(
	[place_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


