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
    public partial class frmControlli : Form
    {
        public frmControlli()
        {
            InitializeComponent();
        }

        private void frmControlli_Load(object sender, EventArgs e)
        {
            
        }

        private void btnBraccioAvanti_Click(object sender, EventArgs e)
        {
            btnBraccioIndietro.Enabled = false;
            btnBraccioAvanti.Enabled = false;
            //quando il plc invia il segnale che è arrivato a fine corsa
                btnBraccioIndietro.Enabled = true;
                btnBraccioAvanti.Enabled = true;
        }

        private void btnBraccioIndietro_Click(object sender, EventArgs e)
        {
            btnBraccioIndietro.Enabled = false;
            btnBraccioAvanti.Enabled = false;
            //quando il plc invia il segnale che è arrivato a fine corsa
                btnBraccioIndietro.Enabled = true;
                btnBraccioAvanti.Enabled = true;
        }
    }
}
