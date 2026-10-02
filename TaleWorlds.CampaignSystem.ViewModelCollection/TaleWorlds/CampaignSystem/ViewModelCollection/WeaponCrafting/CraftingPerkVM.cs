using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x020000FB RID: 251
	public class CraftingPerkVM : ViewModel
	{
		// Token: 0x060016B2 RID: 5810 RVA: 0x000581EF File Offset: 0x000563EF
		public CraftingPerkVM(PerkObject perk)
		{
			this.Perk = perk;
			this.Name = this.Perk.Name.ToString();
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00058214 File Offset: 0x00056414
		// (set) Token: 0x060016B4 RID: 5812 RVA: 0x0005821C File Offset: 0x0005641C
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x04000A60 RID: 2656
		public readonly PerkObject Perk;

		// Token: 0x04000A61 RID: 2657
		private string _name;
	}
}
