using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200008B RID: 139
	public struct SaveResultWithMessage
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0001239A File Offset: 0x0001059A
		public static SaveResultWithMessage Default
		{
			get
			{
				return new SaveResultWithMessage(SaveResult.Success, string.Empty);
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000123A7 File Offset: 0x000105A7
		public SaveResultWithMessage(SaveResult result, string message)
		{
			this.SaveResult = result;
			this.Message = message;
		}

		// Token: 0x0400018D RID: 397
		public readonly SaveResult SaveResult;

		// Token: 0x0400018E RID: 398
		public readonly string Message;
	}
}
