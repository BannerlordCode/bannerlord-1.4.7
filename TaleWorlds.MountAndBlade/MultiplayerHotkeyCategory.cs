using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000233 RID: 563
	public sealed class MultiplayerHotkeyCategory : GameKeyContext
	{
		// Token: 0x060020DA RID: 8410 RVA: 0x000742CD File Offset: 0x000724CD
		public MultiplayerHotkeyCategory()
			: base("MultiplayerHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000742F0 File Offset: 0x000724F0
		private void RegisterHotKeys()
		{
			for (int i = 1; i <= 9; i++)
			{
				base.RegisterHotKey(new HotKey("StoreCameraPosition" + i.ToString(), "MultiplayerHotkeyCategory", InputKey.D0 + i, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			}
			for (int j = 1; j <= 9; j++)
			{
				base.RegisterHotKey(new HotKey("SpectateCameraPosition" + j.ToString(), "MultiplayerHotkeyCategory", InputKey.D0 + j, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			}
			List<Key> list = new List<Key>
			{
				new Key(InputKey.RightMouseButton),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.RightMouseButton),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.F),
				new Key(InputKey.ControllerRLeft)
			};
			base.RegisterHotKey(new HotKey("PerformActionOnCosmeticItem", "MultiplayerHotkeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("PreviewCosmeticItem", "MultiplayerHotkeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("InspectBadgeProgression", "MultiplayerHotkeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ToggleFriendsList", "MultiplayerHotkeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x0007446A File Offset: 0x0007266A
		private void RegisterGameKeys()
		{
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x0007446C File Offset: 0x0007266C
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C68 RID: 3176
		public const string CategoryId = "MultiplayerHotkeyCategory";

		// Token: 0x04000C69 RID: 3177
		private const string _storeCameraPositionBase = "StoreCameraPosition";

		// Token: 0x04000C6A RID: 3178
		public const string StoreCameraPosition1 = "StoreCameraPosition1";

		// Token: 0x04000C6B RID: 3179
		public const string StoreCameraPosition2 = "StoreCameraPosition2";

		// Token: 0x04000C6C RID: 3180
		public const string StoreCameraPosition3 = "StoreCameraPosition3";

		// Token: 0x04000C6D RID: 3181
		public const string StoreCameraPosition4 = "StoreCameraPosition4";

		// Token: 0x04000C6E RID: 3182
		public const string StoreCameraPosition5 = "StoreCameraPosition5";

		// Token: 0x04000C6F RID: 3183
		public const string StoreCameraPosition6 = "StoreCameraPosition6";

		// Token: 0x04000C70 RID: 3184
		public const string StoreCameraPosition7 = "StoreCameraPosition7";

		// Token: 0x04000C71 RID: 3185
		public const string StoreCameraPosition8 = "StoreCameraPosition8";

		// Token: 0x04000C72 RID: 3186
		public const string StoreCameraPosition9 = "StoreCameraPosition9";

		// Token: 0x04000C73 RID: 3187
		private const string _spectateCameraPositionBase = "SpectateCameraPosition";

		// Token: 0x04000C74 RID: 3188
		public const string SpectateCameraPosition1 = "SpectateCameraPosition1";

		// Token: 0x04000C75 RID: 3189
		public const string SpectateCameraPosition2 = "SpectateCameraPosition2";

		// Token: 0x04000C76 RID: 3190
		public const string SpectateCameraPosition3 = "SpectateCameraPosition3";

		// Token: 0x04000C77 RID: 3191
		public const string SpectateCameraPosition4 = "SpectateCameraPosition4";

		// Token: 0x04000C78 RID: 3192
		public const string SpectateCameraPosition5 = "SpectateCameraPosition5";

		// Token: 0x04000C79 RID: 3193
		public const string SpectateCameraPosition6 = "SpectateCameraPosition6";

		// Token: 0x04000C7A RID: 3194
		public const string SpectateCameraPosition7 = "SpectateCameraPosition7";

		// Token: 0x04000C7B RID: 3195
		public const string SpectateCameraPosition8 = "SpectateCameraPosition8";

		// Token: 0x04000C7C RID: 3196
		public const string SpectateCameraPosition9 = "SpectateCameraPosition9";

		// Token: 0x04000C7D RID: 3197
		public const string InspectBadgeProgression = "InspectBadgeProgression";

		// Token: 0x04000C7E RID: 3198
		public const string PerformActionOnCosmeticItem = "PerformActionOnCosmeticItem";

		// Token: 0x04000C7F RID: 3199
		public const string PreviewCosmeticItem = "PreviewCosmeticItem";

		// Token: 0x04000C80 RID: 3200
		public const string ToggleFriendsList = "ToggleFriendsList";
	}
}
