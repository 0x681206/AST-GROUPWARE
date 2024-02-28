USE [GROUPWAREDB]
GO

/****** Object: Table [dbo].[T_SCHEDULEFACILITY] Script Date: 2/16/2024 11:31:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SCHEDULEFACILITY] (
    [schedule_no] INT NOT NULL,
    [place_cd]    INT NOT NULL,
    [staf_cd]     INT NOT NULL
);


