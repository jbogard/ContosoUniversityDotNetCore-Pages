/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4206)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  Table [dbo].[Course]    Script Date: 8/28/2017 1:27:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseID] [int] NOT NULL,
	[Credits] [int] NOT NULL,
	[Title] [nvarchar](50) NULL,
	[DepartmentID] [int] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseAssignment]    Script Date: 8/28/2017 1:27:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseAssignment](
	[CourseID] [int] NOT NULL,
	[InstructorID] [int] NOT NULL,
 CONSTRAINT [PK_CourseAssignment] PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC,
	[InstructorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 8/28/2017 1:27:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[Budget] [money] NOT NULL,
	[InstructorID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[RowVersion] [timestamp] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 8/28/2017 1:27:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[EnrollmentID] [int] IDENTITY(1,1) NOT NULL,
	[CourseID] [int] NOT NULL,
	[Grade] [int] NULL,
	[StudentID] [int] NOT NULL,
 CONSTRAINT [PK_Enrollment] PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OfficeAssignment]    Script Date: 8/28/2017 1:27:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfficeAssignment](
	[InstructorID] [int] NOT NULL,
	[Location] [nvarchar](50) NULL,
 CONSTRAINT [PK_OfficeAssignment] PRIMARY KEY CLUSTERED 
(
	[InstructorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 8/28/2017 1:27:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[HireDate] [datetime2](7) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EnrollmentDate] [datetime2](7) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Instructor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Course_DepartmentID]    Script Date: 8/28/2017 1:27:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_Course_DepartmentID] ON [dbo].[Course]
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CourseAssignment_InstructorID]    Script Date: 8/28/2017 1:27:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_CourseAssignment_InstructorID] ON [dbo].[CourseAssignment]
(
	[InstructorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Department_InstructorID]    Script Date: 8/28/2017 1:27:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_Department_InstructorID] ON [dbo].[Department]
(
	[InstructorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Enrollment_CourseID]    Script Date: 8/28/2017 1:27:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_Enrollment_CourseID] ON [dbo].[Enrollment]
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Enrollment_StudentID]    Script Date: 8/28/2017 1:27:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_Enrollment_StudentID] ON [dbo].[Enrollment]
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Course] ADD  DEFAULT ((1)) FOR [DepartmentID]
GO
ALTER TABLE [dbo].[Person] ADD  DEFAULT (N'Instructor') FOR [Discriminator]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Department_DepartmentID] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Department_DepartmentID]
GO
ALTER TABLE [dbo].[CourseAssignment]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssignment_Course_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseAssignment] CHECK CONSTRAINT [FK_CourseAssignment_Course_CourseID]
GO
ALTER TABLE [dbo].[CourseAssignment]  WITH CHECK ADD  CONSTRAINT [FK_CourseAssignment_Instructor_InstructorID] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Person] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseAssignment] CHECK CONSTRAINT [FK_CourseAssignment_Instructor_InstructorID]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Instructor_InstructorID] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Instructor_InstructorID]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Course_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Course_CourseID]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Person_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Person] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Person_StudentID]
GO
ALTER TABLE [dbo].[OfficeAssignment]  WITH CHECK ADD  CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorID] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Person] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OfficeAssignment] CHECK CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorID]
GO
