using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003C RID: 60
	[ApplicationInterfaceBase]
	internal interface ITime
	{
		// Token: 0x0600063C RID: 1596
		[EngineMethod("get_application_time", false, null, false)]
		float GetApplicationTime();
	}
}
