using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommesseDDL;
using DBManagerDLL;
using AlertDLL;

namespace PLC_Manager.View
{
    public partial class formModificaCommessa : Form
    {
        private string codiceCommessa;
        private Commesse commesse;
        private int idProdotto, idCliente, numeroPezzi;
        DateTime dataConsegna;
        public formModificaCommessa()
        {
            InitializeComponent();
        }
        public formModificaCommessa(string codiceCommessa, Commesse commesse, int idProdotto, int idCliente, int numeroPezzi, DateTime dataConsegna)
        {
            InitializeComponent();
            this.codiceCommessa = codiceCommessa;
            this.commesse = commesse;
            this.idProdotto = idProdotto;
            this.idCliente = idCliente;
            this.numeroPezzi = numeroPezzi;
            this.dataConsegna = dataConsegna;
        }


        private void formModificaCommessa_Load(object sender, EventArgs e)
        {
            setComboBox();
            nudNumeroPezzi.Value = numeroPezzi;
            txtCodiceCommessa.Text = codiceCommessa;
            dateTimePkrDataConsegna.Value = dataConsegna.Date;
            dateTimePkrOraConsegna.Value = Convert.ToDateTime(dataConsegna.Date + dataConsegna.TimeOfDay);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnModificaCommessa_Click(object sender, EventArgs e)
        {
            string newNominativoCliente = ((KeyValuePair<int, string>)cmbClienti.SelectedItem).Value;
            string newDescrizioneProdotto = ((KeyValuePair<int, string>)cmbProdotti.SelectedItem).Value;
            int newIDCliente = ((KeyValuePair<int, string>)cmbClienti.SelectedItem).Key;
            int newIDProdotto = ((KeyValuePair<int, string>)cmbProdotti.SelectedItem).Key;
            DateTime newDataConsegna = dateTimePkrDataConsegna.Value.Date + dateTimePkrOraConsegna.Value.TimeOfDay;

            commesse.modificaCommessa(codiceCommessa, newDescrizioneProdotto, Convert.ToInt32(nudNumeroPezzi.Value), newNominativoCliente, newDataConsegna);
            DBManager.modificaCommessa(codiceCommessa, newIDProdotto, Convert.ToInt32(nudNumeroPezzi.Value), newIDCliente, newDataConsegna);

            Alert("Commessa modificata", AlertDLL.Alert.enmType.Info);
            this.Close();
        }

        private void setComboBox()
        {
            //riempire combobox con dati letti da DB
            Dictionary<int, string> clienti = DBManager.leggiClienti();
            cmbClienti.DataSource = new BindingSource(clienti, null);
            if (clienti.Count > 0)
            {
                cmbClienti.DisplayMember = "Value";
                cmbClienti.ValueMember = "Key";
            }
            else
            {
                cmbClienti.SelectedText = "Nessun dato disponibile";
                //disattivare bottone crea
            }

            Dictionary<int, string> prodotti = DBManager.leggiProdotti();
            cmbProdotti.DataSource = new BindingSource(prodotti, null);
            if (prodotti.Count > 0)
            {
                cmbProdotti.DisplayMember = "Value";
                cmbProdotti.ValueMember = "Key";
            }
            else
            {
                cmbProdotti.SelectedText = "Nessun dato disponibile";
                //disattivare bottone crea
            }

            cmbClienti.SelectedValue = idCliente;
            cmbProdotti.SelectedValue = idProdotto;
        }

        public void Alert(string msg, Alert.enmType type)
        {
            Alert frm = new Alert();
            frm.showAlert(msg, type);
        }
    }
}
