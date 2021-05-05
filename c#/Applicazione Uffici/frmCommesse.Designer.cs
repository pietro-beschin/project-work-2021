
namespace Applicazione_Uffici
{
    partial class frmCommesse
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
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.dgvCommesse = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCommessa = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPezziRichiesti = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProdotto = new System.Windows.Forms.TextBox();
            this.btnCreateCommessa = new System.Windows.Forms.Button();
            this.btnAvviaCommessa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommesse)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(214, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Commesse";
            // 
            // txtCliente
            // 
            this.txtCliente.Location = new System.Drawing.Point(181, 81);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(260, 20);
            this.txtCliente.TabIndex = 1;
            // 
            // dgvCommesse
            // 
            this.dgvCommesse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCommesse.Location = new System.Drawing.Point(620, 72);
            this.dgvCommesse.Name = "dgvCommesse";
            this.dgvCommesse.RowHeadersWidth = 51;
            this.dgvCommesse.Size = new System.Drawing.Size(508, 333);
            this.dgvCommesse.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cliente";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Commessa";
            // 
            // txtCommessa
            // 
            this.txtCommessa.Location = new System.Drawing.Point(181, 123);
            this.txtCommessa.Name = "txtCommessa";
            this.txtCommessa.Size = new System.Drawing.Size(260, 20);
            this.txtCommessa.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Pezzi richiesti";
            // 
            // txtPezziRichiesti
            // 
            this.txtPezziRichiesti.Location = new System.Drawing.Point(181, 208);
            this.txtPezziRichiesti.Name = "txtPezziRichiesti";
            this.txtPezziRichiesti.Size = new System.Drawing.Size(260, 20);
            this.txtPezziRichiesti.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(100, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Prodotto";
            // 
            // txtProdotto
            // 
            this.txtProdotto.Location = new System.Drawing.Point(181, 166);
            this.txtProdotto.Name = "txtProdotto";
            this.txtProdotto.Size = new System.Drawing.Size(260, 20);
            this.txtProdotto.TabIndex = 6;
            // 
            // btnCreateCommessa
            // 
            this.btnCreateCommessa.Location = new System.Drawing.Point(328, 254);
            this.btnCreateCommessa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCreateCommessa.Name = "btnCreateCommessa";
            this.btnCreateCommessa.Size = new System.Drawing.Size(111, 24);
            this.btnCreateCommessa.TabIndex = 10;
            this.btnCreateCommessa.Text = "CREA COMMESSA";
            this.btnCreateCommessa.UseVisualStyleBackColor = true;
            this.btnCreateCommessa.Click += new System.EventHandler(this.btnCreateCommessa_Click);
            // 
            // btnAvviaCommessa
            // 
            this.btnAvviaCommessa.Location = new System.Drawing.Point(1006, 426);
            this.btnAvviaCommessa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAvviaCommessa.Name = "btnAvviaCommessa";
            this.btnAvviaCommessa.Size = new System.Drawing.Size(111, 24);
            this.btnAvviaCommessa.TabIndex = 11;
            this.btnAvviaCommessa.Text = "AVVIA COMMESSA";
            this.btnAvviaCommessa.UseVisualStyleBackColor = true;
            this.btnAvviaCommessa.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmCommesse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 531);
            this.Controls.Add(this.btnAvviaCommessa);
            this.Controls.Add(this.btnCreateCommessa);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPezziRichiesti);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtProdotto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCommessa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvCommesse);
            this.Controls.Add(this.txtCliente);
            this.Controls.Add(this.label1);
            this.Name = "frmCommesse";
            this.Text = "Commesse";
            this.Load += new System.EventHandler(this.frmCommesse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommesse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.DataGridView dgvCommesse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCommessa;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPezziRichiesti;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProdotto;
        private System.Windows.Forms.Button btnCreateCommessa;
        private System.Windows.Forms.Button btnAvviaCommessa;
    }
}