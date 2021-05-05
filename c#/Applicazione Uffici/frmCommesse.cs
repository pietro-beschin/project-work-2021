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
    public partial class frmCommesse : Form
    {
        DataTable dt = new DataTable();

        public frmCommesse()
        {
            InitializeComponent();
            
            dt.Columns.Add(new DataColumn("Cliente"));
            dt.Columns.Add(new DataColumn("Commessa"));
            dt.Columns.Add(new DataColumn("Prodotto"));
            dt.Columns.Add(new DataColumn("Pezzi richiesti"));
        }

        private void frmCommesse_Load(object sender, EventArgs e)
        {

        }

        private void btnCreateCommessa_Click(object sender, EventArgs e)
        {
            dt.Rows.Add(txtCliente.Text, txtCommessa.Text, txtProdotto.Text, txtPezziRichiesti.Text);

            dgvCommesse.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
