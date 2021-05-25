using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnectionDLL
{
	// Token: 0x02000002 RID: 2
	[Serializable]
	public class CDataBlock : IDataBlock
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public string DBName
		{
			get
			{
				return this.dbName;
			}
			set
			{
				this.dbName = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public short Word01
		{
			get
			{
				return this.word01;
			}
			set
			{
				this.word01 = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
		public short Word02
		{
			get
			{
				return this.word02;
			}
			set
			{
				this.word02 = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002083 File Offset: 0x00000283
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000208B File Offset: 0x0000028B
		public short Word03
		{
			get
			{
				return this.word03;
			}
			set
			{
				this.word03 = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002094 File Offset: 0x00000294
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000209C File Offset: 0x0000029C
		public short Word04
		{
			get
			{
				return this.word04;
			}
			set
			{
				this.word04 = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020A5 File Offset: 0x000002A5
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020AD File Offset: 0x000002AD
		public short Word05
		{
			get
			{
				return this.word05;
			}
			set
			{
				this.word05 = value;
			}
		}

		// Token: 0x04000001 RID: 1
		private string dbName;

		// Token: 0x04000002 RID: 2
		private short word01;

		// Token: 0x04000003 RID: 3
		private short word02;

		// Token: 0x04000004 RID: 4
		private short word03;

		// Token: 0x04000005 RID: 5
		private short word04;

		// Token: 0x04000006 RID: 6
		private short word05;
	}
}
