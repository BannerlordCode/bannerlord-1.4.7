using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B1 RID: 177
	public class RecruitVolunteerOwnerVM : HeroVM
	{
		// Token: 0x06001172 RID: 4466 RVA: 0x00045E2F File Offset: 0x0004402F
		public RecruitVolunteerOwnerVM(Hero hero, int relation)
			: base(hero, hero != null && hero.IsNotable)
		{
			this._hero = hero;
			this.RelationToPlayer = relation;
			this.RefreshValues();
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00045E58 File Offset: 0x00044058
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._hero != null)
			{
				if (this._hero.IsPreacher)
				{
					this.TitleText = GameTexts.FindText("str_preacher", null).ToString();
					return;
				}
				if (this._hero.IsGangLeader)
				{
					this.TitleText = GameTexts.FindText("str_gang_leader", null).ToString();
					return;
				}
				if (this._hero.IsMerchant)
				{
					this.TitleText = GameTexts.FindText("str_merchant", null).ToString();
					return;
				}
				if (this._hero.IsRuralNotable)
				{
					this.TitleText = GameTexts.FindText("str_rural_notable", null).ToString();
				}
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00045F05 File Offset: 0x00044105
		public void ExecuteOpenEncyclopedia()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._hero.EncyclopediaLink);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00045F21 File Offset: 0x00044121
		public void ExecuteFocus()
		{
			Action<RecruitVolunteerOwnerVM> onFocused = RecruitVolunteerOwnerVM.OnFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(this);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00045F33 File Offset: 0x00044133
		public void ExecuteUnfocus()
		{
			Action<RecruitVolunteerOwnerVM> onFocused = RecruitVolunteerOwnerVM.OnFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(null);
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x00045F45 File Offset: 0x00044145
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x00045F4D File Offset: 0x0004414D
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x00045F70 File Offset: 0x00044170
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x00045F78 File Offset: 0x00044178
		[DataSourceProperty]
		public int RelationToPlayer
		{
			get
			{
				return this._relationToPlayer;
			}
			set
			{
				if (value != this._relationToPlayer)
				{
					this._relationToPlayer = value;
					base.OnPropertyChangedWithValue(value, "RelationToPlayer");
				}
			}
		}

		// Token: 0x040007F2 RID: 2034
		public static Action<RecruitVolunteerOwnerVM> OnFocused;

		// Token: 0x040007F3 RID: 2035
		private Hero _hero;

		// Token: 0x040007F4 RID: 2036
		private string _titleText;

		// Token: 0x040007F5 RID: 2037
		private int _relationToPlayer;
	}
}
