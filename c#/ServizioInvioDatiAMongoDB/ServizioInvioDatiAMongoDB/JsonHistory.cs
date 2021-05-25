using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServizioInvioDatiAMongoDB
{
    class JsonHistory
    {
        public string articolo { get; set; }
        public string codice_commessa { get; set; }
        public int quantita_prevista { get; set; }
        public int quantita_prodotta { get; set; }
        public int quantita_scarto_difettoso { get; set; }
        public int quantita_scarto_pieno { get; set; }
        public string data_consegna { get; set; }
        public string data_esecuzione{ get; set; }
    }
}
