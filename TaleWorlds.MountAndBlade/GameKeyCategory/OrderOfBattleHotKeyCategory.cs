using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GameKeyCategory
{
	// Token: 0x020003F1 RID: 1009
	public class OrderOfBattleHotKeyCategory : GameKeyContext
	{
		// Token: 0x0600373E RID: 14142 RVA: 0x000E49B9 File Offset: 0x000E2BB9
		public OrderOfBattleHotKeyCategory()
			: base("OrderOfBattleHotKeyCategory", 0, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000E49D0 File Offset: 0x000E2BD0
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerRRight)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter),
				new Key(InputKey.ControllerRLeft)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.ControllerRUp)
			};
			base.RegisterHotKey(new HotKey("Exit", "OrderOfBattleHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Confirm", "OrderOfBattleHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("AutoDeploy", "OrderOfBattleHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x040017BB RID: 6075
		public const string CategoryId = "OrderOfBattleHotKeyCategory";

		// Token: 0x040017BC RID: 6076
		public const string Confirm = "Confirm";

		// Token: 0x040017BD RID: 6077
		public const string Exit = "Exit";

		// Token: 0x040017BE RID: 6078
		public const string AutoDeploy = "AutoDeploy";
	}
}
