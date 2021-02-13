using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class PrimaryKey
    {
        public string name;
        public long object_id;
        public int schema_id;
        public long parent_object_id;
        public string type;

        public PrimaryKey(string name, long object_id, int schema_id, long parent_object_id, string type)
        {
            this.name = name;
            this.object_id = object_id;
            this.schema_id = schema_id;
            this.parent_object_id = parent_object_id;
            this.type = type;
        }

        public static List<PrimaryKey> getAllPrimaryKeys()
        {
            List<PrimaryKey> primaryKeys = new List<PrimaryKey>();


            SqlConnection connection = MyDB.getConnection();
            SqlCommand sqlCommand;
            SqlDataReader dataReader;

            try
            {
                connection.Open();

                sqlCommand = new SqlCommand("SELECT name, object_id, schema_id, parent_object_id, type FROM [sys].[key_constraints]", connection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string name = dataReader.GetValue(index++).ToString();
                    long object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    int schema_id = Int32.Parse(dataReader.GetValue(index++).ToString());
                    long parent_object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    string type = dataReader.GetValue(index++).ToString();

                    primaryKeys.Add(new PrimaryKey(name, object_id, schema_id, parent_object_id, type));

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
            return primaryKeys;
        }
    }
}
