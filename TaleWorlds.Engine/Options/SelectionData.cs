using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A7 RID: 167
	public struct SelectionData
	{
		// Token: 0x06000F48 RID: 3912 RVA: 0x00011E5E File Offset: 0x0001005E
		public SelectionData(bool isLocalizationId, string data)
		{
			this.IsLocalizationId = isLocalizationId;
			this.Data = data;
		}

		// Token: 0x0400021A RID: 538
		public bool IsLocalizationId;

		// Token: 0x0400021B RID: 539
		public string Data;
	}
}
