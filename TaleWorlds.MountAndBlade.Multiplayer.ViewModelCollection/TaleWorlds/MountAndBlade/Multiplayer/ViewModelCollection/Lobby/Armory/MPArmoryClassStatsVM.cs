using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x02000077 RID: 119
	public class MPArmoryClassStatsVM : ViewModel
	{
		// Token: 0x06000BC3 RID: 3011 RVA: 0x000234E9 File Offset: 0x000216E9
		public MPArmoryClassStatsVM()
		{
			this._dummyPerkList = new List<IReadOnlyPerkObject>();
			this.FactionDescription = new TextObject("{=5Pea977J}Faction: ", null).ToString();
			this.HeroInformation = new HeroInformationVM();
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002351D File Offset: 0x0002171D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CostHint = new HintViewModel(GameTexts.FindText("str_armory_troop_cost", null), null);
			this.HeroInformation.RefreshValues();
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00023548 File Offset: 0x00021748
		public void RefreshWith(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			this.FactionName = heroClass.Culture.Name.ToString();
			this.FlavorText = GameTexts.FindText("str_troop_description", heroClass.StringId).ToString();
			this.HeroInformation.RefreshWith(heroClass, this._dummyPerkList);
			this.Cost = heroClass.TroopCost;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x000235A4 File Offset: 0x000217A4
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x000235AC File Offset: 0x000217AC
		[DataSourceProperty]
		public string FactionDescription
		{
			get
			{
				return this._factionDescription;
			}
			set
			{
				if (value != this._factionDescription)
				{
					this._factionDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionDescription");
				}
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x000235CF File Offset: 0x000217CF
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x000235D7 File Offset: 0x000217D7
		[DataSourceProperty]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (value != this._factionName)
				{
					this._factionName = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionName");
				}
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x000235FA File Offset: 0x000217FA
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x00023602 File Offset: 0x00021802
		[DataSourceProperty]
		public string FlavorText
		{
			get
			{
				return this._flavorText;
			}
			set
			{
				if (value != this._flavorText)
				{
					this._flavorText = value;
					base.OnPropertyChangedWithValue<string>(value, "FlavorText");
				}
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00023625 File Offset: 0x00021825
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0002362D File Offset: 0x0002182D
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0002364B File Offset: 0x0002184B
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x00023653 File Offset: 0x00021853
		[DataSourceProperty]
		public HintViewModel CostHint
		{
			get
			{
				return this._costHint;
			}
			set
			{
				if (value != this._costHint)
				{
					this._costHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CostHint");
				}
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00023671 File Offset: 0x00021871
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x00023679 File Offset: 0x00021879
		[DataSourceProperty]
		public HeroInformationVM HeroInformation
		{
			get
			{
				return this._heroInformation;
			}
			set
			{
				if (value != this._heroInformation)
				{
					this._heroInformation = value;
					base.OnPropertyChangedWithValue<HeroInformationVM>(value, "HeroInformation");
				}
			}
		}

		// Token: 0x04000556 RID: 1366
		private readonly List<IReadOnlyPerkObject> _dummyPerkList;

		// Token: 0x04000557 RID: 1367
		private string _factionDescription;

		// Token: 0x04000558 RID: 1368
		private string _factionName;

		// Token: 0x04000559 RID: 1369
		private string _flavorText;

		// Token: 0x0400055A RID: 1370
		private int _cost;

		// Token: 0x0400055B RID: 1371
		private HintViewModel _costHint;

		// Token: 0x0400055C RID: 1372
		private HeroInformationVM _heroInformation;
	}
}
