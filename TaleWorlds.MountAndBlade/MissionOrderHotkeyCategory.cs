using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000232 RID: 562
	public sealed class MissionOrderHotkeyCategory : GameKeyContext
	{
		// Token: 0x060020D6 RID: 8406 RVA: 0x00073F8C File Offset: 0x0007218C
		public MissionOrderHotkeyCategory()
			: base("MissionOrderHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00073FAE File Offset: 0x000721AE
		private void RegisterHotKeys()
		{
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00073FB0 File Offset: 0x000721B0
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(68, "ViewOrders", "MissionOrderHotkeyCategory", InputKey.BackSpace, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(69, "SelectOrder1", "MissionOrderHotkeyCategory", InputKey.F1, InputKey.ControllerRLeft, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(70, "SelectOrder2", "MissionOrderHotkeyCategory", InputKey.F2, InputKey.ControllerRDown, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(71, "SelectOrder3", "MissionOrderHotkeyCategory", InputKey.F3, InputKey.ControllerRRight, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(72, "SelectOrder4", "MissionOrderHotkeyCategory", InputKey.F4, InputKey.ControllerRUp, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(73, "SelectOrder5", "MissionOrderHotkeyCategory", InputKey.F5, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(74, "SelectOrder6", "MissionOrderHotkeyCategory", InputKey.F6, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(75, "SelectOrder7", "MissionOrderHotkeyCategory", InputKey.F7, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(76, "SelectOrder8", "MissionOrderHotkeyCategory", InputKey.F8, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(77, "SelectOrderReturn", "MissionOrderHotkeyCategory", InputKey.F9, InputKey.ControllerROption, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(78, "EveryoneHear", "MissionOrderHotkeyCategory", InputKey.D0, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(79, "Group0Hear", "MissionOrderHotkeyCategory", InputKey.D1, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(80, "Group1Hear", "MissionOrderHotkeyCategory", InputKey.D2, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(81, "Group2Hear", "MissionOrderHotkeyCategory", InputKey.D3, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(82, "Group3Hear", "MissionOrderHotkeyCategory", InputKey.D4, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(83, "Group4Hear", "MissionOrderHotkeyCategory", InputKey.D5, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(84, "Group5Hear", "MissionOrderHotkeyCategory", InputKey.D6, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(85, "Group6Hear", "MissionOrderHotkeyCategory", InputKey.D7, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(86, "Group7Hear", "MissionOrderHotkeyCategory", InputKey.D8, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(87, "HoldOrder", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLBumper, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(88, "SelectLeftFormation", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLLeft, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(89, "SelectRightFormation", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLRight, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(90, "ApplySelection", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLDown, GameKeyMainCategories.OrderMenuCategory), true);
			base.RegisterGameKey(new GameKey(91, "ToggleSelection", "MissionOrderHotkeyCategory", InputKey.Invalid, InputKey.ControllerLUp, GameKeyMainCategories.OrderMenuCategory), true);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000742CB File Offset: 0x000724CB
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C4F RID: 3151
		public const string CategoryId = "MissionOrderHotkeyCategory";

		// Token: 0x04000C50 RID: 3152
		public const int ViewOrders = 68;

		// Token: 0x04000C51 RID: 3153
		public const int SelectOrder1 = 69;

		// Token: 0x04000C52 RID: 3154
		public const int SelectOrder2 = 70;

		// Token: 0x04000C53 RID: 3155
		public const int SelectOrder3 = 71;

		// Token: 0x04000C54 RID: 3156
		public const int SelectOrder4 = 72;

		// Token: 0x04000C55 RID: 3157
		public const int SelectOrder5 = 73;

		// Token: 0x04000C56 RID: 3158
		public const int SelectOrder6 = 74;

		// Token: 0x04000C57 RID: 3159
		public const int SelectOrder7 = 75;

		// Token: 0x04000C58 RID: 3160
		public const int SelectOrder8 = 76;

		// Token: 0x04000C59 RID: 3161
		public const int SelectOrderReturn = 77;

		// Token: 0x04000C5A RID: 3162
		public const int EveryoneHear = 78;

		// Token: 0x04000C5B RID: 3163
		public const int Group0Hear = 79;

		// Token: 0x04000C5C RID: 3164
		public const int Group1Hear = 80;

		// Token: 0x04000C5D RID: 3165
		public const int Group2Hear = 81;

		// Token: 0x04000C5E RID: 3166
		public const int Group3Hear = 82;

		// Token: 0x04000C5F RID: 3167
		public const int Group4Hear = 83;

		// Token: 0x04000C60 RID: 3168
		public const int Group5Hear = 84;

		// Token: 0x04000C61 RID: 3169
		public const int Group6Hear = 85;

		// Token: 0x04000C62 RID: 3170
		public const int Group7Hear = 86;

		// Token: 0x04000C63 RID: 3171
		public const int HoldOrder = 87;

		// Token: 0x04000C64 RID: 3172
		public const int SelectLeftFormation = 88;

		// Token: 0x04000C65 RID: 3173
		public const int SelectRightFormation = 89;

		// Token: 0x04000C66 RID: 3174
		public const int ApplySelection = 90;

		// Token: 0x04000C67 RID: 3175
		public const int ToggleSelection = 91;
	}
}
