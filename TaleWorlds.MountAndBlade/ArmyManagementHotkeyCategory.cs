using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000222 RID: 546
	public sealed class ArmyManagementHotkeyCategory : GameKeyContext
	{
		// Token: 0x0600209D RID: 8349 RVA: 0x000726E7 File Offset: 0x000708E7
		public ArmyManagementHotkeyCategory()
			: base("ArmyManagementHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000726FD File Offset: 0x000708FD
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("RemoveParty", "ArmyManagementHotkeyCategory", InputKey.ControllerRBumper, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x04000B35 RID: 2869
		public const string CategoryId = "ArmyManagementHotkeyCategory";

		// Token: 0x04000B36 RID: 2870
		public const string RemoveParty = "RemoveParty";
	}
}
