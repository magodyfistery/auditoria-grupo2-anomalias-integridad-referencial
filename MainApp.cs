using auditoria_grupo2_anomalias_integridad_referencial.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static auditoria_grupo2_anomalias_integridad_referencial.models.AnomalyDetector;

namespace auditoria_grupo2_anomalias_integridad_referencial
{
    public partial class MainApp : Form
    {
        List<CatalogDatabase> databases;
        Credentials father;

        public MainApp(Credentials father)


        {
            this.father = father;
            this.father.Hide();
            InitializeComponent();

            databases = CatalogDatabase.getAllCatalogDatabase();
            foreach (CatalogDatabase database in databases)
            {
                this.comboBox1.Items.Add(database.name);
            }
        }


        private void button_get_integrity_metadata_Click(object sender, EventArgs e)
        {
            SqlConnection connection = MyDB.getConnection();

            try
            {
                

                SqlCommand sqlCommand = new SqlCommand("SELECT LlaveForanea =o.name, Esquema =SCHEMA_NAME(t1.schema_id), Tabla =t1.name, Columna =c1.name, Esquema_Ref =SCHEMA_NAME(t2.schema_id), Tabla_Ref =t2.name, Columna_Ref =c2.name FROM sys.foreign_keys o INNER JOIN sys.foreign_key_columns fk ON o.object_id = fk.constraint_object_id INNER JOIN sys.tables t1 ON t1.object_id = fk.parent_object_id INNER JOIN sys.columns c1 ON c1.column_id = parent_column_id AND c1.object_id = t1.object_id INNER JOIN sys.tables t2 ON t2.object_id = fk.referenced_object_id INNER JOIN sys.columns c2 ON c2.column_id = referenced_column_id AND c2.object_id = t2.object_id", connection);

                connection.Open();

                // create data adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

                DataTable dtRecord = new DataTable();
                dataAdapter.Fill(dtRecord);
                dataGridView1.DataSource = dtRecord;


                sqlCommand = new SqlCommand("SELECT LlaveForanea =o.name, Esquema =SCHEMA_NAME(t1.schema_id), Tabla =t1.name, Columna =c1.name, Esquema_Ref =SCHEMA_NAME(t2.schema_id), Tabla_Ref =t2.name, Columna_Ref =c2.name FROM sys.foreign_keys o INNER JOIN sys.foreign_key_columns fk ON o.object_id = fk.constraint_object_id INNER JOIN sys.tables t1 ON t1.object_id = fk.parent_object_id INNER JOIN sys.columns c1 ON c1.column_id = parent_column_id AND c1.object_id = t1.object_id INNER JOIN sys.tables t2 ON t2.object_id = fk.referenced_object_id INNER JOIN sys.columns c2 ON c2.column_id = referenced_column_id AND c2.object_id = t2.object_id", connection);

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                string output_log = "LlaveForanea,Esquema,Tabla,Columna,Esquema_Ref,Tabla_Ref,Columna_Ref\n";
                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string LlaveForanea = dataReader.GetValue(index++).ToString();
                    string Esquema = dataReader.GetValue(index++).ToString();
                    string Tabla = dataReader.GetValue(index++).ToString();
                    string Columna = dataReader.GetValue(index++).ToString();
                    string Esquema_Ref = dataReader.GetValue(index++).ToString();
                    string Tabla_Ref = dataReader.GetValue(index++).ToString();
                    string Columna_Ref = dataReader.GetValue(index++).ToString();

                    output_log += LlaveForanea + "," + Esquema + "," + Tabla + "," + Columna + "," + Esquema_Ref + "," + Tabla_Ref + "," + Columna_Ref+"\n";

                }


                MyFileManager.writeTXT(output_log, "D://LOG_AUDITORIA_RELACIONES_INTEGRIDAD_REFERENCIAL.txt");

                sqlCommand.Dispose();
                dataReader.Close();





            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
            }
            finally
            {
                connection.Close();

            }



