use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_WORKFLOW]') AND type in (N'U'))
DROP TABLE [dbo].[T_WORKFLOW]
GO

CREATE TABLE [dbo].[T_WORKFLOW](
	[id] [int] NOT NULL,
	[title] [nvarchar](64) NOT NULL,
	[description] [nvarchar](64) NOT NULL,
	[filename] [nvarchar](64) NULL,
	[icon] [nvarchar](10) NOT NULL,
	[size] [int] NOT NULL,
	[type] [int] NOT NULL,
	[update_user] [varchar](10) NOT NULL,
	[manager] [varchar](10) NULL,
	[approver] [varchar](10) NULL,
	[manager_status] [int] NOT NULL,
	[approver_status] [int] NOT NULL,
	[comment] [nvarchar](64) NULL,
	[update_date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_T_WORKFLOW] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
