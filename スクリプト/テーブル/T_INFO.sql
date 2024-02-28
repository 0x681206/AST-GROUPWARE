use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_INFO]') AND type in (N'U'))
DROP TABLE [dbo].[T_INFO]
GO

CREATE TABLE [dbo].[T_INFO](
	[info_cd] [int] NOT NULL,
	[title] [nvarchar](40) NULL,
	[message] [nvarchar](200) NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2](7) NOT NULL
 CONSTRAINT [PK_T_INFO_1] PRIMARY KEY CLUSTERED 
(
	[info_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into T_INFO(info_cd,update_user,update_date) values(1,'system',SYSDATETIME());
insert into T_INFO(info_cd,update_user,update_date) values(2,'system',SYSDATETIME());
