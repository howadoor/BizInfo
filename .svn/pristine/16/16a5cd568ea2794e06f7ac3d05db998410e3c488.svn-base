SET QUOTED_IDENTIFIER ON;

USE [BizInfo];


IF OBJECT_ID(N'[dbo].[OccurenciesTmp]', 'U') IS NOT NULL
    DROP TABLE [dbo].OccurenciesTmp;


SELECT Fragment, COUNT(Fragment) AS Count
INTO OccurenciesTmp
FROM
(SELECT Q.value('.', 'nvarchar(64)') Fragment
FROM 
	(
	SELECT [StructuredContent] FROM [BizInfo].[dbo].[Infoset]) AS Phones CROSS APPLY Phones.[StructuredContent].nodes('/structured/phone') AS X(Q) 
	) Result
GROUP BY Fragment
UNION
SELECT Fragment, COUNT(Fragment) AS Count
FROM
(SELECT Q.value('.', 'nvarchar(64)') Fragment
FROM 
	(
	SELECT [StructuredContent]  FROM [BizInfo].[dbo].[Infoset]) AS Mails CROSS APPLY Mails.[StructuredContent].nodes('/structured/mail') AS X(Q) 
	) Result
GROUP BY Fragment
UNION
SELECT Fragment, COUNT(Fragment) AS Count
FROM
(SELECT Q.value('.', 'nvarchar(64)') Fragment
FROM 
	(
	SELECT [StructuredContent]  FROM [BizInfo].[dbo].[Infoset]) AS Authors CROSS APPLY Authors.[StructuredContent].nodes('/structured/author') AS X(Q) 
	) Result
GROUP BY Fragment
ORDER BY Count Desc;

IF OBJECT_ID(N'[dbo].[OccurenciesTmp]', 'U') IS NOT NULL
	BEGIN
	TRUNCATE TABLE [dbo].[OccurencySet];
	INSERT INTO [dbo].[OccurencySet] SELECT Count AS [Count], Fragment AS [Fragment]  FROM (SELECT Fragment AS Fragment, MAX([Count]) AS Count FROM [dbo].[OccurenciesTmp] GROUP BY [Fragment]) AS Roll;
	DROP TABLE [dbo].[OccurenciesTmp];
	END

