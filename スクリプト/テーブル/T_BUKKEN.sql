use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_BUKKEN]') AND type in (N'U'))
DROP TABLE [dbo].[T_BUKKEN]
GO

CREATE TABLE [dbo].[T_BUKKEN](
	[bukken_cd] [int] NOT NULL,
	[bukken_name] [nvarchar](60) NULL,
	[zip] [nvarchar](8) NULL,
	[address1] [nvarchar](40) NULL,
	[address2] [nvarchar](40) NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_T_BUKKEN_1] PRIMARY KEY CLUSTERED 
(
	[bukken_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
