using auditoria_grupo2_anomalias_integridad_referencial.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace auditoria_grupo2_anomalias_integridad_referencial
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Credentials());

            /*
            MyDB.setInstanceParams("DESKTOP-U6QA500", "g2", "g2");

            if (MyDB.setConnectionString("mibase"))
            {
                Application.Run(new MainApp());

                List<ForeignKey> foreignKeys = ForeignKey.getAllForeignKeys();
                List<PrimaryKey> primaryKeys = PrimaryKey.getAllPrimaryKeys();
                Console.WriteLine(foreignKeys.Count);
                Console.WriteLine(primaryKeys.Count);
        }
            else
            {
                MessageBox.Show("No se pudo conectar. Credenciales inválidas");
            }
            */


        }
    }
}
