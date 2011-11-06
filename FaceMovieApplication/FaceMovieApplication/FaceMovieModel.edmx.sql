
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/05/2011 19:35:15
-- Generated from EDMX file: C:\Users\Vicente\Documents\Facultad\5to\Recuperación de Información y Recomendaciones en la Web\Proyecto\FaceMovieApplication\trunk\FaceMovieApplication\FaceMovieApplication\FaceMovieModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FaceMovieDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserUserMovie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserMovieSet] DROP CONSTRAINT [FK_UserUserMovie];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieUserMovie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserMovieSet] DROP CONSTRAINT [FK_MovieUserMovie];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieSimilarityMovie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieSimilaritySet] DROP CONSTRAINT [FK_MovieSimilarityMovie];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieSimilarityMovie1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieSimilaritySet] DROP CONSTRAINT [FK_MovieSimilarityMovie1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[MovieSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovieSet];
GO
IF OBJECT_ID(N'[dbo].[UserMovieSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserMovieSet];
GO
IF OBJECT_ID(N'[dbo].[MovieSimilaritySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovieSimilaritySet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserFacebookToken] nvarchar(max)  NULL,
    [UserFacebookId] bigint  NOT NULL,
    [UserFirstName] nvarchar(max)  NOT NULL,
    [UserLastName] nvarchar(max)  NULL,
    [UserPictureLink] nvarchar(max)  NULL
);
GO

-- Creating table 'MovieSet'
CREATE TABLE [dbo].[MovieSet] (
    [MovieId] int IDENTITY(1,1) NOT NULL,
    [MovieFacebookPageId] bigint  NOT NULL,
    [MovieName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserMovieSet'
CREATE TABLE [dbo].[UserMovieSet] (
    [UserMovieId] int IDENTITY(1,1) NOT NULL,
    [UserMovieRanking] float  NOT NULL,
    [User_UserId] int  NOT NULL,
    [Movie_MovieId] int  NOT NULL
);
GO

-- Creating table 'MovieSimilaritySet'
CREATE TABLE [dbo].[MovieSimilaritySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Similarity] float  NOT NULL,
    [Movie_1_MovieId] int  NOT NULL,
    [Movie_2_MovieId] int  NOT NULL
);
GO

-- Creating table 'Parameters'
CREATE TABLE [dbo].[Parameters] (
    [ParameterId] int IDENTITY(1,1) NOT NULL,
    [ParameterName] nvarchar(max)  NOT NULL,
    [ParameterValue] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserId] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [MovieId] in table 'MovieSet'
ALTER TABLE [dbo].[MovieSet]
ADD CONSTRAINT [PK_MovieSet]
    PRIMARY KEY CLUSTERED ([MovieId] ASC);
GO

-- Creating primary key on [UserMovieId] in table 'UserMovieSet'
ALTER TABLE [dbo].[UserMovieSet]
ADD CONSTRAINT [PK_UserMovieSet]
    PRIMARY KEY CLUSTERED ([UserMovieId] ASC);
GO

-- Creating primary key on [Id] in table 'MovieSimilaritySet'
ALTER TABLE [dbo].[MovieSimilaritySet]
ADD CONSTRAINT [PK_MovieSimilaritySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ParameterId] in table 'Parameters'
ALTER TABLE [dbo].[Parameters]
ADD CONSTRAINT [PK_Parameters]
    PRIMARY KEY CLUSTERED ([ParameterId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_UserId] in table 'UserMovieSet'
ALTER TABLE [dbo].[UserMovieSet]
ADD CONSTRAINT [FK_UserUserMovie]
    FOREIGN KEY ([User_UserId])
    REFERENCES [dbo].[UserSet]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserMovie'
CREATE INDEX [IX_FK_UserUserMovie]
ON [dbo].[UserMovieSet]
    ([User_UserId]);
GO

-- Creating foreign key on [Movie_MovieId] in table 'UserMovieSet'
ALTER TABLE [dbo].[UserMovieSet]
ADD CONSTRAINT [FK_MovieUserMovie]
    FOREIGN KEY ([Movie_MovieId])
    REFERENCES [dbo].[MovieSet]
        ([MovieId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieUserMovie'
CREATE INDEX [IX_FK_MovieUserMovie]
ON [dbo].[UserMovieSet]
    ([Movie_MovieId]);
GO

-- Creating foreign key on [Movie_1_MovieId] in table 'MovieSimilaritySet'
ALTER TABLE [dbo].[MovieSimilaritySet]
ADD CONSTRAINT [FK_MovieSimilarityMovie]
    FOREIGN KEY ([Movie_1_MovieId])
    REFERENCES [dbo].[MovieSet]
        ([MovieId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieSimilarityMovie'
CREATE INDEX [IX_FK_MovieSimilarityMovie]
ON [dbo].[MovieSimilaritySet]
    ([Movie_1_MovieId]);
GO

-- Creating foreign key on [Movie_2_MovieId] in table 'MovieSimilaritySet'
ALTER TABLE [dbo].[MovieSimilaritySet]
ADD CONSTRAINT [FK_MovieSimilarityMovie1]
    FOREIGN KEY ([Movie_2_MovieId])
    REFERENCES [dbo].[MovieSet]
        ([MovieId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieSimilarityMovie1'
CREATE INDEX [IX_FK_MovieSimilarityMovie1]
ON [dbo].[MovieSimilaritySet]
    ([Movie_2_MovieId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------