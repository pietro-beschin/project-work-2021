using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlertDLL;

namespace PLC_Manager.View
{
    public partial class formMessaggi : Form
    {
        public formMessaggi(string messaggioOperatore)
        {
            InitializeComponent();

            txtMessaggioDaOperatore.Text = messaggioOperatore;
        }

        private void btnInviaMessaggio_Click(object sender, EventArgs e)
        {
            MainForm f = (MainForm)this.Owner;

            if (f.scriviMessaggioPerOperatore(txtMessaggio.Text))
            {
                Alert("Messaggio inviato", AlertDLL.Alert.enmType.Success);
            }   
            else
            {
                Alert("Messaggio non inviato", AlertDLL.Alert.enmType.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Alert(string msg, Alert.enmType type)
        {
            Alert frm = new Alert();
            frm.showAlert(msg, type);
        }
    }
}
