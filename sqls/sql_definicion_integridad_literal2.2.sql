

CREATE PROCEDURE v_definicion_integridad
AS
DECLARE @PKRepetida varchar(40),
@longitud int =  0,
@i int = 0,
@Nombre_columna varchar (40),
@anomalias int,
@j int = 0,
@Id_objeto int,
@EstaVacio int
SET @longitud = (select count(*) from ( 
select COlUMN_NAME, COUNT(COLUMN_NAME) as duplicados from (
select TABLE_NAME,COLUMN_NAME, CONSTRAINT_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
where  TABLE_NAME NOT IN (select distinct TABLE_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ORDINAL_POSITION > 1 )
)T1 where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%' group by COLUMN_NAME)T2)
WHILE @i < @longitud
BEGIN
    SET @i = @i + 1
	IF ((select duplicados from (
		select COUNT(COLUMN_NAME) as duplicados,ROW_NUMBER() OVER (ORDER BY (select 1)) AS a from (
		select TABLE_NAME,COLUMN_NAME, CONSTRAINT_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
		where  TABLE_NAME NOT IN (select distinct TABLE_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ORDINAL_POSITION > 1 )
		)T1 where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%' group by COLUMN_NAME)T2 where a = @i)>1)
		Begin 
			SET @Nombre_columna =(select COLUMN_NAME from (
			select COUNT(COLUMN_NAME) as duplicados, COLUMN_NAME,ROW_NUMBER() OVER (ORDER BY (select 1)) AS a from (
			select TABLE_NAME,COLUMN_NAME, CONSTRAINT_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
			where  TABLE_NAME NOT IN (select distinct TABLE_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ORDINAL_POSITION > 1 )
			)T1 where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%' group by COLUMN_NAME)T2 where a = @i)
			print @Nombre_columna
			SET @anomalias = (select count(*) from (
			select ROW_NUMBER() OVER (ORDER BY (select 1)) AS a, object_id from (
			select TABLE_NAME, CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where COLUMN_NAME = @Nombre_columna)T1
			join sys.objects so  on (T1.TABLE_NAME = so.name)
			where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%')T2)
			WHILE @j < @anomalias
				BEGIN
					SET @j = @j + 1
					SET @Id_objeto = (select object_id from (
					select ROW_NUMBER() OVER (ORDER BY (select 1)) AS a, object_id from (
					select TABLE_NAME, CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where COLUMN_NAME = @Nombre_columna) T1
					join sys.objects so  on (T1.TABLE_NAME = so.name)
					where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%')T2 where a = @j)
					SET @EstaVacio = (select count(*) from sys.triggers st 
					join sys.sql_modules ssm on (st.object_id = ssm.object_id )
					where st.parent_id = @Id_objeto )
					IF (@EstaVacio > 0)
						Begin
							select so.name as tabla,st.object_id, st.name , ssm.definition from sys.triggers st 
							join sys.sql_modules ssm on (st.object_id = ssm.object_id )
							join sys.objects so on (so.object_id = st.parent_id)
							where st.parent_id = @Id_objeto 
						END
				END
		END
END
GO


EXEC v_definicion_integridad