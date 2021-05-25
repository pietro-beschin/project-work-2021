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
using PLCConnectionDLL;
using Sharp7;
using DBManagerDLL;
using AlertDLL;
using System.Threading;
using System.Configuration;

namespace PLC_Manager.View
{
    public partial class MainForm : Form
    {
        #region VariabiliGlobali
        Commesse commesse = new Commesse();         //da riempire con dati del db (solo commesse NON CONCLUSE)
        Dictionary<int, string> clienti;
        Dictionary<int, string> prodotti;


        private S7Client Client;
        private byte[] db1Buffer = new byte[281];       //spazio occupato nel db
        private byte[] db2Buffer = new byte[233];
        private int result;
        private CDataBlock db1 = new CDataBlock();
        private CDataBlock db2 = new CDataBlock();

        string messaggioOperatore = "";             //messaggio per operatore
        bool mostrareMessaggio = false;             

        int allarmi = 0;                            //codice allarme
        bool mostraAlertAllarme = true;
        List<string> listaGuasti = new List<string>();  //lista dei guasti
        #endregion

        #region FormPrincipale
        public MainForm()
        {
            InitializeComponent();
            this.Client = new S7Client();
            this.db1.DBName = "DB_PC-PLC";
            this.db2.DBName = "DB_PLC-PC";
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.storicoCommesseTableAdapter1.Fill(this.dbProjectWork2021DataSet4.storicoCommesse);
            //controllo del pannello laterale e del laterale-superiore
            formDock.SubscribeControlToDragEvents(pannelloLaterale);
            formDock.SubscribeControlToDragEvents(pannelloLateraleSuperiore);

            //disabilitazione dei bottoni
            btnEseguiCommessa.Enabled = false;
            btnAbilitaPLC.Enabled = false;
            btnStartPLC.Enabled = false;
            switchStatoSimulazione.Enabled = false;
            switchOffset.Enabled = false;
            btnDialogoPcPlc.Enabled = false;

            //set delle combobox di clienti e prodotti
            setComboBoxPaginaCommesse();
            //lettura commesse non concluse da db
            commesse = DBManager.leggiCommesseDalDB();
            //aggiornamento datagridview commesse
            AggiornaDataGridViewCommesse();
            //svuotare le textbox
            setNullTextBoxes();
        }
        //i seguenti metodi visualizzano la pagina corretta e spostano l'indicatore laterale
        private void btnGestioneCommesse_Click(object sender, EventArgs e)
        {
            SpostamentoIndicatore(sender);
            applicationPages.SetPage("Commesse");
        }

        private void btnAnagrafica_Click(object sender, EventArgs e)
        {
            SpostamentoIndicatore(sender);
            applicationPages.SetPage("Anagrafica");
        }

        private void btnClientiProdotti_Click(object sender, EventArgs e)
        {
            SpostamentoIndicatore(sender);
            applicationPages.SetPage("ClientiProdotti");
        }

        private void btnGuasti_Click(object sender, EventArgs e)
        {
            SpostamentoIndicatore(sender);
            applicationPages.SetPage("Guasti");
        }

        //spostamento dell'indicatore laterale sul pulsante premuto
        private void SpostamentoIndicatore(object sender)
        {
            indicator.Top = ((Control)sender).Top;
        }

        //metodo per mostrare l'Alert
        public void Alert(string msg, Alert.enmType type)
        {
            Alert frm = new Alert();
            frm.showAlert(msg, type);
        }
        #endregion

