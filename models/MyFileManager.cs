using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auditoria_grupo2_anomalias_integridad_referencial.models
{
    class MyFileManager
    {
        public static void writeTXT(string text, string full_file_dir)
        {
            System.IO.File.WriteAllText(@full_file_dir, text);

        }
    }
}
