USE [BizInfo]
GO

ALTER FUNCTION [dbo].[IsContactInStructured] (@structured as XML) returns bit
WITH SCHEMABINDING
AS
	begin
	declare @result bit = 0;
	IF (@structured IS NOT NULL AND (@structured.exist('/structured/mail') = 1 OR @structured.exist('/structured/phone') = 1 OR @structured.exist('/structured/skype') = 1 OR @structured.exist('/structured/icq') = 1))
		SET @result = 1;
	ELSE
		SET @result = 0;
	return @result;
	end
GO

ALTER TABLE [BizInfo].[dbo].[InfoSet] 
ADD [HasContact] AS ISNULL (dbo.IsContactInStructured(StructuredContent), 0) PERSISTED -- ISNULL is used because we deliberately want not nullable bit
GO