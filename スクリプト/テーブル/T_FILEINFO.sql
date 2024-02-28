use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_FILEINFO]') AND type in (N'U'))
DROP TABLE [dbo].[T_FILEINFO]
GO

CREATE TABLE [dbo].[T_FILEINFO](
	[file_no] [int] NOT NULL,
	[name] [nvarchar](64) NOT NULL,
	[icon] [nvarchar](10) NOT NULL,
	[size] [int] NOT NULL,
	[type] [int] NOT NULL,
	[path] [nvarchar](1000) NOT NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_T_FILEINFO] PRIMARY KEY CLUSTERED 
(
	[file_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
