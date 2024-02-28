USE [GROUPWAREDB]
GO

/****** Object: Table [dbo].[T_STAFFM] Script Date: 1/17/2024 6:30:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[T_STAFFM];


GO
CREATE TABLE [dbo].[T_STAFFM] (
    [staf_cd]    INT           NOT NULL,
    [password]   VARCHAR (20)  NULL,
    [staf_name]  NVARCHAR (10) NULL,
    [auth_admin] INT           DEFAULT ((0)) NOT NULL,
    [workflow_auth] INT           DEFAULT ((0)) NOT NULL,
    [mail]       VARCHAR (50)  NOT NULL,
    [update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL,
CONSTRAINT [PK_T_STAFFM] PRIMARY KEY CLUSTERED 
(
	[staf_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

