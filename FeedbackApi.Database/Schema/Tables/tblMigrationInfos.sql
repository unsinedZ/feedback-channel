create table if not exists [dbo].[tblMigrationInfos]
(
    [Id] int not null,
    [Name] varchar(50) not null,
    primary key (Id)
);
