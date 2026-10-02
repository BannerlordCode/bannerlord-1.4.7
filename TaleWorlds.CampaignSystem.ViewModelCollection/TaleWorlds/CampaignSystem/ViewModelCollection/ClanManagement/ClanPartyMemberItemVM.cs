using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012B RID: 299
	public class ClanPartyMemberItemVM : ViewModel
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x00067CF9 File Offset: 0x00065EF9
		// (set) Token: 0x06001C0E RID: 7182 RVA: 0x00067D01 File Offset: 0x00065F01
		public Hero HeroObject { get; private set; }

		// Token: 0x06001C0F RID: 7183 RVA: 0x00067D0C File Offset: 0x00065F0C
		public ClanPartyMemberItemVM(Hero hero, MobileParty party)
		{
			this.HeroObject = hero;
			this.IsLeader = hero == party.LeaderHero;
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false);
			this.Visual = new CharacterImageIdentifierVM(characterCode);
			this.HeroModel = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.HeroModel.FillFrom(this.HeroObject, -1, false, false);
			this.RefreshValues();
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00067D74 File Offset: 0x00065F74
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroObject.Name.ToString();
			this.UpdateProperties();
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00067D98 File Offset: 0x00065F98
		private void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x00067DAA File Offset: 0x00065FAA
		public void UpdateProperties()
		{
			this.HeroModel = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.HeroModel.FillFrom(this.HeroObject, -1, false, false);
			this.Banner_9 = new BannerImageIdentifierVM(this.HeroObject.ClanBanner, true);
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00067DE3 File Offset: 0x00065FE3
		public void ExecuteLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.HeroObject.EncyclopediaLink);
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x00067DFF File Offset: 0x00065FFF
		public virtual void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Hero), new object[] { this.HeroObject, true });
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00067E28 File Offset: 0x00066028
		public virtual void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00067E2F File Offset: 0x0006602F
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroModel.OnFinalize();
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06001C17 RID: 7191 RVA: 0x00067E42 File Offset: 0x00066042
		// (set) Token: 0x06001C18 RID: 7192 RVA: 0x00067E4A File Offset: 0x0006604A
		[DataSourceProperty]
		public HeroViewModel HeroModel
		{
			get
			{
				return this._heroModel;
			}
			set
			{
				if (value != this._heroModel)
				{
					this._heroModel = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "HeroModel");
				}
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x00067E68 File Offset: 0x00066068
		// (set) Token: 0x06001C1A RID: 7194 RVA: 0x00067E70 File Offset: 0x00066070
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x00067E8E File Offset: 0x0006608E
		// (set) Token: 0x06001C1C RID: 7196 RVA: 0x00067E96 File Offset: 0x00066096
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x00067EB4 File Offset: 0x000660B4
		// (set) Token: 0x06001C1E RID: 7198 RVA: 0x00067EBC File Offset: 0x000660BC
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

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x00067EDF File Offset: 0x000660DF
		// (set) Token: 0x06001C20 RID: 7200 RVA: 0x00067EE7 File Offset: 0x000660E7
		[DataSourceProperty]
		public bool IsLeader
		{
			get
			{
				return this._isLeader;
			}
			set
			{
				if (value != this._isLeader)
				{
					this._isLeader = value;
					base.OnPropertyChangedWithValue(value, "IsLeader");
				}
			}
		}

		// Token: 0x04000D17 RID: 3351
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000D18 RID: 3352
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x04000D19 RID: 3353
		private string _name;

		// Token: 0x04000D1A RID: 3354
		private bool _isLeader;

		// Token: 0x04000D1B RID: 3355
		private HeroViewModel _heroModel;
	}
}
