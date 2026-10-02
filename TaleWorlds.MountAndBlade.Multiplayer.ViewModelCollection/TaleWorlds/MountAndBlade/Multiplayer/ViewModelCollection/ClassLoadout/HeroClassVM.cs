using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A3 RID: 163
	public class HeroClassVM : ViewModel
	{
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0002FC7B File Offset: 0x0002DE7B
		// (set) Token: 0x06000F7D RID: 3965 RVA: 0x0002FC83 File Offset: 0x0002DE83
		public List<IReadOnlyPerkObject> SelectedPerks { get; private set; }

		// Token: 0x06000F7E RID: 3966 RVA: 0x0002FC8C File Offset: 0x0002DE8C
		public HeroClassVM(Action<HeroClassVM> onSelect, Action<HeroPerkVM, MPPerkVM> onPerkSelect, MultiplayerClassDivisions.MPHeroClass heroClass, MultiplayerBattleColors.MultiplayerCultureColorInfo colorInfo)
		{
			this.HeroClass = heroClass;
			this._onSelect = onSelect;
			this._onPerkSelect = onPerkSelect;
			this.CultureId = heroClass.Culture.StringId;
			this.IconType = heroClass.IconType.ToString();
			this.TroopTypeId = heroClass.ClassGroup.StringId;
			this.CultureColor = colorInfo.Color1;
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.Gold = (this._gameMode.IsGameModeUsingCasualGold ? this.HeroClass.TroopCasualCost : ((this._gameMode.GameType == MultiplayerGameType.Battle) ? this.HeroClass.TroopBattleCost : this.HeroClass.TroopCost));
			this.InitPerksList();
			int intValue = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.IsNumOfTroopsEnabled = !this._gameMode.IsInWarmup && intValue > 0;
			if (this.IsNumOfTroopsEnabled)
			{
				this.NumOfTroops = MPPerkObject.GetTroopCount(heroClass, intValue, MPPerkObject.GetOnSpawnPerkHandler(this._perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM p) => p.SelectedPerk)));
			}
			this.UpdateEnabled();
			this.RefreshValues();
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0002FDCC File Offset: 0x0002DFCC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroClass.HeroName.ToString();
			this.Perks.ApplyActionOnAllItems(delegate(HeroPerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0002FE20 File Offset: 0x0002E020
		private void InitPerksList()
		{
			List<List<IReadOnlyPerkObject>> allPerksForHeroClass = MultiplayerClassDivisions.GetAllPerksForHeroClass(this.HeroClass, null);
			if (this.SelectedPerks == null)
			{
				this.SelectedPerks = new List<IReadOnlyPerkObject>();
			}
			else
			{
				this.SelectedPerks.Clear();
			}
			for (int i = 0; i < allPerksForHeroClass.Count; i++)
			{
				if (allPerksForHeroClass[i].Count > 0)
				{
					this.SelectedPerks.Add(allPerksForHeroClass[i][0]);
				}
				else
				{
					this.SelectedPerks.Add(null);
				}
			}
			if (GameNetwork.IsMyPeerReady)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				int num = MultiplayerClassDivisions.GetMPHeroClasses(this.HeroClass.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>().IndexOf(this.HeroClass);
				component.NextSelectedTroopIndex = num;
				for (int j = 0; j < allPerksForHeroClass.Count; j++)
				{
					if (allPerksForHeroClass[j].Count > 0)
					{
						int num2 = component.GetSelectedPerkIndexWithPerkListIndex(num, j);
						if (num2 >= allPerksForHeroClass[j].Count)
						{
							num2 = 0;
						}
						IReadOnlyPerkObject readOnlyPerkObject = allPerksForHeroClass[j][num2];
						this.SelectedPerks[j] = readOnlyPerkObject;
					}
				}
			}
			MBBindingList<HeroPerkVM> mbbindingList = new MBBindingList<HeroPerkVM>();
			for (int k = 0; k < allPerksForHeroClass.Count; k++)
			{
				if (allPerksForHeroClass[k].Count > 0)
				{
					mbbindingList.Add(new HeroPerkVM(this._onPerkSelect, this.SelectedPerks[k], allPerksForHeroClass[k], k));
				}
			}
			this.Perks = mbbindingList;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0002FFA0 File Offset: 0x0002E1A0
		public void UpdateEnabled()
		{
			this.IsEnabled = this._gameMode.IsClassAvailable(this.HeroClass) && (this._gameMode.IsInWarmup || !this._gameMode.IsGameModeUsingGold || this._gameMode.GetGoldAmount() >= this.Gold);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0002FFFC File Offset: 0x0002E1FC
		[UsedImplicitly]
		public void OnSelect()
		{
			this._onSelect(this);
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0003000A File Offset: 0x0002E20A
		// (set) Token: 0x06000F84 RID: 3972 RVA: 0x00030012 File Offset: 0x0002E212
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x00030030 File Offset: 0x0002E230
		// (set) Token: 0x06000F86 RID: 3974 RVA: 0x00030038 File Offset: 0x0002E238
		[DataSourceProperty]
		public MBBindingList<HeroPerkVM> Perks
		{
			get
			{
				return this._perks;
			}
			set
			{
				if (value != this._perks)
				{
					this._perks = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroPerkVM>>(value, "Perks");
				}
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x00030056 File Offset: 0x0002E256
		// (set) Token: 0x06000F88 RID: 3976 RVA: 0x0003005E File Offset: 0x0002E25E
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x00030081 File Offset: 0x0002E281
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x00030089 File Offset: 0x0002E289
		[DataSourceProperty]
		public string TroopTypeId
		{
			get
			{
				return this._troopTypeId;
			}
			set
			{
				if (value != this._troopTypeId)
				{
					this._troopTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "TroopTypeId");
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x000300AC File Offset: 0x0002E2AC
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x000300B4 File Offset: 0x0002E2B4
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x000300D2 File Offset: 0x0002E2D2
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x000300DA File Offset: 0x0002E2DA
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

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x000300FD File Offset: 0x0002E2FD
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x00030105 File Offset: 0x0002E305
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x00030128 File Offset: 0x0002E328
		// (set) Token: 0x06000F92 RID: 3986 RVA: 0x00030130 File Offset: 0x0002E330
		[DataSourceProperty]
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				if (value != this._gold)
				{
					this._gold = value;
					base.OnPropertyChangedWithValue(value, "Gold");
				}
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0003014E File Offset: 0x0002E34E
		// (set) Token: 0x06000F94 RID: 3988 RVA: 0x00030156 File Offset: 0x0002E356
		[DataSourceProperty]
		public int NumOfTroops
		{
			get
			{
				return this._numOfTroops;
			}
			set
			{
				if (value != this._numOfTroops)
				{
					this._numOfTroops = value;
					base.OnPropertyChangedWithValue(value, "NumOfTroops");
				}
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00030174 File Offset: 0x0002E374
		// (set) Token: 0x06000F96 RID: 3990 RVA: 0x0003017C File Offset: 0x0002E37C
		[DataSourceProperty]
		public bool IsGoldEnabled
		{
			get
			{
				return this._isGoldEnabled;
			}
			set
			{
				if (value != this._isGoldEnabled)
				{
					this._isGoldEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsGoldEnabled");
				}
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0003019A File Offset: 0x0002E39A
		// (set) Token: 0x06000F98 RID: 3992 RVA: 0x000301A2 File Offset: 0x0002E3A2
		[DataSourceProperty]
		public bool IsNumOfTroopsEnabled
		{
			get
			{
				return this._isNumOfTroopsEnabled;
			}
			set
			{
				if (value != this._isNumOfTroopsEnabled)
				{
					this._isNumOfTroopsEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNumOfTroopsEnabled");
				}
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x000301C0 File Offset: 0x0002E3C0
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x000301C8 File Offset: 0x0002E3C8
		[DataSourceProperty]
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChangedWithValue(value, "CultureColor");
				}
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x000301EB File Offset: 0x0002E3EB
		[DataSourceProperty]
		public HeroPerkVM FirstPerk
		{
			get
			{
				return this.Perks.ElementAtOrDefault<HeroPerkVM>(0);
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x000301F9 File Offset: 0x0002E3F9
		[DataSourceProperty]
		public HeroPerkVM SecondPerk
		{
			get
			{
				return this.Perks.ElementAtOrDefault<HeroPerkVM>(1);
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x00030207 File Offset: 0x0002E407
		[DataSourceProperty]
		public HeroPerkVM ThirdPerk
		{
			get
			{
				return this.Perks.ElementAtOrDefault<HeroPerkVM>(2);
			}
		}

		// Token: 0x04000731 RID: 1841
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x04000732 RID: 1842
		public readonly MultiplayerClassDivisions.MPHeroClass HeroClass;

		// Token: 0x04000733 RID: 1843
		private readonly Action<HeroClassVM> _onSelect;

		// Token: 0x04000734 RID: 1844
		private Action<HeroPerkVM, MPPerkVM> _onPerkSelect;

		// Token: 0x04000736 RID: 1846
		private bool _isSelected;

		// Token: 0x04000737 RID: 1847
		private string _name;

		// Token: 0x04000738 RID: 1848
		private string _iconType;

		// Token: 0x04000739 RID: 1849
		private int _gold;

		// Token: 0x0400073A RID: 1850
		private int _numOfTroops;

		// Token: 0x0400073B RID: 1851
		private bool _isEnabled;

		// Token: 0x0400073C RID: 1852
		private bool _isGoldEnabled;

		// Token: 0x0400073D RID: 1853
		private bool _isNumOfTroopsEnabled;

		// Token: 0x0400073E RID: 1854
		private string _cultureId;

		// Token: 0x0400073F RID: 1855
		private string _troopTypeId;

		// Token: 0x04000740 RID: 1856
		private Color _cultureColor;

		// Token: 0x04000741 RID: 1857
		private MBBindingList<HeroPerkVM> _perks;
	}
}
