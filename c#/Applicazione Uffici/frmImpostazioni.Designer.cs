
namespace Applicazione_Uffici
{
    partial class frmImpostazioni
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtWatchDog = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOffsetVelocita = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAvvisoPerOperatore = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtControlWord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 296);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "Watch dog";
            // 
            // txtWatchDog
            // 
            this.txtWatchDog.Location = new System.Drawing.Point(282, 293);
            this.txtWatchDog.Margin = new System.Windows.Forms.Padding(4);
            this.txtWatchDog.Name = "txtWatchDog";
            this.txtWatchDog.Size = new System.Drawing.Size(345, 22);
            this.txtWatchDog.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 244);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "Offset velocità macchina";
            // 
            // txtOffsetVelocita
            // 
            this.txtOffsetVelocita.Location = new System.Drawing.Point(282, 240);
            this.txtOffsetVelocita.Margin = new System.Windows.Forms.Padding(4);
            this.txtOffsetVelocita.Name = "txtOffsetVelocita";
            this.txtOffsetVelocita.Size = new System.Drawing.Size(345, 22);
            this.txtOffsetVelocita.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 191);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Avviso per operatore";
            // 
            // txtAvvisoPerOperatore
            // 
            this.txtAvvisoPerOperatore.Location = new System.Drawing.Point(282, 187);
            this.txtAvvisoPerOperatore.Margin = new System.Windows.Forms.Padding(4);
            this.txtAvvisoPerOperatore.Name = "txtAvvisoPerOperatore";
            this.txtAvvisoPerOperatore.Size = new System.Drawing.Size(345, 22);
            this.txtAvvisoPerOperatore.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 139);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Control word";
            // 
            // txtControlWord
            // 
            this.txtControlWord.Location = new System.Drawing.Point(282, 136);
            this.txtControlWord.Margin = new System.Windows.Forms.Padding(4);
            this.txtControlWord.Name = "txtControlWord";
            this.txtControlWord.Size = new System.Drawing.Size(345, 22);
            this.txtControlWord.TabIndex = 10;
            // 
            // frmImpostazioni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtWatchDog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOffsetVelocita);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAvvisoPerOperatore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtControlWord);
            this.Name = "frmImpostazioni";
            this.Text = "frmImpostazioni";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWatchDog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOffsetVelocita;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAvvisoPerOperatore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtControlWord;
    }
}