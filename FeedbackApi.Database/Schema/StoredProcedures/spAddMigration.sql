create function if not exists fn_tryAddMigration
returns bit
(
    @id int not null,
    @migrationName varchar(50) not null
)
as
begin
    if exists (select id from tblMigrationInfos where id = @id)
        return 0;
    
    insert into tblMigrationInfos
    ([Id], [Name])
    values
    (@id, @migrationName)

    return 1;
end;