            /*

            string[][] sqls = new string[][] {
                //--->Esquemas
                //--Sys.schemas
                new string[]{"sys-schemas", "[name],[schema_id],[principal_id]", "SELECT [name],[schema_id],[principal_id] FROM [pubs].[sys].[schemas]"},
                //--->Información general de tablas:
                //--INFORMATION_SCHEMA.TABLES
                new string[]{ "INFORMATION_SCHEMA-TABLES", "[TABLE_CATALOG],[TABLE_SCHEMA],[TABLE_NAME]", "SELECT [TABLE_CATALOG],[TABLE_SCHEMA],[TABLE_NAME],[TABLE_TYPE] FROM[pubs].[INFORMATION_SCHEMA].[TABLES]"},
                //--->Información de claves primarias y foráneas
                //--INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
                new string[]{ "INFORMATION_SCHEMA-REFERENTIAL_CONSTRAINTS", "[CONSTRAINT_CATALOG],[CONSTRAINT_SCHEMA],[CONSTRAINT_NAME],[UNIQUE_CONSTRAINT_CATALOG],[UNIQUE_CONSTRAINT_SCHEMA],[UNIQUE_CONSTRAINT_NAME],[MATCH_OPTION],[UPDATE_RULE],[DELETE_RULE]", "SELECT [CONSTRAINT_CATALOG],[CONSTRAINT_SCHEMA],[CONSTRAINT_NAME],[UNIQUE_CONSTRAINT_CATALOG],[UNIQUE_CONSTRAINT_SCHEMA],[UNIQUE_CONSTRAINT_NAME],[MATCH_OPTION],[UPDATE_RULE],[DELETE_RULE] FROM [pubs].[INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]"},
                //--INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                new string[]{ "INFORMATION_SCHEMA-TABLE_CONSTRAINTS", "[CONSTRAINT_CATALOG],[CONSTRAINT_SCHEMA],[CONSTRAINT_NAME],[TABLE_CATALOG],[TABLE_SCHEMA],[TABLE_NAME],[CONSTRAINT_TYPE],[IS_DEFERRABLE],[INITIALLY_DEFERRED]", "SELECT [CONSTRAINT_CATALOG],[CONSTRAINT_SCHEMA],[CONSTRAINT_NAME],[TABLE_CATALOG],[TABLE_SCHEMA],[TABLE_NAME],[CONSTRAINT_TYPE],[IS_DEFERRABLE],[INITIALLY_DEFERRED] FROM [pubs].[INFORMATION_SCHEMA].[TABLE_CONSTRAINTS]"},
                //--Sys.key_constraints
                new string[]{ "sys-key_constraints", "[name], [object_id], [principal_id],[schema_id],[parent_object_id],[type],[type_desc],[create_date],[modify_date],[is_ms_shipped],[is_published],[is_schema_published],[unique_index_id],[is_system_named],[is_enforced]", "SELECT [name], [object_id], [principal_id],[schema_id],[parent_object_id],[type],[type_desc],[create_date],[modify_date],[is_ms_shipped],[is_published],[is_schema_published],[unique_index_id],[is_system_named],[is_enforced] FROM [pubs].[sys].[key_constraints]"},
                //--Sys.foreign_key_columns
                new string[]{ "sys-foreign_key_columns", "[constraint_object_id],[constraint_column_id],[parent_object_id],[parent_column_id],[referenced_object_id],[referenced_column_id]", "SELECT [constraint_object_id],[constraint_column_id],[parent_object_id],[parent_column_id],[referenced_object_id],[referenced_column_id] FROM [pubs].[sys].[foreign_key_columns]"},
                //--Sys.foreign_key
                new string[]{ "sys-foreign_key", "[name],[object_id],[principal_id],[schema_id],[parent_object_id],[type],[type_desc],[create_date],[modify_date],[is_ms_shipped],[is_published],[is_schema_published],[referenced_object_id],[key_index_id],[is_disabled],[is_not_for_replication],[is_not_trusted],[delete_referential_action],[delete_referential_action_desc],[update_referential_action],[update_referential_action_desc],[is_system_named]", "SELECT [name],[object_id],[principal_id],[schema_id],[parent_object_id],[type],[type_desc],[create_date],[modify_date],[is_ms_shipped],[is_published],[is_schema_published],[referenced_object_id],[key_index_id],[is_disabled],[is_not_for_replication],[is_not_trusted],[delete_referential_action],[delete_referential_action_desc],[update_referential_action],[update_referential_action_desc],[is_system_named] FROM [pubs].[sys].[foreign_keys]"},
                //--->Información general detallada de las tablas
                //--Sys.tables
                new string[]{ "sys-tables", "[name] ,[object_id] ,[principal_id] ,[schema_id] ,[parent_object_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_published] ,[is_schema_published] ,[lob_data_space_id] ,[filestream_data_space_id] ,[max_column_id_used] ,[lock_on_bulk_load] ,[uses_ansi_nulls] ,[is_replicated] ,[has_replication_filter] ,[is_merge_published] ,[is_sync_tran_subscribed] ,[has_unchecked_assembly_data] ,[text_in_row_limit] ,[large_value_types_out_of_row] ,[is_tracked_by_cdc] ,[lock_escalation] ,[lock_escalation_desc] ,[is_filetable] ,[is_memory_optimized] ,[durability] ,[durability_desc] ,[temporal_type] ,[temporal_type_desc] ,[history_table_id] ,[is_remote_data_archive_enabled] ,[is_external] ,[history_retention_period] ,[history_retention_period_unit] ,[history_retention_period_unit_desc] ,[is_node] ,[is_edge]", "SELECT [name] ,[object_id] ,[principal_id] ,[schema_id] ,[parent_object_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_published] ,[is_schema_published] ,[lob_data_space_id] ,[filestream_data_space_id] ,[max_column_id_used] ,[lock_on_bulk_load] ,[uses_ansi_nulls] ,[is_replicated] ,[has_replication_filter] ,[is_merge_published] ,[is_sync_tran_subscribed] ,[has_unchecked_assembly_data] ,[text_in_row_limit] ,[large_value_types_out_of_row] ,[is_tracked_by_cdc] ,[lock_escalation] ,[lock_escalation_desc] ,[is_filetable] ,[is_memory_optimized] ,[durability] ,[durability_desc] ,[temporal_type] ,[temporal_type_desc] ,[history_table_id] ,[is_remote_data_archive_enabled] ,[is_external] ,[history_retention_period] ,[history_retention_period_unit] ,[history_retention_period_unit_desc] ,[is_node] ,[is_edge] FROM [pubs].[sys].[tables]"},
                //--Sys.columns
                new string[]{ "sys-columns", "[object_id] ,[name] ,[column_id] ,[system_type_id] ,[user_type_id] ,[max_length] ,[precision] ,[scale] ,[collation_name] ,[is_nullable] ,[is_ansi_padded] ,[is_rowguidcol] ,[is_identity] ,[is_computed] ,[is_filestream] ,[is_replicated] ,[is_non_sql_subscribed] ,[is_merge_published] ,[is_dts_replicated] ,[is_xml_document] ,[xml_collection_id] ,[default_object_id] ,[rule_object_id] ,[is_sparse] ,[is_column_set] ,[generated_always_type] ,[generated_always_type_desc] ,[encryption_type] ,[encryption_type_desc] ,[encryption_algorithm_name] ,[column_encryption_key_id] ,[column_encryption_key_database_name] ,[is_hidden] ,[is_masked] ,[graph_type] ,[graph_type_desc]", "SELECT [object_id] ,[name] ,[column_id] ,[system_type_id] ,[user_type_id] ,[max_length] ,[precision] ,[scale] ,[collation_name] ,[is_nullable] ,[is_ansi_padded] ,[is_rowguidcol] ,[is_identity] ,[is_computed] ,[is_filestream] ,[is_replicated] ,[is_non_sql_subscribed] ,[is_merge_published] ,[is_dts_replicated] ,[is_xml_document] ,[xml_collection_id] ,[default_object_id] ,[rule_object_id] ,[is_sparse] ,[is_column_set] ,[generated_always_type] ,[generated_always_type_desc] ,[encryption_type] ,[encryption_type_desc] ,[encryption_algorithm_name] ,[column_encryption_key_id] ,[column_encryption_key_database_name] ,[is_hidden] ,[is_masked] ,[graph_type] ,[graph_type_desc] FROM [pubs].[sys].[columns]"},
                //--Sys.systypes
                new string[]{ "sys-systypes", "[name] ,[xtype] ,[status] ,[xusertype] ,[length] ,[xprec] ,[xscale] ,[tdefault] ,[domain] ,[uid] ,[reserved] ,[collationid] ,[usertype] ,[variable] ,[allownulls] ,[type] ,[printfmt] ,[prec] ,[scale] ,[collation]", "SELECT [name] ,[xtype] ,[status] ,[xusertype] ,[length] ,[xprec] ,[xscale] ,[tdefault] ,[domain] ,[uid] ,[reserved] ,[collationid] ,[usertype] ,[variable] ,[allownulls] ,[type] ,[printfmt] ,[prec] ,[scale] ,[collation] FROM [pubs].[sys].[systypes]"},
                //--Sys.default_constraints
                new string[]{ "sys-default_constraints", "[name] ,[object_id] ,[principal_id] ,[schema_id] ,[parent_object_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_published] ,[is_schema_published] ,[parent_column_id] ,[definition] ,[is_system_named]", "SELECT [name] ,[object_id] ,[principal_id] ,[schema_id] ,[parent_object_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_published] ,[is_schema_published] ,[parent_column_id] ,[definition] ,[is_system_named] FROM [pubs].[sys].[default_constraints]"},
                //--->Restricciones / Checks
                //--INFORMATION_SCHEMA.CHECK_CONSTRAINTS
                new string[]{ "INFORMATION_SCHEMA-CHECK_CONSTRAINTS", "[CONSTRAINT_CATALOG] ,[CONSTRAINT_SCHEMA] ,[CONSTRAINT_NAME] ,[CHECK_CLAUSE]", "SELECT [CONSTRAINT_CATALOG] ,[CONSTRAINT_SCHEMA] ,[CONSTRAINT_NAME] ,[CHECK_CLAUSE] FROM [pubs].[INFORMATION_SCHEMA].[CHECK_CONSTRAINTS]"},
                //--Sys.check_constraints
                new string[]{ "sys-check_constraints", "[name] ,[object_id] ,[principal_id] ,[schema_id] ,[parent_object_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_published] ,[is_schema_published] ,[is_disabled] ,[is_not_for_replication] ,[is_not_trusted] ,[parent_column_id] ,[definition] ,[uses_database_collation] ,[is_system_named]", "SELECT [name] ,[object_id] ,[principal_id] ,[schema_id] ,[parent_object_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_published] ,[is_schema_published] ,[is_disabled] ,[is_not_for_replication] ,[is_not_trusted] ,[parent_column_id] ,[definition] ,[uses_database_collation] ,[is_system_named] FROM [pubs].[sys].[check_constraints]"},
                //--Sys.triggers
                // new string[]{"sys-triggers", "[name] ,[object_id] ,[parent_class] ,[parent_class_desc] ,[parent_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_disabled] ,[is_not_for_replication] ,[is_instead_of_trigger]", "SELECT [name] ,[object_id] ,[parent_class] ,[parent_class_desc] ,[parent_id] ,[type] ,[type_desc] ,[create_date] ,[modify_date] ,[is_ms_shipped] ,[is_disabled] ,[is_not_for_replication] ,[is_instead_of_trigger] FROM [pubs].[sys].[triggers]"},

                };


            for (int i = 0; i < sqls.Length; i++)
            {
                string filename = sqls[i][0], header = sqls[i][1], sql = sqls[i][2];
                MyDB.selectAllAndWrite(sql, "D://" + filename + ".txt", header);
            }

            MessageBox.Show("TXT generados con los metadatos en D://");
            */

        }

