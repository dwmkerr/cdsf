
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/08/2011 16:34:02
-- Generated from EDMX file: C:\Repositories\dwmkerr\CompositeDataFramework\OrdersDataModel\OrdersModel.edmx
-- --------------------------------------------------
create database [OrdersDatabase];
SET QUOTED_IDENTIFIER OFF;
GO
USE [OrdersDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateOrdered] datetime  NOT NULL,
    [DateShipped] datetime  NOT NULL,
    [DateDelivered] datetime  NOT NULL
);
GO

-- Creating table 'ProductLines'
CREATE TABLE [dbo].[ProductLines] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Order_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductLines'
ALTER TABLE [dbo].[ProductLines]
ADD CONSTRAINT [PK_ProductLines]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Product_Id] in table 'ProductLines'
ALTER TABLE [dbo].[ProductLines]
ADD CONSTRAINT [FK_ProductLineProduct]
    FOREIGN KEY ([Product_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductLineProduct'
CREATE INDEX [IX_FK_ProductLineProduct]
ON [dbo].[ProductLines]
    ([Product_Id]);
GO

-- Creating foreign key on [Order_Id] in table 'ProductLines'
ALTER TABLE [dbo].[ProductLines]
ADD CONSTRAINT [FK_OrderProductLine]
    FOREIGN KEY ([Order_Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProductLine'
CREATE INDEX [IX_FK_OrderProductLine]
ON [dbo].[ProductLines]
    ([Order_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------


-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/08/2011 16:30:46
-- Generated from EDMX file: C:\Repositories\dwmkerr\CompositeDataFramework\UsersDataModel\UsersDataModel.edmx
-- -------------------------------------------------

create database [UsersDatabase];

SET QUOTED_IDENTIFIER OFF;
GO
USE [UsersDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [RoleId] int  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoleId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_RoleUser]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUser'
CREATE INDEX [IX_FK_RoleUser]
ON [dbo].[Users]
    ([RoleId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------


INSERT INTO [dbo].[Roles] VALUES ('Administrator');
INSERT INTO [dbo].[Users] VALUES ('Dave', '123', 1);
INSERT INTO [dbo].[Users] VALUES ('Sarah', '123', 1);
INSERT INTO [dbo].[Users] VALUES ('Bill', '123', 1);


INSERT INTO [dbo].[Products] VALUES ('The Endymion Omnibus', '11.99');
INSERT INTO [dbo].[Products] VALUES ('Epiphany of the Long Sun', '11.69');
INSERT INTO [dbo].[Products] VALUES ('Litany of the Long Sun', '16.16');
INSERT INTO [dbo].[Products] VALUES ('More Than Human', '4.95');
INSERT INTO [dbo].[Products] VALUES ('Bend Sinister', '10.79');



INSERT INTO [dbo].[Orders] VALUES (CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO [dbo].[ProductLines] VALUES ('100', 1, 1);
INSERT INTO [dbo].[ProductLines] VALUES ('120', 2, 1);
INSERT INTO [dbo].[ProductLines] VALUES ('140', 3, 1);
