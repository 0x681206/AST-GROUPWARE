use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_MEMO]') AND type in (N'U'))
DROP TABLE [dbo].[T_MEMO]
GO

CREATE TABLE [dbo].[T_MEMO](
	[memo_no] [int] NOT NULL,
	[state] [int] NOT NULL,
	[receiver_type] [int] NOT NULL,
	[receiver_cd] [int] NOT NULL,
	[comment_no] [varchar](10) NOT NULL,
	[phone] [varchar](20) NULL,
	[content] [nvarchar](255) NULL,
	[sender_cd] [int] NOT NULL,
	[working_cd] [int] NOT NULL,
	[working_date] [datetime2](7) NOT NULL,
	[finish_cd] [int] NOT NULL,
	[finish_date] [datetime2](7) NOT NULL,
	[create_date] [datetime2](7) NOT NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL
 CONSTRAINT [PK_T_MEMO] PRIMARY KEY CLUSTERED 
(
	[memo_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

