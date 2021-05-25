using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommesseDDL;

namespace DBManagerDLL
{
    public static class DBManager
    {
		public static string connectionString = "SERVER=(local)\\SQLEXPRESS; DATABASE=dbProjectWork2021; Trusted_Connection=TRUE";

		public static Commessa scriviCommessaInDB(int pezziDaProdurre, string nominativoCliente, string prodotto, int IDProdotto, int IDCliente, DateTime dataConsegna, DateTime dataEsecuzione)
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("INSERT INTO tblCommesse (codiceCommessa, pezziDaProdurre, stato, IDProdotto, IDCliente, dataConsegna) VALUES (@codiceCommessa, @pezziDaProdurre, @stato, @IDProdotto, @IDCliente, @dataConsegna)", conn);

			Commessa commessa = null;

			try
			{
				string nuovoCodiceCommessa = ottieniNuovoCodiceCommessa();

				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@codiceCommessa", nuovoCodiceCommessa);
				cmd.Parameters.AddWithValue("@pezziDaProdurre", pezziDaProdurre);
				cmd.Parameters.AddWithValue("@stato", "disattiva");
				cmd.Parameters.AddWithValue("@IDProdotto", IDProdotto);
				cmd.Parameters.AddWithValue("@IDCliente", IDCliente);
				cmd.Parameters.AddWithValue("@dataConsegna", dataConsegna);

				cmd.ExecuteNonQuery();

				commessa = new Commessa(nuovoCodiceCommessa, prodotto, pezziDaProdurre, nominativoCliente, dataConsegna, dataEsecuzione);

				aggiornaUltimoCodiceCommessa();

			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();   
				conn.Dispose(); 
			}

			return commessa;
		}

		public static Dictionary<int, string> leggiClienti()
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT ID, nominativo FROM tblClienti WHERE attivo = 1", conn);
			SqlDataReader dr;

			Dictionary<int, string> clienti = new Dictionary<int, string>();

