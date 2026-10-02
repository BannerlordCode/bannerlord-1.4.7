using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000226 RID: 550
	public sealed class ConversationHotKeyCategory : GameKeyContext
	{
		// Token: 0x060020AB RID: 8363 RVA: 0x00072F8B File Offset: 0x0007118B
		public ConversationHotKeyCategory()
			: base("ConversationHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00072FB0 File Offset: 0x000711B0
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Space),
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter)
			};
			base.RegisterHotKey(new HotKey("ContinueKey", "ConversationHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			base.RegisterHotKey(new HotKey("ContinueClick", "ConversationHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00073045 File Offset: 0x00071245
		private void RegisterGameKeys()
		{
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00073047 File Offset: 0x00071247
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B6F RID: 2927
		public const string CategoryId = "ConversationHotKeyCategory";

		// Token: 0x04000B70 RID: 2928
		public const string ContinueKey = "ContinueKey";

		// Token: 0x04000B71 RID: 2929
		public const string ContinueClick = "ContinueClick";
	}
}
