use[GROUPWAREDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_DIC]') AND type in (N'U'))
DROP TABLE [dbo].[T_DIC]
GO

CREATE TABLE [dbo].[T_DIC](
	[dic_kb] [int] NOT NULL,
	[dic_cd] [nvarchar](20) NOT NULL,
	[content] [nvarchar](4000) NOT NULL,
	[comment] [nvarchar](100) NULL,
	[update_user] [varchar](10) NOT NULL,
	[update_date] [datetime2] NOT NULL,
 CONSTRAINT [PK_T_DIC] PRIMARY KEY CLUSTERED 
(
	[dic_kb] ASC,
	[dic_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into T_DIC(dic_kb,dic_cd,content,comment,update_user,update_date) values(700,'1','\\ast-dbsv01\share\アスタ業務用\社内連絡','グループウェア用：社内連絡：ファイル保存rootパス','system',SYSDATETIME());
insert into T_DIC(dic_kb,dic_cd,content,comment,update_user,update_date) values(700,'2','\\ast-dbsv01\share\アスタ業務用\日報','グループウェア用：日報：ファイル保存rootパス','system',SYSDATETIME());
insert into T_DIC(dic_kb,dic_cd,content,comment,update_user,update_date) values(710,'1','折返し電話ください','グループウェア用：伝言・電話メモ：用件','system',SYSDATETIME());
insert into T_DIC(dic_kb,dic_cd,content,comment,update_user,update_date) values(710,'2','また電話します','グループウェア用：伝言・電話メモ：用件','system',SYSDATETIME());
insert into T_DIC(dic_kb,dic_cd,content,comment,update_user,update_date) values(710,'3','連絡があったことお伝えください','グループウェア用：伝言・電話メモ：用件','system',SYSDATETIME());
insert into T_DIC(dic_kb,dic_cd,content,comment,update_user,update_date) values(710,'4','伝言を残します','グループウェア用：伝言・電話メモ：用件','system',SYSDATETIME());
