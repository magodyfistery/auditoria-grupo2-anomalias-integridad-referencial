using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class AnomalyDetector
    {
        public class Anomaly
        {
            public static int TYPE_NO_DATA = 0;
            public static int TYPE_WITH_DATA = 1;

            public long object_id;
            public int type;
            public string summary;

            public Anomaly(long object_id, int type, string summary)
            {
                this.object_id = object_id;
                this.type = type;
                this.summary = summary;
            }
        }

        public static List<Anomaly> detectAnomaliesWithNoData()
        {
            List<Anomaly> anomalies = new List<Anomaly>();


            List<ForeignKey> foreignKeys = ForeignKey.getAllForeignKeys();
            List<TableDB> tables = TableDB.getAllTables();
            List<PrimaryKey> primaryKeys = PrimaryKey.getAllPrimaryKeys();

            // Rule#1: Una tabla no posee una relación con ninguna otra tabla (está solitaria), y posiblemente exista una relación cortada

            for (int index_table=0;index_table<tables.Count; index_table++)
            {
                bool table_has_foreign_key_of_other = false;
                bool table_is_a_foreign_key_in_other = false;

                for (int index_fk = 0; index_fk < foreignKeys.Count; index_fk++)
                {
                    if(foreignKeys[index_fk].parent_object_id == tables[index_table].object_id)
                    {
                        table_has_foreign_key_of_other = true;
                    }
                    if (foreignKeys[index_fk].referenced_object_id == tables[index_table].object_id)
                    {
                        table_is_a_foreign_key_in_other = true;
                    }
                }

                if(!table_has_foreign_key_of_other && !table_is_a_foreign_key_in_other)
                {
                    // table is alone and not conected
                    string extra_summary = Trigger.getExtraSummary(tables[index_table].object_id);

                    anomalies.Add(new Anomaly(tables[index_table].object_id, Anomaly.TYPE_NO_DATA,
                        tables[index_table].name + "|" + tables[index_table].object_id + ": tabla aislada, posible anomalía."+ extra_summary));
                }
            }

            // Rule#2: Una tabla no posee clave primaria pero si posee clave foránea de otra tabla. 
            //         ¿Es así o se requiere una clave primaria?


            List<TableDB> tablesWithPrimaryKey = new List<TableDB>();
            List<TableDB> tablesWithOutPrimaryKey = new List<TableDB>();
            for (int index_table = 0; index_table < tables.Count; index_table++)
            {
                for (int index_pk = 0; index_pk < primaryKeys.Count; index_pk++)
                {
                    if(primaryKeys[index_pk].parent_object_id == tables[index_table].object_id)
                    {
                        tablesWithPrimaryKey.Add(tables[index_table]);
                    }
                }
            }

            for (int index_table = 0; index_table < tables.Count; index_table++)
            {
                if (!tablesWithPrimaryKey.Contains(tables[index_table])){
                    tablesWithOutPrimaryKey.Add(tables[index_table]);
                }
            }

            

            for (int index_table_no_pk = 0; index_table_no_pk < tablesWithOutPrimaryKey.Count; index_table_no_pk++)
            {
                bool table_has_foreign_key_of_other = false;
                bool table_is_a_foreign_key_in_other = false;
                for (int index_fk = 0; index_fk < foreignKeys.Count; index_fk++)
                {
                    if (foreignKeys[index_fk].parent_object_id == tablesWithOutPrimaryKey[index_table_no_pk].object_id)
                    {
                        table_has_foreign_key_of_other = true;
                    }
                    if (foreignKeys[index_fk].referenced_object_id == tablesWithOutPrimaryKey[index_table_no_pk].object_id)
                    {
                        table_is_a_foreign_key_in_other = true;
                    }
                }
                if (table_has_foreign_key_of_other)
                {
                    string extra_summary = Trigger.getExtraSummary(tablesWithOutPrimaryKey[index_table_no_pk].object_id);
                    // table has FK but this FK are not primary key
                    anomalies.Add(new Anomaly(tablesWithOutPrimaryKey[index_table_no_pk].object_id, Anomaly.TYPE_NO_DATA,
                        tablesWithOutPrimaryKey[index_table_no_pk].name + "|" + tablesWithOutPrimaryKey[index_table_no_pk].object_id + ": tabla con FK pero sin PK. ¿Esto es a propósito o un error?\n" + extra_summary));
                }
                if (table_is_a_foreign_key_in_other)
                {
                    string extra_summary = Trigger.getExtraSummary(tablesWithOutPrimaryKey[index_table_no_pk].object_id);
                    // table has FK but this FK are not primary key
                    anomalies.Add(new Anomaly(tablesWithOutPrimaryKey[index_table_no_pk].object_id, Anomaly.TYPE_NO_DATA,
                        tablesWithOutPrimaryKey[index_table_no_pk].name + "|" + tablesWithOutPrimaryKey[index_table_no_pk].object_id + ": Gran error, la tabla no tiene PK y es FK en otra tabla\n" + extra_summary));
                }
            }



            Console.WriteLine(tablesWithOutPrimaryKey.Count);




            return anomalies;
        }

        public static List<Anomaly> detectAnomaliesWithData()
        {
            List<Anomaly> anomalies = new List<Anomaly>();

            List<ForeignKey> foreignKeys = ForeignKey.getAllForeignKeys();
            List<PrimaryKey> primaryKeys = PrimaryKey.getAllPrimaryKeys();

            for (int i=0; i<foreignKeys.Count; i++)
            {
                SqlCommand sqlCommand;
                SqlDataReader dataReader;
                SqlConnection connection = MyDB.getConnection();

                try
                {
                    connection.Open();
                    string sql = "DBCC CHECKCONSTRAINTS('" + foreignKeys[i].name + "')";
                    sqlCommand = new SqlCommand(sql, connection);
                    dataReader = sqlCommand.ExecuteReader();
                    string output = "ForeignKey: ";

                    bool exist_data = false;

                    while (dataReader.Read())
                    {
                        int number_fields = dataReader.FieldCount;

                        for (int j = 0; j < number_fields; j++)
                        {
                            output = output + dataReader.GetValue(j);
                            exist_data = true;
                            if (j != number_fields - 1)
                            {
                                output = output + "|";
                            }
                        }
                        output = output + "\n";

                    }

                    if (exist_data)
                    {
                        anomalies.Add(new Anomaly(foreignKeys[i].parent_object_id, Anomaly.TYPE_WITH_DATA, output));
                        // Console.WriteLine("\nANOMALLY DBCC");
                        // Console.WriteLine(output);
                    }

                    sqlCommand.Dispose();
                    dataReader.Close();

                }
                catch (Exception error)
                {
                    Console.WriteLine("Error: " + error.StackTrace);
                }
                finally
                {
                    connection.Close();

                }
            }


            for (int i = 0; i < primaryKeys.Count; i++)
            {
                SqlCommand sqlCommand;
                SqlDataReader dataReader;
                SqlConnection connection = MyDB.getConnection();

                try
                {
                    connection.Open();
                    string sql = "DBCC CHECKCONSTRAINTS('" + primaryKeys[i].name + "')";
                    sqlCommand = new SqlCommand(sql, connection);
                    dataReader = sqlCommand.ExecuteReader();
                    string output = "PrimaryKey: ";

                    bool exist_data = false;

                    while (dataReader.Read())
                    {
                        int number_fields = dataReader.FieldCount;

                        for (int j = 0; j < number_fields; j++)
                        {
                            output = output + dataReader.GetValue(j);
                            exist_data = true;
                            if (j != number_fields - 1)
                            {
                                output = output + "|";
                            }
                        }
                        output = output + "\n";

                    }

                    if (exist_data)
                    {
                        anomalies.Add(new Anomaly(primaryKeys[i].parent_object_id, Anomaly.TYPE_WITH_DATA, output));
                        // Console.WriteLine("\nANOMALLY DBCC");
                        // Console.WriteLine(output);
                    }

                    sqlCommand.Dispose();
                    dataReader.Close();

                }
                catch (Exception error)
                {
                    Console.WriteLine("Error: " + error.StackTrace);
                }
                finally
                {
                    connection.Close();

                }
            }

            //TODO: agregar checkconstraints




            return anomalies;
        }
    }
}
