using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000236 RID: 566
	public sealed class ScoreboardHotKeyCategory : GameKeyContext
	{
		// Token: 0x060020E4 RID: 8420 RVA: 0x000746C1 File Offset: 0x000728C1
		public ScoreboardHotKeyCategory()
			: base("ScoreboardHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000746E4 File Offset: 0x000728E4
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.F),
				new Key(InputKey.ControllerRUp)
			};
			base.RegisterHotKey(new HotKey("ToggleFastForward", "ScoreboardHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerROption)
			};
			base.RegisterHotKey(new HotKey("TogglePause", "ScoreboardHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("MenuShowContextMenu", "ScoreboardHotKeyCategory", InputKey.RightMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.Tab),
				new Key(InputKey.ControllerRRight)
			};
			base.RegisterHotKey(new HotKey("HoldShow", "ScoreboardHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000747C1 File Offset: 0x000729C1
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(35, "ShowMouse", "ScoreboardHotKeyCategory", InputKey.MiddleMouseButton, InputKey.ControllerLThumb, GameKeyMainCategories.ActionCategory), true);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000747EA File Offset: 0x000729EA
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C8D RID: 3213
		public const string CategoryId = "ScoreboardHotKeyCategory";

		// Token: 0x04000C8E RID: 3214
		public const int ShowMouse = 35;

		// Token: 0x04000C8F RID: 3215
		public const string HoldShow = "HoldShow";

		// Token: 0x04000C90 RID: 3216
		public const string ToggleFastForward = "ToggleFastForward";

		// Token: 0x04000C91 RID: 3217
		public const string TogglePause = "TogglePause";

		// Token: 0x04000C92 RID: 3218
		public const string MenuShowContextMenu = "MenuShowContextMenu";
	}
}
