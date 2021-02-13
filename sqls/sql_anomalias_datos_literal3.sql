DECLARE @longitud int =  0 
DECLARE @FKs varchar(40) 
SET @longitud = (select count(*) from sys.foreign_keys) 
DECLARE @i int = 0 WHILE @i < @longitud BEGIN SET @i = @i + 1 SET @FKs = (SELECT name FROM(SELECT ROW_NUMBER() OVER (ORDER BY (select 1)) AS a,name FROM sys.foreign_keys) T1	WHERE a = @i)	DBCC CHECKCONSTRAINTS (@FKs) END
GO



CREATE PROCEDURE v_anomalias_datos2 
	
AS
	DECLARE @i int = 0,
	@longitud int =  0,
	@FKs varchar(40)
	SET @longitud = (select count(*) from sys.foreign_keys) 
	WHILE @i < @longitud
	BEGIN 
		SET @i = @i + 1 
		SET @FKs = (SELECT name FROM(SELECT ROW_NUMBER() OVER (ORDER BY (select 1)) AS a,name 
		FROM sys.foreign_keys) T1	WHERE a = @i)
		DBCC CHECKCONSTRAINTS (@FKs) 
	END
GO


EXEC v_anomalias_datos2

