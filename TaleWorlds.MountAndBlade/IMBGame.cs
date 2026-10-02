using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B8 RID: 440
	[ScriptingInterfaceBase]
	internal interface IMBGame
	{
		// Token: 0x060018CF RID: 6351
		[EngineMethod("start_new", false, null, false)]
		void StartNew();

		// Token: 0x060018D0 RID: 6352
		[EngineMethod("load_module_data", false, null, false)]
		void LoadModuleData(bool isLoadGame);
	}
}
