using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000231 RID: 561
	public sealed class MapNotificationHotKeyCategory : GameKeyContext
	{
		// Token: 0x060020D4 RID: 8404 RVA: 0x00073F57 File Offset: 0x00072157
		public MapNotificationHotKeyCategory()
			: base("MapNotificationHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00073F6D File Offset: 0x0007216D
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("RemoveNotification", "MapNotificationHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x04000C4D RID: 3149
		public const string CategoryId = "MapNotificationHotKeyCategory";

		// Token: 0x04000C4E RID: 3150
		public const string RemoveNotification = "RemoveNotification";
	}
}
