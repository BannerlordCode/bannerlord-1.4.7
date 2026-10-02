using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B6 RID: 438
	[ScriptingInterfaceBase]
	internal interface IMBMessageManager
	{
		// Token: 0x060018C5 RID: 6341
		[EngineMethod("display_message", false, null, false)]
		void DisplayMessage(string message);

		// Token: 0x060018C6 RID: 6342
		[EngineMethod("display_message_with_color", false, null, false)]
		void DisplayMessageWithColor(string message, uint color);

		// Token: 0x060018C7 RID: 6343
		[EngineMethod("set_message_manager", false, null, false)]
		void SetMessageManager(MessageManagerBase messageManager);
	}
}
