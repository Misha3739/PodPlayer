CREATE DATABASE PodPlayer
GO
USE PodPlayer
GO
CREATE TABLE Podcast(
    Id bigint Identity(1,1) Primary Key,
    Title nvarchar(50),
    Url nvarchar(300)
)