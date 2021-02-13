using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class ForeignKey
    {
        public string name;
        public long object_id;
        public int schema_id;
        public long parent_object_id;
        public long referenced_object_id;
        public int key_index_id;
        public bool is_disabled;
        public bool is_not_trusted;
        public int delete_referential_action;
        public string delete_referential_action_desc;
        public int update_referential_action;
        public string update_referential_action_desc;
        public bool is_system_named;

        public ForeignKey(string name, long object_id, int schema_id, long parent_object_id,
            long referenced_object_id, int key_index_id, bool is_disabled, bool is_not_trusted, int delete_referential_action,
            string delete_referential_action_desc, int update_referential_action, string update_referential_action_desc,
            bool is_system_named)
        {
            this.name = name;
            this.object_id = object_id;
            this.schema_id = schema_id;
            this.parent_object_id = parent_object_id;
            this.referenced_object_id = referenced_object_id;
            this.key_index_id = key_index_id;
            this.is_disabled = is_disabled;
            this.is_not_trusted = is_not_trusted;
            this.delete_referential_action = delete_referential_action;
            this.delete_referential_action_desc = delete_referential_action_desc;
            this.update_referential_action = update_referential_action;
            this.update_referential_action_desc = update_referential_action_desc;
            this.is_system_named = is_system_named;
        }


        public static List<ForeignKey> getAllForeignKeys()
        {
            List<ForeignKey> foreignKeys = new List<ForeignKey>();


            SqlConnection connection = MyDB.getConnection();
            SqlCommand sqlCommand;
            SqlDataReader dataReader;

            try
            {
                connection.Open();

                sqlCommand = new SqlCommand("SELECT [name], [object_id] ,[schema_id] ,[parent_object_id] ,[referenced_object_id] ," +
                    "[key_index_id] ,[is_disabled] ,[is_not_trusted] ,[delete_referential_action] ," +
                    "[delete_referential_action_desc] ,[update_referential_action] ,[update_referential_action_desc] ," +
                    "[is_system_named] FROM [sys].[foreign_keys]", connection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string name = dataReader.GetValue(index++).ToString();
                    long object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    int schema_id = Int32.Parse(dataReader.GetValue(index++).ToString());
                    long parent_object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    long referenced_object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    int key_index_id = Int32.Parse(dataReader.GetValue(index++).ToString());
                    bool is_disabled = Boolean.Parse(dataReader.GetValue(index++).ToString());
                    bool is_not_trusted = Boolean.Parse(dataReader.GetValue(index++).ToString());
                    int delete_referential_action = Int32.Parse(dataReader.GetValue(index++).ToString());
                    string delete_referential_action_desc = dataReader.GetValue(index++).ToString();
                    int update_referential_action = Int32.Parse(dataReader.GetValue(index++).ToString());
                    string update_referential_action_desc = dataReader.GetValue(index++).ToString();
                    bool is_system_named = Boolean.Parse(dataReader.GetValue(index++).ToString());

                    foreignKeys.Add(new ForeignKey(name, object_id, schema_id, parent_object_id, referenced_object_id,
                        key_index_id, is_disabled, is_not_trusted, delete_referential_action, delete_referential_action_desc,
                        update_referential_action, update_referential_action_desc, is_system_named));

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
            return foreignKeys;
        }

    }

    
}
