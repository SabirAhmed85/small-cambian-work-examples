/****** Object:  Table [dbo].[KpiFunctions]    Script Date: 09/23/2014 11:49:25 ******/
SET IDENTITY_INSERT [dbo].[KpiFunctions] ON
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (1, N'Data Entry', 0, 0, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (2, N'Percentage', 2, 2, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (3, N'Difference', 2, 2, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (4, N'Sum of Three or more', 3, 3, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (5, N'Reduction on Last Week', 2, 2, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (6, N'Percentage against Last Week', 2, 2, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (7, N'Four Week Rolling Total', 1, 4, 1)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (8, N'This Month Rolling', 1, 5, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (9, N'This Month Rolling on Two Inputs', 2, 10, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (10, N'This Year Rolling Total One Input', 1, 0, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (12, N'This Year Rolling Total Two Inputs', 2, 0, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (13, N'Minus and Plus Calculation', 4, 4, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (14, N'Number as a proportion of another', 2, 2, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (16, N'This Month Rolling with Stored Procedure', 1, 5, 0)
INSERT [dbo].[KpiFunctions] ([KpiFunctionID], [KpiFunctionName], [InputCount], [InputValuesCount], [IsAccumulation]) VALUES (20, N'This Year Rolling with Stored Procedure', 1, 0, 0)
SET IDENTITY_INSERT [dbo].[KpiFunctions] OFF
