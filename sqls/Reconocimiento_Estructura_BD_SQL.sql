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




FOREIGN_KEY_CONSTRAINT
CHECK_CONSTRAINT
DEFAULT_CONSTRAINT
PRIMARY_KEY_CONSTRAINT
UNIQUE_CONSTRAINT
SQL_TRIGGER


