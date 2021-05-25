using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using PLCConnectionDLL;
using Sharp7;
using System.Configuration;

namespace ServizioInvioDatiAMongoDB
{
    public partial class ServizioInvioDati : ServiceBase
    {
        private Timer timer = new Timer();
        private S7Client Client;

        private byte[] db1Buffer = new byte[281];       //spazio occupato nel db1
        private byte[] db2Buffer = new byte[233];

        private int resultdb1;
        private int resultdb2;

        private CDataBlock db1 = new CDataBlock();
        private CDataBlock db2 = new CDataBlock();

        private string postHistoryURL = "http://54.85.250.76:3000/api/history";
        private string postStatusURL = "http://54.85.250.76:3000/api/status";

        private JsonHistory jh;
        private JsonStatus js;

        bool connessione = false;
        bool invioCommessaVuota = false; //invio di una commessa vuota per segnalare alla web app in node.js che la commessa precedente è fallita

        //lista per coda di messaggi
        List<JsonHistory> coda = new List<JsonHistory>();
        public ServizioInvioDati()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Client = new S7Client();
            db1.DBName = "DB_PC-PLC";
            db2.DBName = "DB_PLC-PC";

            timer.Interval = 1000;
            timer.Elapsed += OnTimerEvent;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }

        private void OnTimerEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //connessione al PLC se non è attiva
            if (!connessione)
            {
                ConnessionePLC();
            }
            //log di errore in caso di fallimento della connessione
            if(!connessione)
            {
                EventLog appEventLog = new EventLog("Application");
                appEventLog.Source = "Timer Servizio Dati MongoDB";
                appEventLog.WriteEntry("Connessione al PLC fallita", EventLogEntryType.Error);
            }
            else
            {
                LetturaDatiEInvio();
            }

