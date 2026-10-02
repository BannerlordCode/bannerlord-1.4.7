using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000223 RID: 547
	public sealed class BoardGameHotkeyCategory : GameKeyContext
	{
		// Token: 0x0600209F RID: 8351 RVA: 0x0007271C File Offset: 0x0007091C
		public BoardGameHotkeyCategory()
			: base("BoardGameHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00072740 File Offset: 0x00070940
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.Space),
				new Key(InputKey.ControllerRBumper)
			};
			base.RegisterHotKey(new HotKey("BoardGamePawnSelect", "BoardGameHotkeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("BoardGamePawnDeselect", "BoardGameHotkeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("BoardGameDragPreview", "BoardGameHotkeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("BoardGameRollDice", "BoardGameHotkeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00072846 File Offset: 0x00070A46
		private void RegisterGameKeys()
		{
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00072848 File Offset: 0x00070A48
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B37 RID: 2871
		public const string CategoryId = "BoardGameHotkeyCategory";

		// Token: 0x04000B38 RID: 2872
		public const string BoardGamePawnSelect = "BoardGamePawnSelect";

		// Token: 0x04000B39 RID: 2873
		public const string BoardGamePawnDeselect = "BoardGamePawnDeselect";

		// Token: 0x04000B3A RID: 2874
		public const string BoardGameDragPreview = "BoardGameDragPreview";

		// Token: 0x04000B3B RID: 2875
		public const string BoardGameRollDice = "BoardGameRollDice";
	}
}
