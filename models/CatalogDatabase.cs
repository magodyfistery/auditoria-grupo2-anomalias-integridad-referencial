using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class CatalogDatabase
    {
        public string name;
        public int database_id;
        public string create_date;

        public CatalogDatabase(string name, int database_id, string create_date)
        {
            this.name = name;
            this.database_id = database_id;
            this.create_date = create_date;
        }


        public static List<CatalogDatabase> getAllCatalogDatabase()
        {
            List<CatalogDatabase> databases = new List<CatalogDatabase>();


            SqlConnection connection = MyDB.getConnection();
            SqlCommand sqlCommand;
            SqlDataReader dataReader;

            try
            {
                connection.Open();

                sqlCommand = new SqlCommand("SELECT name, database_id, create_date FROM sys.databases", connection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string name = dataReader.GetValue(index++).ToString();
                    int database_id = Int32.Parse(dataReader.GetValue(index++).ToString());
                    string create_date = dataReader.GetValue(index++).ToString();

                    databases.Add(new CatalogDatabase(name, database_id, create_date));

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
            return databases;
        }
    }
}
