using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022C RID: 556
	public sealed class GenericCampaignPanelsGameKeyCategory : GameKeyContext
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x000733EE File Offset: 0x000715EE
		// (set) Token: 0x060020BC RID: 8380 RVA: 0x000733F5 File Offset: 0x000715F5
		public static GenericCampaignPanelsGameKeyCategory Current { get; private set; }

		// Token: 0x060020BD RID: 8381 RVA: 0x000733FD File Offset: 0x000715FD
		public GenericCampaignPanelsGameKeyCategory(string categoryId = "GenericCampaignPanelsGameKeyCategory")
			: base(categoryId, 116, GameKeyContext.GameKeyContextType.Default)
		{
			GenericCampaignPanelsGameKeyCategory.Current = this;
			this.RegisterHotKeys();
			this.RegisterGameKeys();
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x0007341C File Offset: 0x0007161C
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftShift),
				new Key(InputKey.RightShift)
			};
			base.RegisterHotKey(new HotKey("FiveStackModifier", "GenericCampaignPanelsGameKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftControl),
				new Key(InputKey.RightControl)
			};
			base.RegisterHotKey(new HotKey("EntireStackModifier", "GenericCampaignPanelsGameKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000734A0 File Offset: 0x000716A0
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(36, "BannerWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.B, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(37, "CharacterWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.C, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(38, "InventoryWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.I, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(39, "EncyclopediaWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.N, InputKey.ControllerLOption, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(40, "KingdomWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.K, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(41, "ClanWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.L, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(42, "QuestsWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.J, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(43, "PartyWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.P, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(44, "FacegenWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.V, GameKeyMainCategories.MenuShortcutCategory), true);
			base.RegisterGameKey(new GameKey(45, "ManageFleetWindow", "GenericCampaignPanelsGameKeyCategory", InputKey.U, GameKeyMainCategories.MenuShortcutCategory), true);
		}

		// Token: 0x04000C05 RID: 3077
		public const string CategoryId = "GenericCampaignPanelsGameKeyCategory";

		// Token: 0x04000C06 RID: 3078
		public const string FiveStackModifier = "FiveStackModifier";

		// Token: 0x04000C07 RID: 3079
		public const string EntireStackModifier = "EntireStackModifier";

		// Token: 0x04000C08 RID: 3080
		public const int BannerWindow = 36;

		// Token: 0x04000C09 RID: 3081
		public const int CharacterWindow = 37;

		// Token: 0x04000C0A RID: 3082
		public const int InventoryWindow = 38;

		// Token: 0x04000C0B RID: 3083
		public const int EncyclopediaWindow = 39;

		// Token: 0x04000C0C RID: 3084
		public const int PartyWindow = 43;

		// Token: 0x04000C0D RID: 3085
		public const int KingdomWindow = 40;

		// Token: 0x04000C0E RID: 3086
		public const int ClanWindow = 41;

		// Token: 0x04000C0F RID: 3087
		public const int QuestsWindow = 42;

		// Token: 0x04000C10 RID: 3088
		public const int FacegenWindow = 44;

		// Token: 0x04000C11 RID: 3089
		public const int ManageFleetWindow = 45;
	}
}
