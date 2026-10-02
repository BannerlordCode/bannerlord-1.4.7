using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BE RID: 446
	[ScriptingInterfaceBase]
	internal interface IMBBannerlordConfig
	{
		// Token: 0x0600190A RID: 6410
		[EngineMethod("validate_options", false, null, false)]
		void ValidateOptions();
	}
}
