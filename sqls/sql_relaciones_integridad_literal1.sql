EXEC master.dbo.sp_configure 'show advanced options', 1
RECONFIGURE
EXEC master.dbo.sp_configure 'xp_cmdshell', 1
RECONFIGURE



EXEC xp_cmdshell 'bcp "use pubs;
/*FOREIGN KEY*/

select 
	sfk.object_id,
	sfk.name as FK_constraint_name, 
	sfk.type_desc, 
	sfk.delete_referential_action_desc, 
	sfk.update_referential_action_desc,
	st.name as table_name,
	sc.name as FK_column_name
	
from sys.foreign_keys sfk 
join sys.foreign_key_columns sfkc on (sfkc.constraint_object_id = sfk.object_id)
join sys.tables st on (sfk.parent_object_id = st.object_id)
join sys.columns sc on (sc.object_id = st.object_id and sc.column_id = sfkc.parent_column_id)


select st.object_id, st.name, st.type_desc, st.type from sys.triggers st join sys.objects so on (st.parent_id = so.object_id)


select scc.object_id, scc.name, scc.type_desc, scc.type from sys.check_constraints scc
union all
select sdc.object_id, sdc.name, sdc.type_desc, sdc.type from sys.default_constraints sdc
union all
select skc.object_id, skc.name, skc.type_desc, skc.type from sys.key_constraints skc



select scc.object_id, scc.name, scc.type_desc, scc.type from sys.check_constraints scc
union all
select sdc.object_id, sdc.name, sdc.type_desc, sdc.type from sys.default_constraints sdc
union all
select skc.object_id, skc.name, skc.type_desc, skc.type from sys.key_constraints skc
" queryout "C:\deber.txt" -T -c'
GO

CREATE VIEW v_fk 
AS
select 
	sfk.object_id,
	sfk.name as FK_constraint_name, 
	sfk.type_desc, 
	sfk.delete_referential_action_desc, 
	sfk.update_referential_action_desc,
	st.name as table_name,
	sc.name as FK_column_name
	
from sys.foreign_keys sfk 
join sys.foreign_key_columns sfkc on (sfkc.constraint_object_id = sfk.object_id)
join sys.tables st on (sfk.parent_object_id = st.object_id)
join sys.columns sc on (sc.object_id = st.object_id and sc.column_id = sfkc.parent_column_id)
GO

SELECT * FROM v_fk
GO



CREATE VIEW v_tr
AS
select st.object_id, st.name, st.type_desc, st.type from sys.triggers st join sys.objects so on (st.parent_id = so.object_id)
GO

SELECT * FROM v_tr
GO


CREATE VIEW v_chekc_pk_upk 
AS
select scc.object_id, scc.name, scc.type_desc, scc.type from sys.check_constraints scc
union all
select sdc.object_id, sdc.name, sdc.type_desc, sdc.type from sys.default_constraints sdc
union all
select skc.object_id, skc.name, skc.type_desc, skc.type from sys.key_constraints skc
GO


SELECT * FROM v_chekc_pk_upk where type = 'C'
GO




--FOREIGN_KEY_CONSTRAINT
--CHECK_CONSTRAINT
--DEFAULT_CONSTRAINT
--PRIMARY_KEY_CONSTRAINT
--UNIQUE_CONSTRAINT
--SQL_TRIGGER


SELECT  
 LlaveForanea	=o.name,
 Esquema	=SCHEMA_NAME(t1.schema_id),
 Tabla		=t1.name,
 Columna	=c1.name,
 Esquema_Ref	=SCHEMA_NAME(t2.schema_id),
 Tabla_Ref	=t2.name,
 Columna_Ref	=c2.name
FROM sys.foreign_keys o
INNER JOIN sys.foreign_key_columns fk
    ON o.object_id = fk.constraint_object_id
INNER JOIN sys.tables t1
    ON t1.object_id = fk.parent_object_id
INNER JOIN sys.columns c1
    ON c1.column_id = parent_column_id 
	  AND c1.object_id = t1.object_id
INNER JOIN sys.tables t2
    ON t2.object_id = fk.referenced_object_id
INNER JOIN sys.columns c2
    ON c2.column_id = referenced_column_id 
	  AND c2.object_id = t2.object_id

GO


CREATE VIEW v_referencial_integridad
AS
SELECT  
 LlaveForanea	=o.name,
 Esquema	=SCHEMA_NAME(t1.schema_id),
 Tabla		=t1.name,
 Columna	=c1.name,
 Esquema_Ref	=SCHEMA_NAME(t2.schema_id),
 Tabla_Ref	=t2.name,
 Columna_Ref	=c2.name
FROM sys.foreign_keys o
INNER JOIN sys.foreign_key_columns fk
    ON o.object_id = fk.constraint_object_id
INNER JOIN sys.tables t1
    ON t1.object_id = fk.parent_object_id
INNER JOIN sys.columns c1
    ON c1.column_id = parent_column_id 
	  AND c1.object_id = t1.object_id
INNER JOIN sys.tables t2
    ON t2.object_id = fk.referenced_object_id
INNER JOIN sys.columns c2
    ON c2.column_id = referenced_column_id 
	  AND c2.object_id = t2.object_id

GO

SELECT * FROM v_referencial_integridad