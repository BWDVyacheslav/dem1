-- --------------------------------------------------
-- Entity Designer DDL Script (corrected for SSMS)
-- --------------------------------------------------

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- Создаём базу, если её нет
IF DB_ID(N'Catalog') IS NULL
    CREATE DATABASE [Catalog];
GO

USE [Catalog];
GO

IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------
IF OBJECT_ID(N'[dbo].[fk_users_roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [fk_users_roles];
GO

-- --------------------------------------------------
-- Dropping existing tables (в обратном порядке зависимостей)
-- --------------------------------------------------
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[PunktyVydachi]', 'U') IS NOT NULL DROP TABLE [dbo].[PunktyVydachi];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL DROP TABLE [dbo].[Categories];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

CREATE TABLE [dbo].[Categories] (
    [Id]   int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Products] (
    [Article]          nvarchar(50)  NOT NULL,
    [Name]             nvarchar(50)  NOT NULL,
    [EdinicaIzmereniya] nvarchar(50) NOT NULL,
    [Price]            float         NOT NULL,
    [Postavshik]       nvarchar(50)  NOT NULL,
    [Proizvoditel]     nvarchar(50)  NOT NULL,
    [CategoryId]       int           NOT NULL,
    [Discount]         float         NOT NULL,
    [Quantity]         int           NOT NULL,
    [Description]      nvarchar(255) NOT NULL,
    [Image]            nvarchar(255) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Article] ASC)
);
GO

CREATE TABLE [dbo].[PunktyVydachi] (
    [Number]       int      NOT NULL,
    [City]         nvarchar(50) NOT NULL,
    [Street]       nvarchar(50) NOT NULL,
    [StreetNumber] tinyint  NOT NULL,
    CONSTRAINT [PK_PunktyVydachi] PRIMARY KEY CLUSTERED ([Number] ASC)
);
GO

CREATE TABLE [dbo].[Roles] (
    [Id]   int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Users] (
    [RoleId]   int          NOT NULL,
    [FullName] nvarchar(50) NOT NULL,
    [Login]    nvarchar(50) NOT NULL,
    [Password] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Login] ASC)
);
GO

CREATE TABLE [dbo].[Orders] (
    [OrderNumber]    int           NOT NULL,
    [Products]       nvarchar(255) NOT NULL,
    [OrderDate]      nvarchar(50)  NOT NULL,
    [DeliveryDate]   datetime      NOT NULL,
    [StreetPVZ]      tinyint       NOT NULL,
    [ClientFullName] nvarchar(50)  NOT NULL,
    [Code]           int           NOT NULL,
    [OrderStatus]    nvarchar(50)  NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderNumber] ASC)
);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [fk_users_roles]
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_fk_users_roles] ON [dbo].[Users] ([RoleId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------


