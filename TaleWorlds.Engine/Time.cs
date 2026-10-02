using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000095 RID: 149
	public static class Time
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x0000EBD0 File Offset: 0x0000CDD0
		public static float ApplicationTime
		{
			get
			{
				return EngineApplicationInterface.ITime.GetApplicationTime();
			}
		}
	}
}
