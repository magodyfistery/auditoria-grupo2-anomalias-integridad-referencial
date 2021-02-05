using auditoria_grupo2_anomalias_integridad_referencial.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static auditoria_grupo2_anomalias_integridad_referencial.models.AnomalyDetector;

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
            // Application.Run(new Form1());
            List<Anomaly> anomalies_dbcc = AnomalyDetector.detectAnomaliesWithData();
            List<Anomaly> anomalies_structure = AnomalyDetector.detectAnomaliesWithNoData();


            Console.WriteLine("**********ANOMALY in DATA**************");
            foreach (var item in anomalies_dbcc)
            {
                Console.WriteLine("\nAnomalía in " + item.object_id + ": \n" + item.summary);
            }

            Console.WriteLine("\n\n**********ANOMALY STRUCTURE**************");
            foreach (var item in anomalies_structure)
            {
                Console.WriteLine("\nAnomalía structure in " + item.object_id + ": \n" + item.summary);
            }

        }
    }
}
