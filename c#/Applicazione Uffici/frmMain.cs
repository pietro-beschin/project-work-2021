using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Applicazione_Uffici
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void DisplayForm(Form myForm)
        {
            myForm.MdiParent = this;
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.ControlBox = false;
            myForm.MaximizeBox = false;
            myForm.MinimizeBox = false;
            myForm.ShowIcon = false;
            myForm.Text = "";
            myForm.Dock = DockStyle.Fill;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form myForm = new frmCommesse();
            DisplayForm(myForm);
            myForm.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form myForm = new frmControlli();
            DisplayForm(myForm);
            myForm.Show();
        }
    }
}
