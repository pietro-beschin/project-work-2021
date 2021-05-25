using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnectionDLL
{
	public interface IDataBlock
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16
		// (set) Token: 0x06000011 RID: 17
		string DBName { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18
		// (set) Token: 0x06000013 RID: 19
		short Word01 { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20
		// (set) Token: 0x06000015 RID: 21
		short Word02 { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22
		// (set) Token: 0x06000017 RID: 23
		short Word03 { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24
		// (set) Token: 0x06000019 RID: 25
		short Word04 { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26
		// (set) Token: 0x0600001B RID: 27
		short Word05 { get; set; }
	}
}
