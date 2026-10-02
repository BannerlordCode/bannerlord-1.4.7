using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022F RID: 559
	public sealed class InventoryHotKeyCategory : GameKeyContext
	{
		// Token: 0x060020CC RID: 8396 RVA: 0x00073AD9 File Offset: 0x00071CD9
		public InventoryHotKeyCategory()
			: base("InventoryHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00073AFB File Offset: 0x00071CFB
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("SwitchAlternative", "InventoryHotKeyCategory", InputKey.LeftAlt, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00073B17 File Offset: 0x00071D17
		private void RegisterGameKeys()
		{
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00073B19 File Offset: 0x00071D19
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C2E RID: 3118
		public const string CategoryId = "InventoryHotKeyCategory";

		// Token: 0x04000C2F RID: 3119
		public const string SwitchAlternative = "SwitchAlternative";
	}
}
