
namespace Applicazione_Uffici
{
    partial class frmControlli
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnBraccioAvanti = new System.Windows.Forms.Button();
            this.btnBraccioIndietro = new System.Windows.Forms.Button();
            this.btnBraccioSinistra = new System.Windows.Forms.Button();
            this.btnBraccioDestra = new System.Windows.Forms.Button();
            this.btnBraccioSu = new System.Windows.Forms.Button();
            this.btnBraccioGiù = new System.Windows.Forms.Button();
            this.btnApriPinza = new System.Windows.Forms.Button();
            this.btnChiudiPinza = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(309, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "Controlli";
            // 
            // btnBraccioAvanti
            // 
            this.btnBraccioAvanti.Location = new System.Drawing.Point(257, 305);
            this.btnBraccioAvanti.Name = "btnBraccioAvanti";
            this.btnBraccioAvanti.Size = new System.Drawing.Size(103, 49);
            this.btnBraccioAvanti.TabIndex = 2;
            this.btnBraccioAvanti.Text = "braccio avanti";
            this.btnBraccioAvanti.UseVisualStyleBackColor = true;
            // 
            // btnBraccioIndietro
            // 
            this.btnBraccioIndietro.Location = new System.Drawing.Point(403, 305);
            this.btnBraccioIndietro.Name = "btnBraccioIndietro";
            this.btnBraccioIndietro.Size = new System.Drawing.Size(103, 49);
            this.btnBraccioIndietro.TabIndex = 3;
            this.btnBraccioIndietro.Text = "braccio indietro";
            this.btnBraccioIndietro.UseVisualStyleBackColor = true;
            // 
            // btnBraccioSinistra
            // 
            this.btnBraccioSinistra.Location = new System.Drawing.Point(257, 142);
            this.btnBraccioSinistra.Name = "btnBraccioSinistra";
            this.btnBraccioSinistra.Size = new System.Drawing.Size(75, 49);
            this.btnBraccioSinistra.TabIndex = 5;
            this.btnBraccioSinistra.Text = "braccio a sinistra";
            this.btnBraccioSinistra.UseVisualStyleBackColor = true;
            // 
            // btnBraccioDestra
            // 
            this.btnBraccioDestra.Location = new System.Drawing.Point(431, 142);
            this.btnBraccioDestra.Name = "btnBraccioDestra";
            this.btnBraccioDestra.Size = new System.Drawing.Size(75, 49);
            this.btnBraccioDestra.TabIndex = 4;
            this.btnBraccioDestra.Text = "braccio a destra";
            this.btnBraccioDestra.UseVisualStyleBackColor = true;
            // 
            // btnBraccioSu
            // 
            this.btnBraccioSu.Location = new System.Drawing.Point(344, 86);
            this.btnBraccioSu.Name = "btnBraccioSu";
            this.btnBraccioSu.Size = new System.Drawing.Size(75, 49);
            this.btnBraccioSu.TabIndex = 7;
            this.btnBraccioSu.Text = "braccio su";
            this.btnBraccioSu.UseVisualStyleBackColor = true;
            // 
            // btnBraccioGiù
            // 
            this.btnBraccioGiù.Location = new System.Drawing.Point(344, 202);
            this.btnBraccioGiù.Name = "btnBraccioGiù";
            this.btnBraccioGiù.Size = new System.Drawing.Size(75, 49);
            this.btnBraccioGiù.TabIndex = 6;
            this.btnBraccioGiù.Text = "braccio giù";
            this.btnBraccioGiù.UseVisualStyleBackColor = true;
            // 
            // btnApriPinza
            // 
            this.btnApriPinza.Location = new System.Drawing.Point(257, 411);
            this.btnApriPinza.Name = "btnApriPinza";
            this.btnApriPinza.Size = new System.Drawing.Size(103, 49);
            this.btnApriPinza.TabIndex = 2;
            this.btnApriPinza.Text = "apri pinza";
            this.btnApriPinza.UseVisualStyleBackColor = true;
            // 
            // btnChiudiPinza
            // 
            this.btnChiudiPinza.Location = new System.Drawing.Point(403, 411);
            this.btnChiudiPinza.Name = "btnChiudiPinza";
            this.btnChiudiPinza.Size = new System.Drawing.Size(103, 49);
            this.btnChiudiPinza.TabIndex = 3;
            this.btnChiudiPinza.Text = "chiudi pinza";
            this.btnChiudiPinza.UseVisualStyleBackColor = true;
            // 
            // frmControlli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 531);
            this.Controls.Add(this.btnBraccioSu);
            this.Controls.Add(this.btnBraccioGiù);
            this.Controls.Add(this.btnBraccioSinistra);
            this.Controls.Add(this.btnBraccioDestra);
            this.Controls.Add(this.btnChiudiPinza);
            this.Controls.Add(this.btnBraccioIndietro);
            this.Controls.Add(this.btnApriPinza);
            this.Controls.Add(this.btnBraccioAvanti);
            this.Controls.Add(this.label1);
            this.Name = "frmControlli";
            this.Text = "Controlli";
            this.Load += new System.EventHandler(this.frmControlli_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBraccioAvanti;
        private System.Windows.Forms.Button btnBraccioIndietro;
        private System.Windows.Forms.Button btnBraccioSinistra;
        private System.Windows.Forms.Button btnBraccioDestra;
        private System.Windows.Forms.Button btnBraccioSu;
        private System.Windows.Forms.Button btnBraccioGiù;
        private System.Windows.Forms.Button btnApriPinza;
        private System.Windows.Forms.Button btnChiudiPinza;
    }
}