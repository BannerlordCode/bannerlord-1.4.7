using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters
{
	// Token: 0x02000131 RID: 305
	public class ClanSupporterItemVM : ViewModel
	{
		// Token: 0x06001CA4 RID: 7332 RVA: 0x00069FF7 File Offset: 0x000681F7
		public ClanSupporterItemVM(Hero hero)
		{
			this.Hero = new HeroVM(hero, false);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0006A00C File Offset: 0x0006820C
		public void ExecuteOpenTooltip()
		{
			InformationManager.ShowTooltip(typeof(Hero), new object[]
			{
				this.Hero.Hero,
				false
			});
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0006A03A File Offset: 0x0006823A
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x0006A041 File Offset: 0x00068241
		// (set) Token: 0x06001CA8 RID: 7336 RVA: 0x0006A049 File Offset: 0x00068249
		[DataSourceProperty]
		public HeroVM Hero
		{
			get
			{
				return this._hero;
			}
			set
			{
				if (value != this._hero)
				{
					this._hero = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Hero");
				}
			}
		}

		// Token: 0x04000D5C RID: 3420
		private HeroVM _hero;
	}
}
