use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_REPORTCOMMENT]') AND type in (N'U'))
DROP TABLE [dbo].[T_REPORTCOMMENT]
GO

CREATE TABLE [dbo].[T_REPORTCOMMENT](
	[report_no] [int] NOT NULL,
	[comment_no] [int] NOT NULL,
	[message] [nvarchar](1000) NULL,
	[alreadyread_flg] [bit] NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_T_REPORTCOMMENT] PRIMARY KEY CLUSTERED 
(
	[report_no] ASC,
	[comment_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[T_REPORTCOMMENT] ADD  CONSTRAINT [DF_T_REPORTCOMMENT_alreadyread_flg]  DEFAULT ((0)) FOR [alreadyread_flg]
GO