        private void buttonDetectAnomalies_Click(object sender, EventArgs e)
        {
            List<Anomaly> anomalies_dbcc = AnomalyDetector.detectAnomaliesWithData();
            List<Anomaly> anomalies_structure = AnomalyDetector.detectAnomaliesWithNoData();


            string output_log = "";
            output_log += "**********ANOMALY in DATA***********\n";

            foreach (var item in anomalies_dbcc)
            {
                // string extra_summary = Trigger.getExtraSummary(item.object_id);

                output_log += "\n***Anomalía del objeto " + item.object_id + ": \n" + item.summary; // + "\n" + extra_summary + "\n";
            }

            output_log += "\n\n**********ANOMALY STRUCTURE*************\n";
            foreach (var item in anomalies_structure)
            {

                output_log += "\n***Anomalía structure in " + item.object_id + ": \n" + item.summary;
            }

            MyFileManager.writeTXT(output_log, "D://LOG_AUDITORIA_FULL_ANOMALIAS.txt");

            if(anomalies_dbcc.Count + anomalies_structure.Count > 0)
            {
                output_log += "\n\nLOG GENERADO CON ANOMALÍAS en D://AUDITORIA_LOG_.txt";
            }
            else
            {
                output_log += "\n\nNO SE DETECTARON ANOMALÍAS";
            }

            this.richTextBox1.Text = output_log;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string database_name = this.comboBox1.SelectedItem.ToString();

            if (!MyDB.setConnectionString(database_name))
            {
                MessageBox.Show("No se pudo conectar con el catálogo");
            }

            Console.WriteLine(MyDB.connetionString);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = MyDB.getConnection();
            SqlCommand sqlCommand;
            SqlDataReader dataReader;
            string output_definition = "";

            try
            {


                

                connection.Open();

                sqlCommand = new SqlCommand("DECLARE @PKRepetida varchar(40), @longitud int = 0, @i int = 0, @Nombre_columna varchar (40), @anomalias int, @j int = 0, @Id_objeto int, @EstaVacio int SET @longitud = (select count(*) from ( select COlUMN_NAME, COUNT(COLUMN_NAME) as duplicados from ( select TABLE_NAME,COLUMN_NAME, CONSTRAINT_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME NOT IN (select distinct TABLE_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ORDINAL_POSITION > 1 ) )T1 where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%' group by COLUMN_NAME)T2) WHILE @i < @longitud BEGIN SET @i = @i + 1 IF ((select duplicados from ( select COUNT(COLUMN_NAME) as duplicados,ROW_NUMBER() OVER (ORDER BY (select 1)) AS a from ( select TABLE_NAME,COLUMN_NAME, CONSTRAINT_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME NOT IN (select distinct TABLE_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ORDINAL_POSITION > 1 ) )T1 where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%' group by COLUMN_NAME)T2 where a = @i)>1) Begin SET @Nombre_columna =(select COLUMN_NAME from ( select COUNT(COLUMN_NAME) as duplicados, COLUMN_NAME,ROW_NUMBER() OVER (ORDER BY (select 1)) AS a from ( select TABLE_NAME,COLUMN_NAME, CONSTRAINT_NAME, ORDINAL_POSITION from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME NOT IN (select distinct TABLE_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ORDINAL_POSITION > 1 ) )T1 where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%' group by COLUMN_NAME)T2 where a = @i) print @Nombre_columna SET @anomalias = (select count(*) from ( select ROW_NUMBER() OVER (ORDER BY (select 1)) AS a, object_id from ( select TABLE_NAME, CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where COLUMN_NAME = @Nombre_columna)T1 join sys.objects so on (T1.TABLE_NAME = so.name) where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%')T2) WHILE @j < @anomalias BEGIN SET @j = @j + 1 SET @Id_objeto = (select object_id from ( select ROW_NUMBER() OVER (ORDER BY (select 1)) AS a, object_id from ( select TABLE_NAME, CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where COLUMN_NAME = @Nombre_columna) T1 join sys.objects so on (T1.TABLE_NAME = so.name) where CONSTRAINT_NAME LIKE 'PK%' or CONSTRAINT_NAME LIKE 'UPKCL%')T2 where a = @j) SET @EstaVacio = (select count(*) from sys.triggers st join sys.sql_modules ssm on (st.object_id = ssm.object_id ) where st.parent_id = @Id_objeto ) IF (@EstaVacio > 0) Begin select so.name as tabla,st.object_id, st.name , ssm.definition from sys.triggers st join sys.sql_modules ssm on (st.object_id = ssm.object_id ) join sys.objects so on (so.object_id = st.parent_id) where st.parent_id = @Id_objeto END END END END", connection);


                dataReader = sqlCommand.ExecuteReader();

                

                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string tabla = dataReader.GetValue(index++).ToString();
                    long object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    string name = dataReader.GetValue(index++).ToString();
                    string definition = dataReader.GetValue(index++).ToString();


                    output_definition += "Tabla: " + tabla + ", Object_id: " + object_id + ", nombre: " + name + "\nDefinition: " + definition + "\n\n";

                    

                }

                this.richTextBox2.Text = output_definition;
                MyFileManager.writeTXT(output_definition, "D://LOG_AUDITORIA_ANOMALIAS_DEFINICION.txt");




                sqlCommand.Dispose();
                dataReader.Close();



            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
            }
            finally
            {
                connection.Close();

            }
        }

        private void closeAll(object sender, FormClosedEventArgs e)
        {
            this.father.Close();
        }
    }
}
