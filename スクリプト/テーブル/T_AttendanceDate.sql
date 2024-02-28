USE [GROUPWAREDB]
GO

/****** Object:  Table [dbo].[T_AttendanceDate]    Script Date: 1/23/2024 1:25:13 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_AttendanceDate]') AND type in (N'U'))
DROP TABLE [dbo].[T_AttendanceDate]
GO