        #region PaginaCommesse
        #region Funzioni
        private void setComboBoxPaginaCommesse()
        {
            //riempire combobox con dati letti da DB
            clienti = DBManager.leggiClienti();
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

            prodotti = DBManager.leggiProdotti();
            cmbProdotti.DataSource = new BindingSource(prodotti, null);
            if (prodotti.Count > 0)
            {
                cmbProdotti.DisplayMember = "Value";
                cmbProdotti.ValueMember = "Key";
            }
            else
            {
                cmbProdotti.SelectedText = "Nessun dato disponibile";
            }

        }
        //aggiornamento della datagridview
        private void AggiornaDataGridViewCommesse()
        {
            //riordinamento delle commesse con ordine: in esecuzione - in coda - disattive
            commesse.ordina();

            dgvCommesse.Rows.Clear();
            foreach (var commessa in commesse.ListaCommesse)
            {
                if(commessa.DataEsecuzione.Year != 1 && commessa.DataEsecuzione.Month != 1 && commessa.DataEsecuzione.Day != 1)
                    dgvCommesse.Rows.Add(commessa.CodiceCommessa, commessa.Prodotto, commessa.PezziDaProdurre, commessa.Stato, commessa.NominativoCliente, commessa.DataEsecuzione, commessa.DataConsegna);
                else
                    dgvCommesse.Rows.Add(commessa.CodiceCommessa, commessa.Prodotto, commessa.PezziDaProdurre, commessa.Stato, commessa.NominativoCliente, "NON DISPONIBILE", commessa.DataConsegna);
            }

            //se non ci sono commesse, vengono disattivati i pulsanti
            if (dgvCommesse.Rows.Count <= 0)
            {
                btnModifica.Enabled = false;
                btnEseguiCommessa.Enabled = false;
                btnEliminaCommessa.Enabled = false;
            }
            else
            {
                if (btnConnessionePLC.Text == "DISCONNETTI PLC")
                    btnEseguiCommessa.Enabled = true;
                btnModifica.Enabled = true;
                btnEliminaCommessa.Enabled = true;
            }
        }
        //metodo per controllare se esiste una commessa in esecuzione
        private bool presenzaCommesseInEsecuzione()
        {
            if (commesse.ListaCommesse.Count > 0 && commesse.ListaCommesse[0].Stato == statoCommesa.inEsecuzione)
                return true;
            return false;
        }
        //metodo per eseguire la prossima commessa in coda
        public bool MandaInEsecuzioneCommessaSuccessiva()
        {
            string codiceCommessa, prodotto;
            int numeroPezzi;
            DateTime dataConsegna;

            DateTime ora = DateTime.Now;

            if (commesse.prossimaCommessaInCoda(out codiceCommessa, out prodotto, out numeroPezzi, out dataConsegna) && eseguiCommessaSuPLC(codiceCommessa, prodotto, numeroPezzi, dataConsegna, ora))
            {
                commesse.MandaInEsecuzioneCommessaSuccessiva();
                DBManager.updateStatoCommessa(commesse.ListaCommesse[0].CodiceCommessa, "inEsecuzione");
                DBManager.updateDataEsecuzione(commesse.ListaCommesse[0].CodiceCommessa, ora);

                commesse.ListaCommesse[0].DataEsecuzione = ora;

                return true;
            }
            return false;
        }
        private void disconnettiPLC()
        {
            this.Client.Disconnect();
            btnConnessionePLC.Text = "CONNETTI";
            btnAbilitaPLC.Enabled = false;
            btnAbilitaPLC.Text = "ABILITA";
            btnStartPLC.Enabled = false;
            btnStartPLC.Text = "AVVIA";
            tmrLetturaDatiPLC.Enabled = false;
            tmrWatchDog.Enabled = false;
            btnEseguiCommessa.Enabled = false;
            btnDialogoPcPlc.Enabled = false;
            switchStatoSimulazione.Enabled = false;
            switchOffset.Enabled = false;

            //set delle textbox a 0
            setNullTextBoxes();
        }
        //scrittura della commmessa sul plc per l'esecuzione
        private bool eseguiCommessaSuPLC(string codiceCommessa, string prodotto, int pezziDaProdurre, DateTime dataConsegna, DateTime dataEsecuzione)
        {
            CSerialDeserial.ReadFile(ref this.db1);
            S7.SetStringAt(this.db1Buffer, 0, 50, codiceCommessa);
            S7.SetStringAt(this.db1Buffer, 52, 50, prodotto);
            S7.SetDIntAt(this.db1Buffer, 104, pezziDaProdurre);
            S7.SetStringAt(this.db1Buffer, 218, 30, dataConsegna.ToString());
            S7.SetStringAt(this.db1Buffer, 250, 30, dataEsecuzione.ToString());

            this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

            return result == 0;
        }
        //svuotare parzialmente i dati della commessa sul plc
        private bool eliminaEsecuzioneCommessaSulPLC()
        {
            //codice commessa e prodotto vengono settati a stringa vuota e il numero pezzi a 0
            CSerialDeserial.ReadFile(ref this.db1);
            S7.SetStringAt(this.db1Buffer, 0, 50, "");
            S7.SetStringAt(this.db1Buffer, 52, 50, "");
            S7.SetDIntAt(this.db1Buffer, 104, 0);

            this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

            return result == 0;
        }
        //scrittura del messaggio per l'operatore sul plc
        public bool scriviMessaggioPerOperatore(string messaggio)
        {
            try
            {
                CSerialDeserial.ReadFile(ref this.db1);
                S7.SetStringAt(this.db1Buffer, 110, 100, messaggio);

                this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                return result == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void setTempoRimanenteEProgressBar(int velocitàMacchina, int pezziProdotti)
        {
            string strTempoRimanente = "Non Disponibile";

            if (presenzaCommesseInEsecuzione())
            {
                int numeroPezzi = commesse.ListaCommesse[0].PezziDaProdurre;
                int pezziMancanti = numeroPezzi - pezziProdotti;

                progressCommessa.Value = pezziProdotti * 100 / numeroPezzi;

                if (velocitàMacchina != 0 && pezziProdotti != numeroPezzi)
                {
                    double tempoRimanente = ((double)pezziMancanti) / velocitàMacchina;

                    double tempoRimanenteInSecondi = tempoRimanente * 60 * 60;

                    DateTime data = DateTime.Now;
                    data = data.AddSeconds(tempoRimanenteInSecondi);

                    strTempoRimanente = data.ToString();
                }
            }
            else
            {
                progressCommessa.Value = 0;
            }

            txtTempoRimanente.Text = strTempoRimanente;
        }
        private string calcoloStatoMacchina(int stato)
        {
            string result = "";
            switch (stato)
            {
                case 1:
                    result = "CICLO AUTOMATICO";
                    break;
                case 3:
                    result = "START";
                    break;
                case 4:
                    result = "MANUALE";
                    break;
                case 9:
                    result = "STOP";
                    break;
                case 48:
                    result = "EMERGENZA";
                    break;
                case 49:
                    result = "RESET EMERGENZA";
                    break;
                case 52:
                    result = "RESET EMERGENZA";
                    break;
                case 32:
                    result = "ALLARME";
                    break;
                case 65:
                    result = "CONCLUSO";
                    break;
                case 67:
                    result = "CONCLUSO";
                    break;
                case 73:
                    result = "CONCLUSO";
                    break;
                case 112:
                    result = "EMERGENZA";
                    break;
                case 113:
                    result = "RESET EMERGENZA";
                    break;
                default:
                    result = "NON DISPONIBILE";
                    break;
            }

            return result;
        }
        private void NotificaMessaggioDaOperatore()
        {
            Alert("Ricevuto messaggio da operatore", AlertDLL.Alert.enmType.Info);
        }
        //lettura della control word del plc, controlla se la macchina è abilitata e in start e setta i pulsanti di conseguenza
        private void leggiControlWordSettaPulsanti()
        {
            //lettura dei dati dal PLC
            this.result = this.Client.DBRead(1, 0, this.db1Buffer.Length, this.db1Buffer);

            int controlWord = S7.GetUSIntAt(this.db1Buffer, 108);

            CSerialDeserial.WriteFile(this.db1);


            if (controlWord == 0)   //macchina diabilitata
            {
                btnAbilitaPLC.Text = "ABILITA";
                btnStartPLC.Enabled = false;
                btnStartPLC.Text = "AVVIA";
            }
            else
            {
                if (controlWord == 1)    //macchina abilitata
                {
                    btnAbilitaPLC.Text = "DISABILITA";
                    btnStartPLC.Enabled = true;
                    btnStartPLC.Text = "AVVIA";
                }
                else
                {
                    if (controlWord == 3)    //macchina abilitata e in start
                    {
                        btnAbilitaPLC.Text = "DISABILITA";
                        btnStartPLC.Enabled = true;
                        btnStartPLC.Text = "INTERROMPI";
                    }
                }
            }
        }
        private void setNullTextBoxes()
        {
            txtPezziProdotti.Text = "---";
            txtPezziScartati.Text = "---";
            txtVelocita.Text = "---";
            txtTempoRimanente.Text = "Non Disponibile";
            progressCommessa.Value = 0;
        }

        #endregion
        #region Eventi
        private void tabCommesse_Enter(object sender, EventArgs e)
        {
            setComboBoxPaginaCommesse();
        }
        private void btnConnessionePLC_Click(object sender, EventArgs e)
        {
            int Rack = Convert.ToInt32(ConfigurationManager.AppSettings["PLC_RACK"]);
            int Slot = Convert.ToInt32(ConfigurationManager.AppSettings["PLC_SLOT"]);
            string ip = ConfigurationManager.AppSettings["PLC_IP"];

            if (btnConnessionePLC.Text == "CONNETTI")
            {
                int Result = this.Client.ConnectTo(ip, Rack, Slot);

                if (Result == 0)
                {
                    //attivazione di bottoni e interfaccia
                    btnAbilitaPLC.Enabled = true;
                    btnConnessionePLC.Text = "DISCONNETTI";
                    tmrLetturaDatiPLC.Enabled = true;
                    tmrWatchDog.Enabled = true;

                    btnEseguiCommessa.Enabled = true;
                    btnDialogoPcPlc.Enabled = true;

                    switchStatoSimulazione.Enabled = true;
                    switchOffset.Enabled = true;

                    leggiControlWordSettaPulsanti();
                    if (presenzaCommesseInEsecuzione())
                    {
                        AggiornaDataGridViewCommesse();
                    }
                    else if (MandaInEsecuzioneCommessaSuccessiva())
                        AggiornaDataGridViewCommesse();

                    Alert("PLC connesso", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Connessione al PLC fallita", AlertDLL.Alert.enmType.Error);
                }
            }
            else
            {
                disconnettiPLC();
                Alert("PLC disconnesso", AlertDLL.Alert.enmType.Success);
            }
        }
        private void btnCreaCommessa_Click(object sender, EventArgs e)
        {
            string nominativoCliente = ((KeyValuePair<int, string>)cmbClienti.SelectedItem).Value;
            int IDCliente = ((KeyValuePair<int, string>)cmbClienti.SelectedItem).Key;
            string descrizioneProdotto = ((KeyValuePair<int, string>)cmbProdotti.SelectedItem).Value;
            int IDProdotto = ((KeyValuePair<int, string>)cmbProdotti.SelectedItem).Key;
            DateTime dataConsegna = dateTimePkrDataConsegna.Value.Date + dateTimePkrOraConsegna.Value.TimeOfDay;

            Commessa commessa = DBManager.scriviCommessaInDB(Convert.ToInt32(nudNumeroPezzi.Value), nominativoCliente, descrizioneProdotto, IDProdotto, IDCliente, dataConsegna, new DateTime());

            if (commessa == null)
            {
                Alert("Commessa non creata", AlertDLL.Alert.enmType.Error);
            }
            else
            {
                commesse.aggiungiCommessa(commessa);
                AggiornaDataGridViewCommesse();
                Alert("Commessa creata", AlertDLL.Alert.enmType.Success);

                if (btnConnessionePLC.Text != "CONNETTI")
                    btnEseguiCommessa.Enabled = true;
            }
        }
        private void btnModifica_Click(object sender, EventArgs e)
        {
            int index = dgvCommesse.CurrentRow.Index;
            string codiceCommessa = dgvCommesse.Rows[index].Cells["codiceCommessa"].Value.ToString();
            string stato = dgvCommesse.Rows[index].Cells["stato"].Value.ToString();
            string prodotto = dgvCommesse.Rows[index].Cells["Prodotto"].Value.ToString();
            string cliente = dgvCommesse.Rows[index].Cells["nominativoCliente"].Value.ToString();
            int numeroPezzi = Convert.ToInt32(dgvCommesse.Rows[index].Cells["PezziDaProdurre"].Value);
            DateTime dataConsegna = Convert.ToDateTime(dgvCommesse.Rows[index].Cells["dataConsegna"].Value);

            int IDCliente = clienti.FirstOrDefault(c => c.Value == cliente).Key;
            int IDProdotto = prodotti.FirstOrDefault(p => p.Value == prodotto).Key;

            if (stato != "inEsecuzione")
            {
                //aprire finestra di modifica
                Form formDiModifica = new formModificaCommessa(codiceCommessa, commesse, IDProdotto, IDCliente, numeroPezzi, dataConsegna);
                formDiModifica.ShowDialog();

                AggiornaDataGridViewCommesse();
            }
            else
            {
                Alert("Commessa non modificabile", AlertDLL.Alert.enmType.Warning);
            }
        }
        private void btnEliminaCommessa_Click(object sender, EventArgs e)
        {
            int index = dgvCommesse.CurrentRow.Index;
            string codiceCommessa = dgvCommesse.Rows[index].Cells["codiceCommessa"].Value.ToString();
            string stato = dgvCommesse.Rows[index].Cells["stato"].Value.ToString();

            if (stato != "inEsecuzione")
            {
                commesse.rimuoviCommessa(codiceCommessa);
                DBManager.rimuoviCommessaDalDB(codiceCommessa);
                AggiornaDataGridViewCommesse();

                Alert("Eliminazione effettuata", AlertDLL.Alert.enmType.Success);
            }
            else
            {
                if (MessageBox.Show("Stai cercando di rimuovere una commessa in esecuzione.\nLa commessa verrà terminata e classificata come \"fallita\", si desidera procedere?", "Conferma termine commessa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (btnConnessionePLC.Text == "CONNETTI")
                        Alert("Nessuna connessione con PLC", AlertDLL.Alert.enmType.Error);
                    else
                    {
                        if (eliminaEsecuzioneCommessaSulPLC())
                        {
                            commesse.rimuoviCommessa(codiceCommessa);
                            DBManager.updateStatoCommessa(codiceCommessa, "fallita");

                            Alert("Eliminazione effettuata", AlertDLL.Alert.enmType.Success);

                            if (MandaInEsecuzioneCommessaSuccessiva())
                            {
                                AggiornaDataGridViewCommesse();
                                Alert("Nuova commessa in esecuzione", AlertDLL.Alert.enmType.Info);
                            }
                            else
                            {
                                AggiornaDataGridViewCommesse();
                            }
                        }
                        else
                        {
                            Alert("Operazione fallita", AlertDLL.Alert.enmType.Error);
                        }
                    }
                }
            }
        }
        private void btnAbilitaPLC_Click(object sender, EventArgs e)
        {
            if (btnAbilitaPLC.Text == "ABILITA")
            {
                //scrivi abilitazione su PLC
                CSerialDeserial.ReadFile(ref this.db1);
                S7.SetUSIntAt(this.db1Buffer, 108, 1);

                this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                //se abilitazione OK attiva btnStartPLC
                if (result == 0)
                {
                    btnAbilitaPLC.Text = "BLOCCA";
                    btnStartPLC.Enabled = true;

                    Alert("PLC abilitato", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Abilitazione del PLC fallita", AlertDLL.Alert.enmType.Error);
                }
            }
            else
            {
                //scrivi abilitazione su PLC
                CSerialDeserial.ReadFile(ref this.db1);
                S7.SetUSIntAt(this.db1Buffer, 108, 0);

                this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                //se abilitazione OK attiva btnStartPLC
                if (result == 0)
                {
                    btnAbilitaPLC.Text = "ABILITA";
                    btnStartPLC.Enabled = false;
                    btnStartPLC.Text = "AVVIA";

                    Alert("PLC disabilitato", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Disabilitazione del PLC fallita", AlertDLL.Alert.enmType.Error);
                }
            }
        }
        //se esiste una commessa in esecuzione, la commessa viene messa in coda, altrimenti viene eseguita
        private void btnEseguiCommessa_Click(object sender, EventArgs e)
        {
            if (dgvCommesse.Rows.Count > 0)
            {
                int index = dgvCommesse.CurrentRow.Index;
                string codiceCommessa = dgvCommesse.Rows[index].Cells["codiceCommessa"].Value.ToString();
                string prodotto = dgvCommesse.Rows[index].Cells["prodotto"].Value.ToString();
                int pezziDaProdurre = Convert.ToInt32(dgvCommesse.Rows[index].Cells["PezziDaProdurre"].Value.ToString());
                string stato = dgvCommesse.Rows[index].Cells["stato"].Value.ToString();
                DateTime dataConsegna = Convert.ToDateTime(dgvCommesse.Rows[index].Cells["dataConsegna"].Value.ToString());

                if (stato != "inEsecuzione")
                {
                    if (!presenzaCommesseInEsecuzione())
                    {
                        DateTime dataEsecuzione = DateTime.Now;
                        if (eseguiCommessaSuPLC(codiceCommessa, prodotto, pezziDaProdurre, dataConsegna, dataEsecuzione))
                        {
                            commesse.eseguiCommessa(codiceCommessa, dataEsecuzione);
                            DBManager.updateStatoCommessa(codiceCommessa, "inEsecuzione");
                            DBManager.updateDataEsecuzione(codiceCommessa, dataEsecuzione);
                            AggiornaDataGridViewCommesse();

                            Alert("Commessa in esecuzione", AlertDLL.Alert.enmType.Success);
                        }
                        else
                        {
                            Alert("Operazione fallita", AlertDLL.Alert.enmType.Error);
                        }
                    }
                    else
                    {
                        commesse.mettiInCodaCommessa(codiceCommessa);
                        DBManager.updateStatoCommessa(codiceCommessa, "inCoda");
                        AggiornaDataGridViewCommesse();

                        Alert("Commessa in coda", AlertDLL.Alert.enmType.Success);
                    }
                }
                else
                {
                    Alert("Commessa già in esecuzione", AlertDLL.Alert.enmType.Warning);
                }
            }
        }
        private void btnStartPLC_Click(object sender, EventArgs e)
        {
            if (btnStartPLC.Text == "AVVIA")
            {
                //scrivi start su PLC
                CSerialDeserial.ReadFile(ref this.db1);
                S7.SetUSIntAt(this.db1Buffer, 108, 3);

                this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                //se avvio OK attiva tutto
                if (result == 0)
                {
                    btnStartPLC.Text = "INTERROMPI";
                    //set di tutto a enable
                    //setGUI(true);

                    Alert("Ciclo avviato", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Avvio del ciclo fallito", AlertDLL.Alert.enmType.Error);
                }

                //se ci sono commesse in coda, viene eseguita la prima
                MandaInEsecuzioneCommessaSuccessiva();
                AggiornaDataGridViewCommesse();
            }
            else
            {
                //scrivi stop su PLC
                CSerialDeserial.ReadFile(ref this.db1);
                S7.SetUSIntAt(this.db1Buffer, 108, 1);

                this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                //se OK cambia stato del bottone
                if (result == 0)
                {
                    btnStartPLC.Text = "AVVIA";
                    Alert("Ciclo interrotto", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Interruzione del ciclo fallita", AlertDLL.Alert.enmType.Error);
                }
            }
        }
        //apertura form per invio/ricezione messaggi dal PLC
        private void btnDialogoPcPlc_Click(object sender, EventArgs e)
        {
            Form formMessaggi = new formMessaggi(messaggioOperatore);
            formMessaggi.Owner = this;
            formMessaggi.ShowDialog();

            mostrareMessaggio = false;
            btnDialogoPcPlc.BackgroundImage = PLC_Manager.Properties.Resources.icona_nessunaNotifica;
        }
        private void tmrLetturaDatiPLC_Tick(object sender, EventArgs e)
        {
            try
            {
                //lettura dei dati dal PLC
                this.result = this.Client.DBRead(2, 0, this.db2Buffer.Length, this.db2Buffer);

                string codiceCommessa = S7.GetStringAt(this.db1Buffer, 0);
                //string prodotto = S7.GetStringAt(this.db1Buffer, 52);
                int pezziProdotti = S7.GetDIntAt(this.db2Buffer, 104);
                int pezziScartatiScaricoPieno = S7.GetDIntAt(this.db2Buffer, 108);
                int pezziScartatiDifettosi = S7.GetDIntAt(this.db2Buffer, 112);
                int pezziScartatiTotali = pezziScartatiDifettosi + pezziScartatiScaricoPieno;
                int statoMacchina = S7.GetUSIntAt(this.db2Buffer, 116);
                int velocitàMacchina = S7.GetDIntAt(this.db2Buffer, 118);
                string newMessaggioOperatore = S7.GetStringAt(this.db2Buffer, 130);
                allarmi = S7.GetUSIntAt(this.db2Buffer, 232);

                CSerialDeserial.WriteFile(this.db2);

                //visualizza gli allarmi
                setVisualizzazioneAllarmi(allarmi);

                //se è presente un allarme e non è mai stata mostrato l'alert, viene mostrato 
                if(allarmi != 0 && mostraAlertAllarme)
                {
                    Alert("ALLARME!", AlertDLL.Alert.enmType.Error);
                    mostraAlertAllarme = false;
                }
                //se non ci sono allarmi, resetto la booleana
                if (allarmi == 0)
                    mostraAlertAllarme = true;

                string strStatoMacchina = calcoloStatoMacchina(statoMacchina);
                txtStatoMacchina.Text = strStatoMacchina;

                //se è presente una commessa in esecuzione sul plc, imposto le variabili opportune per la visualizzazione
                if (codiceCommessa != "")
                {
                    if (txtPezziProdotti.Text != ("" + pezziProdotti) || txtPezziScartati.Text != ("" + pezziScartatiTotali))
                        setTempoRimanenteEProgressBar(velocitàMacchina, pezziProdotti);

                    txtPezziProdotti.Text = "" + pezziProdotti;
                    txtPezziScartati.Text = "" + pezziScartatiTotali;
                    txtVelocita.Text = "" + velocitàMacchina;
                }
                else
                {
                    setNullTextBoxes();
                }

                //se il messaggio dell'operatore è diverso da l'ultimo che è arrivatom viene attivata la notifica
                if (newMessaggioOperatore != messaggioOperatore)
                {
                    messaggioOperatore = newMessaggioOperatore;
                    mostrareMessaggio = true;
                    NotificaMessaggioDaOperatore();
                }

                //set delle immagini per la notifica su pagina Gestione Commesse
                if (mostrareMessaggio)
                {
                    btnDialogoPcPlc.BackgroundImage = PLC_Manager.Properties.Resources.icona_NotficaPresenza;
                }
                else
                {
                    btnDialogoPcPlc.BackgroundImage = PLC_Manager.Properties.Resources.icona_nessunaNotifica;
                }

                //concusione della commessa
                if (strStatoMacchina == "CONCLUSO")
                {
                    DBManager.updateStatoCommessa(codiceCommessa, "completata");
                    commesse.completaCommessa(codiceCommessa);

                    Alert("Commessa conclusa", AlertDLL.Alert.enmType.Info);

                    Thread.Sleep(2000); //il codice viene bloccato per 2 secondi, per lasciare il tempo al servizio di leggere i dati

                    eliminaEsecuzioneCommessaSulPLC();
                    MandaInEsecuzioneCommessaSuccessiva();
                    AggiornaDataGridViewCommesse();
                }
            }
            catch (Exception)
            {
                Alert("Errore non gestito", AlertDLL.Alert.enmType.Error);
            }
        }

        private void tmrWatchDog_Tick(object sender, EventArgs e)
        {
            try
            {
                this.result = this.Client.DBRead(1, 0, this.db1Buffer.Length, this.db1Buffer);
                int watchDog = S7.GetUSIntAt(this.db1Buffer, 216);  //1 se scrivo da pc, 0 se scrivo da plc

                if (watchDog == 0)
                {
                    CSerialDeserial.ReadFile(ref this.db1);
                    S7.SetUSIntAt(this.db1Buffer, 216, 1);

                    this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);
                }
                else
                {
                    disconnettiPLC();
                    Alert("PLC disconnesso", AlertDLL.Alert.enmType.Error);
                }
            }
            catch (Exception)
            {
                disconnettiPLC();
                Alert("PLC disconnesso", AlertDLL.Alert.enmType.Error);
            } 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            tmrLetturaDatiPLC.Enabled = false;
            tmrWatchDog.Enabled = false;
            this.Close();
        }
        #endregion

        #endregion

        #region PaginaClientiProdotti
        #region Eventi
        private void btnAggiungiCliente_Click(object sender, EventArgs e)
        {
            if (txtCliente.Text != "")
            {
                bool result = DBManager.aggiungiCliente(txtCliente.Text);
                if (result)
                {
                    Alert("Cliente creato", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Errore nella creazione", AlertDLL.Alert.enmType.Error);
                }

                txtCliente.Text = "";
                setComboBoxPaginaClientiProdotti();
            }
            else
            {
                Alert("Impossibile creare cliente senza nome", AlertDLL.Alert.enmType.Error);
            }
        }
        private void btnAggiungiProdotto_Click(object sender, EventArgs e)
        {
            if (txtProdotto.Text != "")
            {
                bool result = DBManager.aggiungiProdotto(txtProdotto.Text);
                if (result)
                {
                    Alert("Prodotto creato", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    Alert("Errore nella creazione", AlertDLL.Alert.enmType.Error);
                }

                setComboBoxPaginaClientiProdotti();
                txtProdotto.Text = "";
            }
            else
            {
                Alert("Impossibile creare prodotto senza nome", AlertDLL.Alert.enmType.Error);
            }
        }
        private void tabGestioneClientiProdotti_Enter(object sender, EventArgs e)
        {
            setComboBoxPaginaClientiProdotti();
        }
        private void btnEliminaCliente_Click(object sender, EventArgs e)
        {
            int currentIndex = ((KeyValuePair<int, string>)cmbClienteClienti.SelectedItem).Key;
            bool result = DBManager.eliminaCliente(currentIndex);
            if (result)
            {
                Alert("Cliente eliminato", AlertDLL.Alert.enmType.Success);
                setComboBoxPaginaClientiProdotti();
            }
            else
            {
                Alert("Errore nell'eliminazione", AlertDLL.Alert.enmType.Error);
            }

        }
        private void btnEliminaProdotti_Click(object sender, EventArgs e)
        {
            int currentIndex = ((KeyValuePair<int, string>)cmbProdottoProdotti.SelectedItem).Key;
            bool result = DBManager.eliminaProdotto(currentIndex);
            if (result)
            {
                Alert("Prodotto eliminato", AlertDLL.Alert.enmType.Success);
                setComboBoxPaginaClientiProdotti();
            }
            else
            {
                Alert("Errore nell'eliminazione", AlertDLL.Alert.enmType.Error);
            }
        }
        #endregion
        #region Funzioni
        private void setComboBoxPaginaClientiProdotti()
        {
            //riempire combobox con dati letti da DB
            clienti = DBManager.leggiClienti();
            cmbClienteClienti.DataSource = new BindingSource(clienti, null);
            if (clienti.Count > 0)
            {
                cmbClienteClienti.DisplayMember = "Value";
                cmbClienteClienti.ValueMember = "Key";
            }
            else
            {
                cmbClienteClienti.SelectedText = "Nessun dato disponibile";
                //disattivare bottone crea
            }

            prodotti = DBManager.leggiProdotti();
            cmbProdottoProdotti.DataSource = new BindingSource(prodotti, null);
            if (prodotti.Count > 0)
            {
                cmbProdottoProdotti.DisplayMember = "Value";
                cmbProdottoProdotti.ValueMember = "Key";
            }
            else
            {
                cmbProdottoProdotti.SelectedText = "Nessun dato disponibile";
                //disattivare bottone crea
            }

        }
        #endregion
        #endregion

        #region PaginaGuasti
        #region Eventi
        //se cambia lo stato del bottone che attiva la simulazione
        private void switchStatoSimulazione_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchStatoSimulazione.Checked)
            {
                txtStatoSimulazione.Text = "ON";

                //avvio simulazione
                setSwitchGuastiStatus(true);
                Alert("Simulazione avviata", AlertDLL.Alert.enmType.Success);
                simulaGuasto();
            }
            else
            {
                txtStatoSimulazione.Text = "OFF";
                //blocco simulazione
                setSwitchGuastiStatus(false);
                Alert("Simulazione bloccata", AlertDLL.Alert.enmType.Success);
                cancellaSimulazioneGuasto();
            }
        }
        //se è attivo lo switch dell'offset, esso viene attivato sul plc
        private void nudDelayTime_ValueChanged(object sender, EventArgs e)
        {
            //se cambia il valore dell'offset, metto lo switch di abilitazione offset a off
            if (switchOffset.Checked)
                switchOffset.Checked = false;
        }

        private void switchEVMovOr_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchEVMovOr.Checked)
            {
                listaGuasti.Add("EV movimento orizzontale");
            }
            else
            {
                listaGuasti.Remove("EV movimento orizzontale");
            }

            simulaGuasto();
        }

        private void switchEVMovVert_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchEVMovVert.Checked)
            {
                listaGuasti.Add("EV movimento verticale");
            }
            else
            {
                listaGuasti.Remove("EV movimento verticale");
            }

            simulaGuasto();
        }

        private void switchEVTraslazione_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchEVTraslazione.Checked)
            {
                listaGuasti.Add("EV traslazione");
            }
            else
            {
                listaGuasti.Remove("EV traslazione");
            }

            simulaGuasto();
        }

        private void switchEVPinza_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchEVPinza.Checked)
            {
                listaGuasti.Add("EV pinza");
            }
            else
            {
                listaGuasti.Remove("EV pinza");
            }

