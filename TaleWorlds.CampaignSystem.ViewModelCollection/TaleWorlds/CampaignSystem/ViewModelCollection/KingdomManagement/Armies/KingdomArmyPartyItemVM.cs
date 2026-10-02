using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x0200008A RID: 138
	public class KingdomArmyPartyItemVM : ViewModel
	{
		// Token: 0x06000BC8 RID: 3016 RVA: 0x0003133B File Offset: 0x0002F53B
		public KingdomArmyPartyItemVM(MobileParty party)
		{
			this._party = party;
			Hero leaderHero = party.LeaderHero;
			this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode((leaderHero != null) ? leaderHero.CharacterObject : null, false));
			this.RefreshValues();
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00031373 File Offset: 0x0002F573
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._party.Name.ToString();
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00031391 File Offset: 0x0002F591
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(MobileParty), new object[] { this._party, true, false });
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000313C3 File Offset: 0x0002F5C3
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000313CA File Offset: 0x0002F5CA
		public void ExecuteLink()
		{
			if (this._party != null && this._party.LeaderHero != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._party.LeaderHero.EncyclopediaLink);
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00031400 File Offset: 0x0002F600
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x00031408 File Offset: 0x0002F608
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

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00031426 File Offset: 0x0002F626
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x0003142E File Offset: 0x0002F62E
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

		// Token: 0x04000541 RID: 1345
		private MobileParty _party;

		// Token: 0x04000542 RID: 1346
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000543 RID: 1347
		private string _name;
	}
}
