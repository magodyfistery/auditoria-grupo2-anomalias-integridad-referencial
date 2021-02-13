using auditoria_grupo2_anomalias_integridad_referencial.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace auditoria_grupo2_anomalias_integridad_referencial
{
    public partial class Credentials : Form
    {
        public Credentials()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyDB.setInstanceParams(this.textBox3.Text.Trim(), this.textBox1.Text.Trim(), this.textBox2.Text.Trim());

            if (MyDB.setConnectionString("pubs"))
            {
                MainApp mainApp = new MainApp(this);
                mainApp.Show();
            }
            else
            {
                MessageBox.Show("No se pudo conectar. Credenciales inválidas");
            }


            

        }
    }
}