            //controllo grandezza coda (se supera x elementi, viene eliminato il primo)
            if (coda.Count > 1000)
                coda.RemoveAt(0);
        }
        private void ConnessionePLC()
        {
            int Rack = Convert.ToInt32(ConfigurationManager.AppSettings["PLC_RACK"]);
            int Slot = Convert.ToInt32(ConfigurationManager.AppSettings["PLC_SLOT"]);
            string ip = ConfigurationManager.AppSettings["PLC_IP"];

            if (Client.ConnectTo(ip, Rack, Slot) == 0)	//0 se la connessione è riuscita
                connessione = true;
            else
                connessione = false;
        }

        private void LetturaDatiEInvio()
        {
            try
            {
                resultdb1 = Client.DBRead(1, 0, db1Buffer.Length, db1Buffer);
                resultdb2 = Client.DBRead(2, 0, db2Buffer.Length, db2Buffer);

                if (resultdb1 == 0 && resultdb2 == 0)
                {
                    jh = new JsonHistory();
                    js = new JsonStatus();

                    //LEGGO I DATI DAL PLC
                    jh.quantita_prevista = S7.GetDIntAt(db1Buffer, 104);
                    jh.data_consegna = S7.GetStringAt(db1Buffer, 218);
                    jh.data_esecuzione = S7.GetStringAt(db1Buffer, 250);

                    //manipolazione date in anno-mese-giorno
                    DateTime tmpC = Convert.ToDateTime(jh.data_consegna);
                    jh.data_consegna = tmpC.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime tmpE = Convert.ToDateTime(jh.data_esecuzione);
                    jh.data_esecuzione = tmpE.ToString("yyyy-MM-dd HH:mm:ss");

                    jh.codice_commessa = S7.GetStringAt(db2Buffer, 0);
                    jh.articolo = S7.GetStringAt(db2Buffer, 52);
                    jh.quantita_prodotta = S7.GetDIntAt(db2Buffer, 104);
                    jh.quantita_scarto_pieno = S7.GetDIntAt(db2Buffer, 108);
                    jh.quantita_scarto_difettoso = S7.GetDIntAt(db2Buffer, 112);

                    //Settaggio stato macchina e allarmi
                    int statusMacchina = S7.GetUSIntAt(db2Buffer, 116);
                    js.stato = GetStatusMacchina(statusMacchina);
                    js.velocita = S7.GetDIntAt(db2Buffer, 118);
                    int codiceAllarme = S7.GetUSIntAt(db2Buffer, 232);
                    js.allarme = GetAllarmi(codiceAllarme);

                    //invio dati solo se è presente una commessa con codice non vuoto, oppure se è la prima con codice vuoto dopo almeno una normale
                    if ( (jh.codice_commessa == "" && !invioCommessaVuota) || jh.codice_commessa != "")
                    {
                        //CONVERTO I DATI LETTI IN STRINGHE JSON
                        string jsonstato = JsonConvert.SerializeObject(js);

                        //INVIO I JSON ALLE API POST DEL MONGODB
                        SendHistoryJsonToAPI(jh);
                        SendStatusJsonToAPI(jsonstato);

                        EventLog appEventLog2 = new EventLog("Application");
                        appEventLog2.Source = "Timer Servizio Dati MongoDB";
                        appEventLog2.WriteEntry($"{jh.codice_commessa}", EventLogEntryType.Error);
                        //set della booleana per sapere se è già stata inviata una commessa vuota
                        if (jh.codice_commessa == "")
                            invioCommessaVuota = true;
                        else
                            invioCommessaVuota = false;
                    }
                }
                else
                {
                    EventLog appEventLog2 = new EventLog("Application");
                    appEventLog2.Source = "Timer Servizio Dati MongoDB";
                    appEventLog2.WriteEntry($"no connection", EventLogEntryType.Error);
                }
            }
            catch (Exception)
            {
                Client.Disconnect();
                connessione = false;
            }
        }
        private void SendHistoryJsonToAPI(JsonHistory jsonhistory)
        {
            //aggiungere nuova lettura o aggiornare la coda
            if (coda.Count == 0)
                coda.Add(jsonhistory);
            else
            {
                int pos = posObject(jsonhistory);
                if(pos != -1)
                {
                    coda[pos] = jsonhistory;
                }
                else
                {
                    coda.Add(jsonhistory);
                }
            }

            string jh = JsonConvert.SerializeObject(coda[0]);
            //prova l'invio dei dati dello storico, se va a buon fine toglie il primo elemento dalla coda
            if (trySendAPIHistory(jh))
            {
                coda.RemoveAt(0);
            }
        }

        private int posObject(JsonHistory jsonHistory)
        {
            for (int i = 0; i < coda.Count; i++)
            {
                if (coda[i].codice_commessa == jsonHistory.codice_commessa)
                    return i;
            }
            return -1;
        }

        //invio dati storico
        private bool trySendAPIHistory(string jh)
        {
            HttpWebResponse responseHistory = null;
            try
            {
                // JSON HISTORY
                HttpWebRequest requestHistory = (HttpWebRequest)WebRequest.Create(postHistoryURL);
                requestHistory.Method = "POST";
                requestHistory.ContentType = "application/json";
                requestHistory.Timeout = 500;

                StreamWriter swHistory = new StreamWriter(requestHistory.GetRequestStream());
                swHistory.Write(jh);

                swHistory.Flush();
                swHistory.Close();

                responseHistory = (HttpWebResponse)requestHistory.GetResponse();
                responseHistory.Close();
            }
            catch (Exception)
            {
                return false;
            }
            if (responseHistory == null)
                return false;
            return true;
        }
        //invio dati stato
        private void SendStatusJsonToAPI(string js)
        {
            HttpWebRequest requestStatus = (HttpWebRequest)WebRequest.Create(postStatusURL);
            requestStatus.Timeout = 500;
            requestStatus.Method = "POST";
            requestStatus.ContentType = "application/json";
            StreamWriter swStatus = new StreamWriter(requestStatus.GetRequestStream());
            swStatus.Write(js);

            swStatus.Flush();
            swStatus.Close();

            HttpWebResponse responseStatus = (HttpWebResponse)requestStatus.GetResponse();
            responseStatus.Close();
        }
        private string GetStatusMacchina(int stato)
        {
            string result;
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

        private string GetAllarmi(int allarme)
        {
            string result;

            switch (allarme)
            {
                case 1:
                    result = "FUNGO DI EMERGENZA PREMUTO";
                    break;

                case 2:
                    result = "GUASTO FINECORSA INDIETRO";
                    break;

                case 3:
                    result = "GUASTO FINECORSA AVANTI";
                    break;

                case 4:
                    result = "GUASTO FINECORSA SINISTRA";
                    break;

                case 5:
                    result = "GUASTO FINECORSA DESTRA";
                    break;
                case 6:
                    result = "GUASTO APERTURA PINZA";
                    break;

                case 7:
                    result = "GUASTO CHIUSURA PINZA";
                    break;

                case 8:
                    result = "GUASTO FINECORSA GIU";
                    break;

                case 9:
                    result = "GUASTO FINECORSA SU";
                    break;

                case 10:
                    result = "PEZZI MANCANTI SU NASTRO DI CARICO";
                    break;

                case 11:
                    result = "NASTRO DI SCARICO PIENO";
                    break;

                case 12:
                    result = "GUASTO SU TAMPOGRAFIA";
                    break;

                case 13:
                    result = "MANCANZA ARIA IMPIANTO";
                    break;

                case 14:
                    result = "GUASTO EV MOVIMENTO ORIZZONTALE";
                    break;

                case 15:
                    result = "GUASTO EV MOVIMENTO VERTICALE";
                    break;

                case 16:
                    result = "GUASTO EV TRASLAZIONE";
                    break;

                case 17:
                    result = "GUASTO EV PINZA";
                    break;

                default:
                    result = "NESSUN ALLARME";
                    break;
            }

            return result;
        }


    }
}
