use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_BUKKENCOMMENT_READ]') AND type in (N'U'))
DROP TABLE [dbo].[T_BUKKENCOMMENT_READ]
GO

CREATE TABLE [dbo].[T_BUKKENCOMMENT_READ](
	[bukken_cd] [int] NOT NULL,
	[comment_no] [int] NOT NULL,
	[staf_cd] [int] NOT NULL,
	[alreadyread_flg] [bit] NOT NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_T_BUKKENCOMMENT_READ] PRIMARY KEY CLUSTERED 
(
	[bukken_cd] ASC,
	[comment_no] ASC,
	[staf_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
