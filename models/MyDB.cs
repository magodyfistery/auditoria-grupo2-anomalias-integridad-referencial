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
        public static string instance = "DESKTOP-U6QA500";
        public static string catalog = "mibase";
        public static string user = "g2";
        public static string password = "g2";

        public static void setInstanceParams(string instance, string user, string password)
        {
            MyDB.instance = instance;
            MyDB.user = user;
            MyDB.password = password;
        }

        


        public static string connetionString = @"Data Source=DESKTOP-U6QA500;Initial Catalog=mibase;User ID=gx;Password=gx";
        static SqlConnection cnn = null;

        public static bool setConnectionString(string catalog)
        {
            bool condition = false;
            string tryConnetionString = "Data Source=" + MyDB.instance + ";Initial Catalog=" + catalog + ";User ID=" + MyDB.user + ";Password="+ MyDB.password;


            SqlConnection tryConnection = new SqlConnection(tryConnetionString);
            try
            {
                tryConnection.Open();
                condition = true;

            }
            catch (Exception error)
            {
                Console.WriteLine(error);

            }
            finally
            {
                tryConnection.Close();
            }
            MyDB.connetionString = tryConnetionString;
            MyDB.cnn = new SqlConnection(MyDB.connetionString);
            return condition;

        }

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
