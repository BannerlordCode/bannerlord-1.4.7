using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000098 RID: 152
	public class InventoryCharacterSelectorItemVM : SelectorItemVM
	{
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0003D751 File Offset: 0x0003B951
		// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x0003D759 File Offset: 0x0003B959
		public string CharacterID { get; private set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0003D762 File Offset: 0x0003B962
		// (set) Token: 0x06000EAA RID: 3754 RVA: 0x0003D76A File Offset: 0x0003B96A
		public Hero Hero { get; private set; }

		// Token: 0x06000EAB RID: 3755 RVA: 0x0003D773 File Offset: 0x0003B973
		public InventoryCharacterSelectorItemVM(string characterID, Hero hero, TextObject characterName)
			: base(characterName)
		{
			this.Hero = hero;
			this.CharacterID = characterID;
		}
	}
}
