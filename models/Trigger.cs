using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class Trigger
    {
        public string name;
        public long object_id;
        public int parent_class;
        public string parent_class_desc;
        public long parent_id;
        public string type;
        public string type_desc;
        public bool is_ms_shipped;
        public bool is_disabled;
        public bool is_not_for_replication;
        public bool is_instead_of_trigger;


        public Trigger(string name, long object_id, int parent_class, string parent_class_desc, long parent_id,
                       string type, string type_desc, bool is_ms_shipped, bool is_disabled, bool is_not_for_replication,
                       bool is_instead_of_trigger)
        {
            this.name = name;
            this.object_id = object_id;
            this.parent_class = parent_class;
            this.parent_class_desc = parent_class_desc;
            this.parent_id = parent_id;
            this.type = type;
            this.type_desc = type_desc;
            this.is_ms_shipped = is_ms_shipped;
            this.is_disabled = is_disabled;
            this.is_not_for_replication = is_not_for_replication;
            this.is_instead_of_trigger = is_instead_of_trigger;
        }


        public static List<Trigger> getAllTriggersFromObject(long parent_id)
        {
            List<Trigger> triggers = new List<Trigger>();


            SqlConnection connection = MyDB.getConnection();
            SqlCommand sqlCommand;
            SqlDataReader dataReader;

            try
            {
                connection.Open();

                sqlCommand = new SqlCommand("SELECT [name] ,[object_id] ,[parent_class] ,[parent_class_desc] ," +
                    "[parent_id] ,[type] ,[type_desc] ,[is_ms_shipped] ,[is_disabled] ,[is_not_for_replication] ," +
                    "[is_instead_of_trigger] FROM [sys].[triggers] WHERE parent_id='"+ parent_id+"'", connection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    // Console.WriteLine("LEYENDO " + dataReader.FieldCount);
                    int index = 0;

                    string name = dataReader.GetValue(index++).ToString();
                    long object_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    int parent_class = Int32.Parse(dataReader.GetValue(index++).ToString());
                    string parent_class_des = dataReader.GetValue(index++).ToString();
                    long queried_parent_id = Int64.Parse(dataReader.GetValue(index++).ToString());
                    string type = dataReader.GetValue(index++).ToString();
                    string type_desc = dataReader.GetValue(index++).ToString();
                    bool is_ms_shipped = Boolean.Parse(dataReader.GetValue(index++).ToString());
                    bool is_disabled = Boolean.Parse(dataReader.GetValue(index++).ToString());
                    bool is_not_for_replication = Boolean.Parse(dataReader.GetValue(index++).ToString());
                    bool is_instead_of_trigger = Boolean.Parse(dataReader.GetValue(index++).ToString());




                    triggers.Add(new Trigger(name, object_id, parent_class, parent_class_des,
                        queried_parent_id, type, type_desc, is_ms_shipped, is_disabled,
                        is_not_for_replication, is_instead_of_trigger));

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
            return triggers;
        }

        public static string getExtraSummary(long parent_id)
        {
            string extra_summary = "";

            List<Trigger> triggers = Trigger.getAllTriggersFromObject(parent_id);

            if (triggers.Count > 0)
            {
                extra_summary = "\nVerificar si uno de los siguientes triggers resuelve este problema en su lógica:\n";
            }

            for (int index_triggers = 0; index_triggers < triggers.Count; index_triggers++)
            {
                extra_summary += "\tNombre del Trigger es " + triggers[index_triggers].name + ". ¿El trigger está activo?: " + ((triggers[index_triggers].is_disabled) ? "No" : "Si") + "\n";
            }

            return extra_summary;
        }
    }
}
