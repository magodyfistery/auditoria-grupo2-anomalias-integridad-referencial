
namespace auditoria_grupo2_anomalias_integridad_referencial
{
    partial class MainApp
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_test_connection = new System.Windows.Forms.Button();
            this.button_get_integrity_metadata = new System.Windows.Forms.Button();
            this.button_detect_anomalies = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_test_connection
            // 
            this.button_test_connection.Location = new System.Drawing.Point(72, 12);
            this.button_test_connection.Name = "button_test_connection";
            this.button_test_connection.Size = new System.Drawing.Size(97, 23);
            this.button_test_connection.TabIndex = 0;
            this.button_test_connection.Text = "Test conexión";
            this.button_test_connection.UseVisualStyleBackColor = true;
            this.button_test_connection.Click += new System.EventHandler(this.button_test_connection_Click);
            // 
            // button_get_integrity_metadata
            // 
            this.button_get_integrity_metadata.Location = new System.Drawing.Point(62, 57);
            this.button_get_integrity_metadata.Name = "button_get_integrity_metadata";
            this.button_get_integrity_metadata.Size = new System.Drawing.Size(123, 41);
            this.button_get_integrity_metadata.TabIndex = 1;
            this.button_get_integrity_metadata.Text = "Obtener tablas de integridad referencial";
            this.button_get_integrity_metadata.UseVisualStyleBackColor = true;
            this.button_get_integrity_metadata.Click += new System.EventHandler(this.button_get_integrity_metadata_Click);
            // 
            // button_detect_anomalies
            // 
            this.button_detect_anomalies.Location = new System.Drawing.Point(72, 124);
            this.button_detect_anomalies.Name = "button_detect_anomalies";
            this.button_detect_anomalies.Size = new System.Drawing.Size(113, 34);
            this.button_detect_anomalies.TabIndex = 2;
            this.button_detect_anomalies.Text = "Detectar anomalías";
            this.button_detect_anomalies.UseVisualStyleBackColor = true;
            this.button_detect_anomalies.Click += new System.EventHandler(this.buttonDetectAnomalies_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 185);
            this.Controls.Add(this.button_detect_anomalies);
            this.Controls.Add(this.button_get_integrity_metadata);
            this.Controls.Add(this.button_test_connection);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_test_connection;
        private System.Windows.Forms.Button button_get_integrity_metadata;
        private System.Windows.Forms.Button button_detect_anomalies;
    }
}