			try
			{
				conn.Open();

				dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					int id = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("ID")));
					string nominativo = Convert.ToString(dr.GetValue(dr.GetOrdinal("nominativo")));

					clienti.Add(id, nominativo);
				}
			}
			catch (SqlException sqle)
			{
				Console.WriteLine(sqle.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return clienti;
		}
		public static bool aggiungiCliente(string nominativo)
		{
			bool aggiuntoCorrettamente = false;

			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("INSERT INTO tblClienti (nominativo) VALUES (@nominativo)", conn);

			try
			{
				string nuovoCodiceCommessa = ottieniNuovoCodiceCommessa();

				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@nominativo", nominativo);

				cmd.ExecuteNonQuery();
				aggiuntoCorrettamente = true;
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return aggiuntoCorrettamente;
		}
		public static bool eliminaCliente(int id)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("UPDATE tblClienti SET attivo = 0 WHERE ID = @id", conn);

			bool eliminato = false;
			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();

				eliminato = true;
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return eliminato;
		}
		public static bool eliminaProdotto(int id)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("UPDATE tblProdotti SET attivo = 0 WHERE ID = @id", conn);

			bool eliminato = false;
			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();

				eliminato = true;
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return eliminato;
		}

		public static bool aggiungiProdotto(string descrizione)
		{
			bool aggiuntoCorrettamente = false;

			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("INSERT INTO tblProdotti (descrizione) VALUES (@descrizione)", conn);

			try
			{
				string nuovoCodiceCommessa = ottieniNuovoCodiceCommessa();

				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@descrizione", descrizione);

				cmd.ExecuteNonQuery();
				aggiuntoCorrettamente = true;
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return aggiuntoCorrettamente;
		}

		public static Dictionary<int, string> leggiProdotti()
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT ID, descrizione FROM tblProdotti WHERE attivo = 1", conn);
			SqlDataReader dr;

			Dictionary<int, string> prodotti = new Dictionary<int, string>();

			try
			{
				conn.Open();

				dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					int id = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("ID")));
					string descrizione = Convert.ToString(dr.GetValue(dr.GetOrdinal("descrizione")));

					prodotti.Add(id, descrizione);
				}
			}
			catch (SqlException sqle)
			{
				Console.WriteLine(sqle.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return prodotti;
		}


		//legge le commesse dal database (solo quelle non completate)
		public static Commesse leggiCommesseDalDB()
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT codiceCommessa, pezziDaProdurre, stato, idProdotto, idCliente, dataConsegna, dataEsecuzione FROM tblCommesse WHERE stato != 'completata' AND stato != 'fallita'", conn);
			SqlDataReader dr;

			Commesse commesse = new Commesse();

			try
			{
				conn.Open();

				dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					string codiceCommessa = Convert.ToString(dr.GetValue(dr.GetOrdinal("codiceCommessa")));
					int pezziDaProdurre = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("pezziDaProdurre")));
					string strStato = Convert.ToString(dr.GetValue(dr.GetOrdinal("stato")));
					statoCommesa stato = getStatoFromString(strStato);
					int idProdotto  = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("idProdotto")));
					int idCliente = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("idCliente")));
					DateTime dataConsegna = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("dataConsegna")));
					DateTime dataEsecuzione = new DateTime(1, 1, 1);

					if (!Convert.IsDBNull(dr.GetValue(dr.GetOrdinal("dataEsecuzione"))))
						dataEsecuzione = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("dataEsecuzione")));

					commesse.aggiungiCommessa(new Commessa(codiceCommessa, descrizioneProdottoDaID(idProdotto), pezziDaProdurre, nominativoClienteDaID(idCliente), stato, dataConsegna, dataEsecuzione));
				}
			}
			catch (SqlException sqle)
			{
				Console.WriteLine(sqle.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return commesse;
		}

		private static string descrizioneProdottoDaID(int ID)
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT descrizione FROM tblProdotti WHERE ID = @ID", conn);
			SqlDataReader dr;

			string descrizione = "";

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@ID", ID);

				dr = cmd.ExecuteReader();

				if (dr.Read())
				{
					descrizione = Convert.ToString(dr.GetValue(dr.GetOrdinal("descrizione")));
				}
			}
			catch (SqlException sqle)
			{
				Console.WriteLine(sqle.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return descrizione;
		}

		private static string nominativoClienteDaID(int ID)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT nominativo FROM tblClienti WHERE ID = @ID", conn);
			SqlDataReader dr;

			string nominativo = "";

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@ID", ID);

				dr = cmd.ExecuteReader();

				if (dr.Read())
				{
					nominativo = Convert.ToString(dr.GetValue(dr.GetOrdinal("nominativo")));
				}
			}
			catch (SqlException sqle)
			{
				Console.WriteLine(sqle.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return nominativo;
		}

		public static void updateStatoCommessa(string codiceCommessa, string stato)
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("UPDATE tblCommesse SET stato = @stato WHERE codiceCommessa = @codiceCommessa", conn);

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@codiceCommessa", codiceCommessa);
				cmd.Parameters.AddWithValue("@stato", stato);

				cmd.ExecuteNonQuery();
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}
		public static void updateDataEsecuzione(string codiceCommessa, DateTime dataEsecuzione)
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("UPDATE tblCommesse SET dataEsecuzione = @dataEsecuzione WHERE codiceCommessa = @codiceCommessa", conn);

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@codiceCommessa", codiceCommessa);
				cmd.Parameters.AddWithValue("@dataEsecuzione", dataEsecuzione);

				cmd.ExecuteNonQuery();
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		//da sistemare
		public static void modificaCommessa(string codiceCommessa, int idProdotto, int pezziDaProdurre, int idCliente, DateTime dataConsegna)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("UPDATE tblCommesse SET IDProdotto = @idProdotto, pezziDaProdurre = @pezziDaProdurre, dataConsegna = @dataConsegna, IDCliente = @idCliente WHERE codiceCommessa = @codiceCommessa", conn);

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@codiceCommessa", codiceCommessa);
				cmd.Parameters.AddWithValue("@pezziDaProdurre", pezziDaProdurre);
				cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
				cmd.Parameters.AddWithValue("@idCliente", idCliente);
				cmd.Parameters.AddWithValue("@dataConsegna", dataConsegna);

				cmd.ExecuteNonQuery();
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		public static void rimuoviCommessaDalDB(string codiceCommessa)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("DELETE FROM tblCommesse WHERE codiceCommessa = @codiceCommessa", conn);

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@codiceCommessa", codiceCommessa);

				cmd.ExecuteNonQuery();
			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}


		private static string ottieniNuovoCodiceCommessa()
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT ultimoIdentificativoCommessa FROM tblUltimoIdentificativoCommessa", conn);
			SqlDataReader dr;

			string nuovoCodiceCommessa = "";

			try
			{
				conn.Open();

				dr = cmd.ExecuteReader();

				if(dr.Read())
				{
					nuovoCodiceCommessa = ""+ (Convert.ToInt32(dr.GetValue(dr.GetOrdinal("ultimoIdentificativoCommessa")))+1);
				}
                else
                {
					int nuovoID = 1;
					nuovoCodiceCommessa = "" + nuovoID;
					aggiungiUltimoCodiceCommessa(nuovoID);
                }

				nuovoCodiceCommessa = DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.ToString("ddd").Substring(0,2).ToUpper()+"_"+nuovoCodiceCommessa;
			}
			catch (SqlException sqle)
			{
				Console.WriteLine(sqle.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}

			return nuovoCodiceCommessa;
		}

		private static void aggiornaUltimoCodiceCommessa()
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("update tblUltimoIdentificativoCommessa set ultimoIdentificativoCommessa = ultimoIdentificativoCommessa +1", conn);

			try
			{
				conn.Open();

				cmd.ExecuteNonQuery();

			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();   
				conn.Dispose();
			}
		}

		private static void aggiungiUltimoCodiceCommessa(int nuovoID)
        {
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("insert into tblUltimoIdentificativoCommessa (ultimoIdentificativoCommessa) VALUES (@nuovoID)", conn);

			try
			{
				conn.Open();

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@nuovoID", nuovoID);

				cmd.ExecuteNonQuery();

			}
			catch (SqlException SQLEx)
			{
				Console.WriteLine(SQLEx.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		private static statoCommesa getStatoFromString(string strStato)
        {
			if (strStato == "inEsecuzione")
				return statoCommesa.inEsecuzione;
			if (strStato == "inCoda")
				return statoCommesa.inCoda;

			return statoCommesa.disattiva;
		}
	}
}
