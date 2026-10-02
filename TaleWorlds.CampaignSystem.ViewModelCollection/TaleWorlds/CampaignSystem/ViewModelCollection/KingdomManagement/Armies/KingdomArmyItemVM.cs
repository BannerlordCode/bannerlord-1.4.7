using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x02000089 RID: 137
	public class KingdomArmyItemVM : KingdomItemVM
	{
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00030DE1 File Offset: 0x0002EFE1
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x00030DE9 File Offset: 0x0002EFE9
		public float DistanceToMainParty { get; set; }

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00030DF4 File Offset: 0x0002EFF4
		public KingdomArmyItemVM(Army army, Action<KingdomArmyItemVM> onSelect)
		{
			this.Army = army;
			this._onSelect = onSelect;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			CampaignUIHelper.GetCharacterCode(army.ArmyOwner.CharacterObject, false);
			this.Leader = new HeroVM(this.Army.LeaderParty.LeaderHero, false);
			this.LordCount = army.Parties.Count;
			this.Strength = army.Parties.Sum<MobileParty>((MobileParty p) => p.Party.NumberOfAllMembers);
			this.Location = CampaignUIHelper.GetPartyLocationText(army.LeaderParty);
			this.Behavior = army.GetLongTermBehaviorText(true).ToString();
			this.UpdateIsNew();
			this.Cohesion = (int)this.Army.Cohesion;
			this.Parties = new MBBindingList<KingdomArmyPartyItemVM>();
			foreach (MobileParty mobileParty in this.Army.Parties)
			{
				this.Parties.Add(new KingdomArmyPartyItemVM(mobileParty));
			}
			this.DistanceToMainParty = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(army.LeaderParty, MobileParty.MainParty, army.LeaderParty.NavigationCapability);
			this.IsMainArmy = army.LeaderParty == MobileParty.MainParty;
			this.RefreshValues();
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00030F6C File Offset: 0x0002F16C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmyName = this.Army.Name.ToString();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_cohesion", null));
			GameTexts.SetVariable("STR2", this.Cohesion.ToString());
			this.CohesionLabel = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_men_count", null));
			GameTexts.SetVariable("RIGHT", this.Strength.ToString());
			this.StrengthLabel = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
			this.ShipCount = this.Army.Parties.Sum<MobileParty>(delegate(MobileParty p)
			{
				MBReadOnlyList<Ship> ships = p.Ships;
				if (ships == null)
				{
					return 0;
				}
				return ships.Count;
			});
			this.ShipCountLabel = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).SetTextVariable("LEFT", new TextObject("{=URbKirPS}Ship Count", null)).SetTextVariable("RIGHT", this.ShipCount)
				.ToString();
			this.Parties.ApplyActionOnAllItems(delegate(KingdomArmyPartyItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000310B5 File Offset: 0x0002F2B5
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
			this.ExecuteResetNew();
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000310CF File Offset: 0x0002F2CF
		private void ExecuteResetNew()
		{
			if (base.IsNew)
			{
				this._viewDataTracker.OnArmyExamined(this.Army);
				this.UpdateIsNew();
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000310F0 File Offset: 0x0002F2F0
		private void UpdateIsNew()
		{
			base.IsNew = this._viewDataTracker.UnExaminedArmies.Any<Army>((Army a) => a == this.Army);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00031114 File Offset: 0x0002F314
		protected void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00031126 File Offset: 0x0002F326
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x0003112E File Offset: 0x0002F32E
		[DataSourceProperty]
		public MBBindingList<KingdomArmyPartyItemVM> Parties
		{
			get
			{
				return this._parties;
			}
			set
			{
				if (value != this._parties)
				{
					this._parties = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomArmyPartyItemVM>>(value, "Parties");
				}
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0003114C File Offset: 0x0002F34C
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x00031154 File Offset: 0x0002F354
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChanged("Visual");
				}
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00031171 File Offset: 0x0002F371
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00031179 File Offset: 0x0002F379
		[DataSourceProperty]
		public string ArmyName
		{
			get
			{
				return this._armyName;
			}
			set
			{
				if (value != this._armyName)
				{
					this._armyName = value;
					base.OnPropertyChangedWithValue<string>(value, "ArmyName");
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0003119C File Offset: 0x0002F39C
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x000311A4 File Offset: 0x0002F3A4
		[DataSourceProperty]
		public int Cohesion
		{
			get
			{
				return this._cohesion;
			}
			set
			{
				if (value != this._cohesion)
				{
					this._cohesion = value;
					base.OnPropertyChangedWithValue(value, "Cohesion");
				}
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x000311C2 File Offset: 0x0002F3C2
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x000311CA File Offset: 0x0002F3CA
		[DataSourceProperty]
		public string CohesionLabel
		{
			get
			{
				return this._cohesionLabel;
			}
			set
			{
				if (value != this._cohesionLabel)
				{
					this._cohesionLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "CohesionLabel");
				}
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x000311ED File Offset: 0x0002F3ED
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x000311F5 File Offset: 0x0002F3F5
		[DataSourceProperty]
		public int LordCount
		{
			get
			{
				return this._lordCount;
			}
			set
			{
				if (value != this._lordCount)
				{
					this._lordCount = value;
					base.OnPropertyChangedWithValue(value, "LordCount");
				}
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00031213 File Offset: 0x0002F413
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0003121B File Offset: 0x0002F41B
		[DataSourceProperty]
		public int Strength
		{
			get
			{
				return this._strength;
			}
			set
			{
				if (value != this._strength)
				{
					this._strength = value;
					base.OnPropertyChangedWithValue(value, "Strength");
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00031239 File Offset: 0x0002F439
		// (set) Token: 0x06000BBC RID: 3004 RVA: 0x00031241 File Offset: 0x0002F441
		[DataSourceProperty]
		public string StrengthLabel
		{
			get
			{
				return this._strengthLabel;
			}
			set
			{
				if (value != this._strengthLabel)
				{
					this._strengthLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthLabel");
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00031264 File Offset: 0x0002F464
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x0003126C File Offset: 0x0002F46C
		[DataSourceProperty]
		public int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
				}
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0003128A File Offset: 0x0002F48A
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x00031292 File Offset: 0x0002F492
		[DataSourceProperty]
		public string ShipCountLabel
		{
			get
			{
				return this._shipCountLabel;
			}
			set
			{
				if (value != this._shipCountLabel)
				{
					this._shipCountLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipCountLabel");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x000312B5 File Offset: 0x0002F4B5
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x000312BD File Offset: 0x0002F4BD
		[DataSourceProperty]
		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				if (value != this._location)
				{
					this._location = value;
					base.OnPropertyChangedWithValue<string>(value, "Location");
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x000312E0 File Offset: 0x0002F4E0
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x000312E8 File Offset: 0x0002F4E8
		[DataSourceProperty]
		public string Behavior
		{
			get
			{
				return this._behavior;
			}
			set
			{
				if (value != this._behavior)
				{
					this._behavior = value;
					base.OnPropertyChanged("Objective");
				}
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0003130A File Offset: 0x0002F50A
		// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x00031312 File Offset: 0x0002F512
		[DataSourceProperty]
		public bool IsMainArmy
		{
			get
			{
				return this._isMainArmy;
			}
			set
			{
				if (value != this._isMainArmy)
				{
					this._isMainArmy = value;
					base.OnPropertyChangedWithValue(value, "IsMainArmy");
				}
			}
		}

		// Token: 0x04000530 RID: 1328
		public readonly Army Army;

		// Token: 0x04000532 RID: 1330
		private readonly Action<KingdomArmyItemVM> _onSelect;

		// Token: 0x04000533 RID: 1331
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x04000534 RID: 1332
		private HeroVM _leader;

		// Token: 0x04000535 RID: 1333
		private MBBindingList<KingdomArmyPartyItemVM> _parties;

		// Token: 0x04000536 RID: 1334
		private string _armyName;

		// Token: 0x04000537 RID: 1335
		private int _strength;

		// Token: 0x04000538 RID: 1336
		private int _cohesion;

		// Token: 0x04000539 RID: 1337
		private string _strengthLabel;

		// Token: 0x0400053A RID: 1338
		private string _shipCountLabel;

		// Token: 0x0400053B RID: 1339
		private int _shipCount;

		// Token: 0x0400053C RID: 1340
		private int _lordCount;

		// Token: 0x0400053D RID: 1341
		private string _location;

		// Token: 0x0400053E RID: 1342
		private string _behavior;

		// Token: 0x0400053F RID: 1343
		private string _cohesionLabel;

		// Token: 0x04000540 RID: 1344
		private bool _isMainArmy;
	}
}
