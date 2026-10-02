using System;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003B RID: 59
	public class LoadError
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000BEC9 File Offset: 0x0000A0C9
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000BED1 File Offset: 0x0000A0D1
		public string Message { get; private set; }

		// Token: 0x06000256 RID: 598 RVA: 0x0000BEDA File Offset: 0x0000A0DA
		internal LoadError(string message)
		{
			this.Message = message;
		}
	}
}
