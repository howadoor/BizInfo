ALTER FUNCTION [dbo].[HasContact] (@structured as XML) returns bit
AS
	begin
	declare @result bit;
	set @result = (@structured IS NOT NULL AND (SELECT @structured.exist('/structured/mail') OR @structured.exist('/structured/phone') = 1 OR @structured.exist('/structured/skype') = 1 OR @structured.exist('/structured/icq') = 1));
	return @result;
	end
GO

ALTER TABLE [BizInfo].[dbo].[InfoSet] 
ADD [HasContact] AS (NOT ISNULL([StructuredContent])
GO