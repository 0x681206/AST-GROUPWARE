USE [GROUPWAREDB]
GO

/****** Object: Table [dbo].[T_SCHEDULE] Script Date: 1/19/2024 4:38:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[T_SCHEDULE];


GO
CREATE TABLE [dbo].[T_SCHEDULE] (
    [schedule_no]    INT            NOT NULL,
    [schedule_type]  INT            NOT NULL,
    [allday]         BIT            NOT NULL,
    [start_datetime] DATETIME2 (7)  NULL,
    [end_datetime]   DATETIME2 (7)  NULL,
    [title]          NVARCHAR (50)  NULL,
    [memo]           NVARCHAR (200) NULL,
    [update_user]    VARCHAR (10)   NOT NULL,
    [update_date]    DATETIME2 (7)  NOT NULL
);


