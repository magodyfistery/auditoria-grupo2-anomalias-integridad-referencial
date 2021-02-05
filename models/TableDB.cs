using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class TableDB
    {
        public string name;
        public long object_id;
        public int schema_id;

        public TableDB(string name, long object_id, int schema_id)
        {
            this.name = name;
            this.object_id = object_id;
            this.schema_id = schema_id;
        }

        public static List<TableDB> getAllTables()
        {
            List<TableDB> tables = new List<TableDB>();


            SqlConnection connection = MyDB.getConnection();
            SqlCommand sqlCommand;
            SqlDataReader dataReader;

            try
            {
                connection.Open();

                sqlCommand = new SqlCommand("SELECT name, object_id, schema_id FROM [pubs].[sys].[tables]", connection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string name = dataReader.GetValue(index++).ToString();
                    long object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    int schema_id = Int32.Parse(dataReader.GetValue(index++).ToString());

                    tables.Add(new TableDB(name, object_id, schema_id));

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
            return tables;
        }


    }
}
