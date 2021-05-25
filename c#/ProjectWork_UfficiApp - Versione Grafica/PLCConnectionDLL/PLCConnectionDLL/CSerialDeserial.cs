using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PLCConnectionDLL
{
	public static class CSerialDeserial
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000020C0 File Offset: 0x000002C0
		public static void WriteFile(CDataBlock db)
		{
			XmlSerializer formatterWR = new XmlSerializer(typeof(CDataBlock));
			Stream myStreamWR = new FileStream(db.DBName + ".xml", FileMode.Create, FileAccess.Write, FileShare.Read);
			formatterWR.Serialize(myStreamWR, db);
			myStreamWR.Close();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002108 File Offset: 0x00000308
		public static void ReadFile(ref CDataBlock db)
		{
			XmlSerializer formatterRD = new XmlSerializer(typeof(CDataBlock));
			try
			{
				Stream myStreamRD = new FileStream(db.DBName + ".xml", FileMode.Open, FileAccess.Read, FileShare.Write);
				db = (CDataBlock)formatterRD.Deserialize(myStreamRD);
				myStreamRD.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
