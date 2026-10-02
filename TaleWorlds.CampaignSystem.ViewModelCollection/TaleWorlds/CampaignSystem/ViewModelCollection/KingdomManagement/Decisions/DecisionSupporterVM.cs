using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x02000077 RID: 119
	public class DecisionSupporterVM : ViewModel
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x0002A9E4 File Offset: 0x00028BE4
		public DecisionSupporterVM(TextObject name, string imagePath, Clan clan, Supporter.SupportWeights weight)
		{
			this._nameObj = name;
			this._clan = clan;
			this._weight = weight;
			this.SupportWeightImagePath = DecisionSupporterVM.GetSupporterWeightImagePath(weight);
			this.RefreshValues();
			this._hero = Hero.FindFirst((Hero H) => H.Name == name);
			if (this._hero != null)
			{
				this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(this._hero.CharacterObject, false));
				return;
			}
			this.Visual = new CharacterImageIdentifierVM(null);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002AA7A File Offset: 0x00028C7A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002AA93 File Offset: 0x00028C93
		private void ExecuteBeginHint()
		{
			if (this._hero != null)
			{
				InformationManager.ShowTooltip(typeof(Hero), new object[] { this._hero, false });
			}
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002AAC4 File Offset: 0x00028CC4
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0002AACB File Offset: 0x00028CCB
		internal static string GetSupporterWeightImagePath(Supporter.SupportWeights weight)
		{
			switch (weight)
			{
			case Supporter.SupportWeights.SlightlyFavor:
				return "SPKingdom\\voter_strength1";
			case Supporter.SupportWeights.StronglyFavor:
				return "SPKingdom\\voter_strength2";
			case Supporter.SupportWeights.FullyPush:
				return "SPKingdom\\voter_strength3";
			}
			return string.Empty;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0002AB00 File Offset: 0x00028D00
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0002AB08 File Offset: 0x00028D08
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

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0002AB26 File Offset: 0x00028D26
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0002AB2E File Offset: 0x00028D2E
		[DataSourceProperty]
		public int SupportStrength
		{
			get
			{
				return this._supportStrength;
			}
			set
			{
				if (value != this._supportStrength)
				{
					this._supportStrength = value;
					base.OnPropertyChangedWithValue(value, "SupportStrength");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0002AB4C File Offset: 0x00028D4C
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0002AB54 File Offset: 0x00028D54
		[DataSourceProperty]
		public string SupportWeightImagePath
		{
			get
			{
				return this._supportWeightImagePath;
			}
			set
			{
				if (value != this._supportWeightImagePath)
				{
					this._supportWeightImagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportWeightImagePath");
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0002AB77 File Offset: 0x00028D77
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0002AB7F File Offset: 0x00028D7F
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
					base.OnPropertyChanged("string");
				}
			}
		}

		// Token: 0x04000451 RID: 1105
		private Supporter.SupportWeights _weight;

		// Token: 0x04000452 RID: 1106
		private Clan _clan;

		// Token: 0x04000453 RID: 1107
		private TextObject _nameObj;

		// Token: 0x04000454 RID: 1108
		private Hero _hero;

		// Token: 0x04000455 RID: 1109
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000456 RID: 1110
		private string _name;

		// Token: 0x04000457 RID: 1111
		private int _supportStrength;

		// Token: 0x04000458 RID: 1112
		private string _supportWeightImagePath;
	}
}
