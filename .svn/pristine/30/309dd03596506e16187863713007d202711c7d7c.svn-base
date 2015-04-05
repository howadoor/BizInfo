
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/16/2012 10:13:33
-- Generated from EDMX file: C:\Projects\BizInfo\devel\BizInfo\BizInfo.Model\Entities\BizInfo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BizInfo];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

DROP TABLE [dbo].[SearchCriteriaSet];

DROP TABLE [dbo].[SearchingLogSet];

-- Creating table 'SearchCriteriaSet'
CREATE TABLE [dbo].[SearchCriteriaSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Hash] uniqueidentifier  NOT NULL,
    [Criteria] xml  NULL,
    [LastUsedTime] datetime  NOT NULL,
    [UsageCount] int  NOT NULL
);
GO

-- Creating table 'SearchingLogSet'
CREATE TABLE [dbo].[SearchingLogSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [SearchCriteriaId] bigint  NOT NULL,
    [Start] datetime  NOT NULL,
    [Duration] time  NOT NULL,
    [ResultsCount] int  NOT NULL,
    [Exception] nvarchar(max)  NULL,
    [UserId] uniqueidentifier  NOT NULL,
	[Succeeded] bit NOT NULL
);
GO


-- Creating primary key on [Id] in table 'SearchCriteriaSet'
ALTER TABLE [dbo].[SearchCriteriaSet]
ADD CONSTRAINT [PK_SearchCriteriaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SearchingLogSet'
ALTER TABLE [dbo].[SearchingLogSet]
ADD CONSTRAINT [PK_SearchingLogSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO



-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------