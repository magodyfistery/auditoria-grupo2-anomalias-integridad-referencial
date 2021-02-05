using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class MyDB
    {
        static string connetionString = @"Data Source=DESKTOP-U6QA500;Initial Catalog=pubs;User ID=g2;Password=g2";
        static SqlConnection cnn = null;

        public static SqlConnection getConnection()
        {
            if(MyDB.cnn == null)
            {
                MyDB.cnn = new SqlConnection(MyDB.connetionString);
                
            }
            return MyDB.cnn;
        }

        public static void selectAllAndWrite(string sql, string full_file_dir, string header="")
        {
            SqlConnection connection = MyDB.getConnection();

            SqlCommand sqlCommand;
            SqlDataReader dataReader;

            try
            {
                connection.Open();



                String output = header;
                sqlCommand = new SqlCommand(sql, connection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    int number_fields = dataReader.FieldCount;

                    for (int i = 0; i < number_fields; i++)
                    {
                        output = output + dataReader.GetValue(i);
                        if (i != number_fields - 1)
                        {
                            output = output + "|";
                        }
                    }
                    output = output + "\n";

                }

                MyFileManager.writeTXT(output, full_file_dir);
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

        

    }
}
