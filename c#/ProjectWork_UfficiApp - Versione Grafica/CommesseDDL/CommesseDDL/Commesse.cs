using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommesseDDL
{
    public class Commesse
    {
        private List<Commessa> _commesse;
        private int _numeroCommesse;

        public Commesse()
        {
            _commesse = new List<Commessa>();
            _numeroCommesse = 0;
        }

        public int NumeroCommesse { get => _numeroCommesse; }
        public List<Commessa> ListaCommesse { get => _commesse;}

        public void aggiungiCommessa(Commessa commessa)
        {
            _commesse.Add(commessa);
            _numeroCommesse++;
        }

        public void aggiungiCommessa(string codiceCommessa, string prodotto, int pezziDaProdurre, string nominativoCliente, DateTime dataConsegna, DateTime dataEsecuzione)
        {
            aggiungiCommessa(new Commessa(codiceCommessa, prodotto, pezziDaProdurre, nominativoCliente, dataConsegna, dataEsecuzione));
        }

        public bool rimuoviCommessa(string codiceCommessa)
        {
            Commessa commessa = null;
            commessa = _commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; });

            if (commessa != null)
            {
                _commesse.Remove(commessa);
                _numeroCommesse--;
                return true;
            }
            return false;
        }

        public bool modificaCommessa(string codiceCommessa, string prodotto, int pezziDaProdurre, string nominativoCliente, DateTime dataConsegna)
        {
            foreach (var commessa in _commesse)
            {
                if (commessa.CodiceCommessa == codiceCommessa)
                {
                    commessa.Prodotto = prodotto;
                    commessa.PezziDaProdurre = pezziDaProdurre;
                    commessa.NominativoCliente = nominativoCliente;
                    commessa.DataConsegna = dataConsegna;

                    return true;
                }
            }

            return false;
        }

        public Commessa getCommessa(int pos)
        {
            if (pos >= 0 && pos < _numeroCommesse)
                return _commesse[pos];

            return null;
        }

        public void eseguiCommessa(string codiceCommessa, DateTime dataEsecuzione)
        {
            (_commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; })).esegui();
            (_commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; })).DataEsecuzione = dataEsecuzione;
        }

        public void mettiInCodaCommessa(string codiceCommessa)
        {
            (_commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; })).mettiInCoda();
        }

        public void disattivaCommmessa(string codiceCommessa)
        {
            (_commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; })).disattiva();
        }

        public void completaCommessa(string codiceCommessa)
        {
            (_commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; })).completa();
            _commesse.RemoveAt(0);
        }

        public void fallisciCommessa(string codiceCommessa)
        {
            (_commesse.Find((item) => { return item.CodiceCommessa == codiceCommessa; })).fallita();
        }

        public bool MandaInEsecuzioneCommessaSuccessiva()
        {
            if (_commesse.Count() > 0 && _commesse[0].Stato == statoCommesa.inCoda)
            {
                _commesse[0].Stato = statoCommesa.inEsecuzione;
                return true;
            }
            return false;
        }

        public bool prossimaCommessaInCoda(out string codiceCommessa, out string prodotto, out int pezziDaProdurre, out DateTime dataConsegna)
        {
            if (_commesse.Count() > 0 && _commesse[0].Stato == statoCommesa.inCoda)
            {
                codiceCommessa = _commesse[0].CodiceCommessa;
                prodotto = _commesse[0].Prodotto;
                pezziDaProdurre = _commesse[0].PezziDaProdurre;
                dataConsegna = _commesse[0].DataConsegna;

                return true;
            }

            codiceCommessa = "";
            prodotto = "";
            pezziDaProdurre = -1;
            dataConsegna = new DateTime();

            return false;
        }
        public void ordina()
        {
            List<Commessa> tmp = new List<Commessa>();

            //aggiunta di quella in esec.
            foreach (var commessa in _commesse)
            {
                if (commessa.Stato == statoCommesa.inEsecuzione)
                {
                    tmp.Add(commessa);
                    break;
                }
            }

            foreach (var commessa in _commesse)
            {
                if (commessa.Stato == statoCommesa.inCoda)
                    tmp.Add(commessa);
            }

            foreach (var commessa in _commesse)
            {
                if (commessa.Stato == statoCommesa.disattiva)
                    tmp.Add(commessa);
            }

            _commesse = tmp;
        }
    }
}
