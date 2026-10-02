using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B1 RID: 433
	[ScriptingInterfaceBase]
	internal interface IMBScreen
	{
		// Token: 0x060018AC RID: 6316
		[EngineMethod("on_exit_button_click", false, null, false)]
		void OnExitButtonClick();

		// Token: 0x060018AD RID: 6317
		[EngineMethod("on_edit_mode_enter_press", false, null, false)]
		void OnEditModeEnterPress();

		// Token: 0x060018AE RID: 6318
		[EngineMethod("on_edit_mode_enter_release", false, null, false)]
		void OnEditModeEnterRelease();
	}
}
