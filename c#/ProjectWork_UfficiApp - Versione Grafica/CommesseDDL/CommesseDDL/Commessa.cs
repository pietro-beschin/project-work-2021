using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommesseDDL
{
    public enum statoCommesa
    {
        inEsecuzione,
        inCoda,
        disattiva,
        fallita,
        completata
    }
    public class Commessa
    {
        private string _codiceCommessa;     //numeroIncrementaleSalvatoInDB + idCliente + data
        private string _nominativoCliente;
        private string _prodotto;
        private int _pezziDaProdurre;
        private statoCommesa _stato;
        private DateTime _dataEsecuzione, _dataConsegna;

        public string CodiceCommessa { get => _codiceCommessa; set => _codiceCommessa = value; }
        public string Prodotto { get => _prodotto; set => _prodotto = value; }
        public int PezziDaProdurre { get => _pezziDaProdurre; set => _pezziDaProdurre = value; }
        public statoCommesa Stato { get => _stato; set => _stato = value; }
        public string NominativoCliente { get => _nominativoCliente; set => _nominativoCliente = value; }
        public DateTime DataEsecuzione { get => _dataEsecuzione; set => _dataEsecuzione = value; }
        public DateTime DataConsegna { get => _dataConsegna; set => _dataConsegna = value; }

        public Commessa(string codiceCommessa, string prodotto, int pezziDaProdurre, string nominativoCliente, statoCommesa stato, DateTime dataConsegna, DateTime dataEsecuzione)
        {
            _codiceCommessa = codiceCommessa;
            _prodotto = prodotto;
            _pezziDaProdurre = pezziDaProdurre;
            _nominativoCliente = nominativoCliente;
            _dataConsegna = dataConsegna;
            _stato = stato;
            _dataEsecuzione = dataEsecuzione;
        }

        public Commessa(string codiceCommessa, string prodotto, int pezziDaProdurre, string nominativoCliente, DateTime dataConsegna, DateTime dataEsecuzione) : this(codiceCommessa, prodotto, pezziDaProdurre, nominativoCliente, statoCommesa.disattiva, dataConsegna, dataEsecuzione)
        {
        }

        public void esegui()
        {
            _stato = statoCommesa.inEsecuzione;
        }

        public void mettiInCoda()
        {
            _stato = statoCommesa.inCoda;
        }

        public void disattiva()
        {
            _stato = statoCommesa.disattiva;
        }

        public void completa()
        {
            _stato = statoCommesa.completata;
        }

        public void fallita()
        {
            _stato = statoCommesa.fallita;
        }
    }
}