            simulaGuasto();
        }

        private void switchScaricoPieno_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchScaricoPieno.Checked)
            {
                listaGuasti.Add("scarico pieno");
            }
            else
            {
                listaGuasti.Remove("scarico pieno");
            }

            simulaGuasto();
        }

        private void switchMancanzaPezzi_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchMancanzaPezzi.Checked)
            {
                listaGuasti.Add("mancanza pezzi");
            }
            else
            {
                listaGuasti.Remove("mancanza pezzi");
            }

            simulaGuasto();
        }

        private void switchTampografia_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchTampografia.Checked)
            {
                listaGuasti.Add("tampografia");
            }
            else
            {
                listaGuasti.Remove("tampografia");
            }

            simulaGuasto();
        }
        private void switchAriaImpianto_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (switchAriaImpianto.Checked)
            {
                listaGuasti.Add("aria impianto");
            }
            else
            {
                listaGuasti.Remove("aria impianto");
            }

            simulaGuasto();
        }
        private void tabGuasti_Enter(object sender, EventArgs e)
        {
            setVisualizzazioneAllarmi(allarmi);
        }
        #endregion

        #region Funzioni

        private void setVisualizzazioneAllarmi(int a)
        {
            //set della visualizzazione degli allarmi
            switch (a)
            {
                case 1:
                    txtAllarme.Text = "FUNGO DI EMERGENZA PREMUTO";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.Fungo_di_emergenza_premuto_ALLARME_1;
                    break;

                case 2:
                    txtAllarme.Text = "GUASTO FINECORSA INDIETRO";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.FCIndietro_ALLARME_2;
                    break;

                case 3:
                    txtAllarme.Text = "GUASTO FINECORSA AVANTI";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.FCAvanti_ALLARME_3;
                    break;

                case 4:
                    txtAllarme.Text = "GUASTO FINECORSA SINISTRA";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.FCSinistra_ALLARME_4;
                    break;

                case 5:
                    txtAllarme.Text = "GUASTO FINECORSA DESTRA";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.FCDestra_ALLARME_5;
                    break;

                case 6:
                    txtAllarme.Text = "GUASTO APERTURA PINZA";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.AperturaPinza_ALLARME_6;
                    break;

                case 7:
                    txtAllarme.Text = "GUASTO CHIUSURA PINZA";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.ChiusuraPinza_ALLARME_7;
                    break;

                case 8:
                    txtAllarme.Text = "GUASTO FINECORSA GIU";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.FCGiu_ALLARME_8;
                    break;

                case 9:
                    txtAllarme.Text = "GUASTO FINECORSA SU";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.FCSu_ALLARME_9;
                    break;

                case 10:
                    txtAllarme.Text = "PEZZI MANCANTI SU NASTRO DI CARICO";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.nastroVuoto_ALLARME_10;
                    break;

                case 11:
                    txtAllarme.Text = "NASTRO DI SCARICO PIENO";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.NastroPieno_ALLARME_11;
                    break;

                case 12:
                    txtAllarme.Text = "GUASTO SU TAMPOGRAFIA";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.TampografiaGuasta_ALLARME_12;
                    break;

                case 13:
                    txtAllarme.Text = "MANCANZA ARIA IMPIANTO";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.MancanzaAriaImpianto_ALLARME_13;
                    break;

                case 14:
                    txtAllarme.Text = "GUASTO EV MOVIMENTO ORIZZONTALE";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.EV_ALLARME_14_17_trasparente;
                    break;

                case 15:
                    txtAllarme.Text = "GUASTO EV MOVIMENTO VERTICALE";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.EV_ALLARME_14_17_trasparente;
                    break;

                case 16:
                    txtAllarme.Text = "GUASTO EV TRASLAZIONE";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.EV_ALLARME_14_17_trasparente;
                    break;

                case 17:
                    txtAllarme.Text = "GUASTO EV PINZA";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.EV_ALLARME_14_17_trasparente;
                    break;

                default:
                    txtAllarme.Text = "NESSUN ALLARME";
                    pictureAllarme.BackgroundImage = PLC_Manager.Properties.Resources.Nessuna_Emergenza;
                    break;
            }
        }
        private void setSwitchGuastiStatus(bool enable)
        {
            switchEVTraslazione.Enabled = enable;
            switchEVMovOr.Enabled = enable;
            switchEVMovVert.Enabled = enable;
            switchEVPinza.Enabled = enable;
            switchAriaImpianto.Enabled = enable;
            switchMancanzaPezzi.Enabled = enable;
            switchScaricoPieno.Enabled = enable;
            switchTampografia.Enabled = enable;
        }

        private int codiceGuastoDaNome(string guasto)
        {
            if (guasto == "mancanza pezzi")
                return 1;
            if (guasto == "scarico pieno")
                return 2;
            if (guasto == "tampografia")
                return 4;
            if (guasto == "aria impianto")
                return 8;
            if (guasto == "EV movimento orizzontale")
                return 16;
            if (guasto == "EV movimento verticale")
                return 32;
            if (guasto == "EV traslazione")
                return 64;
            if (guasto == "EV pinza")
                return 128;

            return 0;
        }

        private void simulaGuasto()
        {
            //se sono presenti guasti da simulare simula quello più recente
            if(listaGuasti.Count > 0)
            {
                string guasto = listaGuasti[listaGuasti.Count -1];  //ultimo guasto inserito

                CSerialDeserial.ReadFile(ref this.db1);
                S7.SetUSIntAt(this.db1Buffer, 217, Convert.ToByte(codiceGuastoDaNome(guasto)));

                this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                if (result == 0)
                    Alert($"Guasto simulato correttamente", AlertDLL.Alert.enmType.Success);
                else
                    Alert("Errore nella simulazione", AlertDLL.Alert.enmType.Error);
            }
            else
            {
                cancellaSimulazioneGuasto();
            }
        }

        private void cancellaSimulazioneGuasto()
        {
            //set variabile guasti del plc a 0
            CSerialDeserial.ReadFile(ref this.db1);
            S7.SetUSIntAt(this.db1Buffer, 217, 0);

            this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);
        }
        #endregion

        #endregion

        #region PaginaAnagrafica
        private void tabAnagrafica_Enter(object sender, EventArgs e)
        {
            int totCommesse = dgvAnagrafica.Rows.Count;
            int totCommesseConcluse = 0;
            int totProdotti = 0;

            for (int i = 0; i < totCommesse; i++)
            {
                string nProdotto = "" + dgvAnagrafica.Rows[i].Cells[2].Value;
                string stato = "" + dgvAnagrafica.Rows[i].Cells[4].Value;

                if(stato == "completata")
                {
                    totCommesseConcluse++;
                    totProdotti += Convert.ToInt32(nProdotto);
                }
            }

            txtTotCommesse.Text = "" + totCommesse;
            txtTotalePezzi.Text = "" + totProdotti;
            progressPercCommesseCompletate.Value = totCommesseConcluse * 100 / totCommesse;
        }
        #endregion

        private void btnRiduciIcona_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void switchOffset_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            try
            {
                if (switchOffset.Checked)
                {
                    CSerialDeserial.ReadFile(ref this.db1);
                    S7.SetDIntAt(this.db1Buffer, 212, Convert.ToByte(nudDelayTime.Value));

                    this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                    Alert($"Offset impostato a {nudDelayTime.Value}s", AlertDLL.Alert.enmType.Success);
                }
                else
                {
                    CSerialDeserial.ReadFile(ref this.db1);
                    S7.SetDIntAt(this.db1Buffer, 212, 0);

                    this.result = this.Client.DBWrite(1, 0, this.db1Buffer.Length, this.db1Buffer);

                    Alert("Offset rimosso", AlertDLL.Alert.enmType.Success);
                }
            }
            catch (Exception)
            {
                Alert("Errore non gestito", AlertDLL.Alert.enmType.Error);
            }
        }
    }
}
