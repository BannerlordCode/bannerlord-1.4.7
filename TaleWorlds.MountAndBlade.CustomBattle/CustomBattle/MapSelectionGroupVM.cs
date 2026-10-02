using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x0200001C RID: 28
	public class MapSelectionGroupVM : ViewModel
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000959D File Offset: 0x0000779D
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000095A5 File Offset: 0x000077A5
		public int SelectedWallBreachedCount { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000095AE File Offset: 0x000077AE
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000095B6 File Offset: 0x000077B6
		public int SelectedSceneLevel { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000095BF File Offset: 0x000077BF
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000095C7 File Offset: 0x000077C7
		public int SelectedTimeOfDay { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000095D0 File Offset: 0x000077D0
		// (set) Token: 0x06000153 RID: 339 RVA: 0x000095D8 File Offset: 0x000077D8
		public string SelectedSeasonId { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000095E1 File Offset: 0x000077E1
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000095E9 File Offset: 0x000077E9
		public MapItemVM SelectedMap { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000095F2 File Offset: 0x000077F2
		// (set) Token: 0x06000157 RID: 343 RVA: 0x000095FA File Offset: 0x000077FA
		private List<MapItemVM> _battleMaps { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00009603 File Offset: 0x00007803
		// (set) Token: 0x06000159 RID: 345 RVA: 0x0000960B File Offset: 0x0000780B
		private List<MapItemVM> _villageMaps { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00009614 File Offset: 0x00007814
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000961C File Offset: 0x0000781C
		private List<MapItemVM> _siegeMaps { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00009625 File Offset: 0x00007825
		// (set) Token: 0x0600015D RID: 349 RVA: 0x0000962D File Offset: 0x0000782D
		private List<MapItemVM> _availableMaps { get; set; }

		// Token: 0x0600015E RID: 350 RVA: 0x00009638 File Offset: 0x00007838
		public MapSelectionGroupVM()
		{
			this._battleMaps = new List<MapItemVM>();
			this._villageMaps = new List<MapItemVM>();
			this._siegeMaps = new List<MapItemVM>();
			this._availableMaps = this._battleMaps;
			this.MapSelection = new SelectorVM<MapItemVM>(0, new Action<SelectorVM<MapItemVM>>(this.OnMapSelection));
			this.WallHitpointSelection = new SelectorVM<WallHitpointItemVM>(0, new Action<SelectorVM<WallHitpointItemVM>>(this.OnWallHitpointSelection));
			this.SceneLevelSelection = new SelectorVM<SceneLevelItemVM>(0, new Action<SelectorVM<SceneLevelItemVM>>(this.OnSceneLevelSelection));
			this.SeasonSelection = new SelectorVM<SeasonItemVM>(0, new Action<SelectorVM<SeasonItemVM>>(this.OnSeasonSelection));
			this.TimeOfDaySelection = new SelectorVM<TimeOfDayItemVM>(0, new Action<SelectorVM<TimeOfDayItemVM>>(this.OnTimeOfDaySelection));
			this.RefreshValues();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000096F8 File Offset: 0x000078F8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PrepareMapLists();
			this.TitleText = new TextObject("{=customgametitle}Map", null).ToString();
			this.MapText = new TextObject("{=customgamemapname}Map", null).ToString();
			this.SeasonText = new TextObject("{=xTzDM5XE}Season", null).ToString();
			this.TimeOfDayText = new TextObject("{=DszSWnc3}Time of Day", null).ToString();
			this.SceneLevelText = new TextObject("{=0s52GQJt}Scene Level", null).ToString();
			this.WallHitpointsText = new TextObject("{=4IuXGSdc}Wall Hitpoints", null).ToString();
			this.AttackerSiegeMachinesText = new TextObject("{=AmfIfeIc}Choose Attacker Siege Machines", null).ToString();
			this.DefenderSiegeMachinesText = new TextObject("{=UoiSWe87}Choose Defender Siege Machines", null).ToString();
			this.SalloutText = new TextObject("{=EcKMGoFv}Sallyout", null).ToString();
			this.MapSelection.ItemList.Clear();
			this.WallHitpointSelection.ItemList.Clear();
			this.SceneLevelSelection.ItemList.Clear();
			this.SeasonSelection.ItemList.Clear();
			this.TimeOfDaySelection.ItemList.Clear();
			foreach (MapItemVM mapItemVM in this._availableMaps)
			{
				this.MapSelection.AddItem(new MapItemVM(mapItemVM.MapName, mapItemVM.MapId, mapItemVM.ForcedSceneLevel));
			}
			foreach (Tuple<string, int> tuple in CustomBattleData.WallHitpoints)
			{
				this.WallHitpointSelection.AddItem(new WallHitpointItemVM(tuple.Item1, tuple.Item2));
			}
			foreach (int num in CustomBattleData.SceneLevels)
			{
				this.SceneLevelSelection.AddItem(new SceneLevelItemVM(num));
			}
			foreach (Tuple<string, string> tuple2 in CustomBattleData.Seasons)
			{
				this.SeasonSelection.AddItem(new SeasonItemVM(tuple2.Item1, tuple2.Item2));
			}
			foreach (Tuple<string, CustomBattleTimeOfDay> tuple3 in CustomBattleData.TimesOfDay)
			{
				this.TimeOfDaySelection.AddItem(new TimeOfDayItemVM(tuple3.Item1, (int)tuple3.Item2));
			}
			this.MapSelection.SelectedIndex = 0;
			this.WallHitpointSelection.SelectedIndex = 0;
			this.SceneLevelSelection.SelectedIndex = 0;
			this.SeasonSelection.SelectedIndex = 0;
			this.TimeOfDaySelection.SelectedIndex = 0;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009A10 File Offset: 0x00007C10
		public void ExecuteSallyOutChange()
		{
			this.IsSallyOutSelected = !this.IsSallyOutSelected;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009A24 File Offset: 0x00007C24
		private void PrepareMapLists()
		{
			this._battleMaps.Clear();
			this._villageMaps.Clear();
			this._siegeMaps.Clear();
			bool isOnlyCoreContentEnabled = Module.CurrentModule.IsOnlyCoreContentEnabled;
			if (CustomGame.Current != null)
			{
				IEnumerable<CustomBattleSceneData> enumerable;
				if (isOnlyCoreContentEnabled)
				{
					enumerable = CustomGame.Current.CustomBattleScenes.Where<CustomBattleSceneData>((CustomBattleSceneData s) => s.SceneID == "battle_terrain_029");
				}
				else
				{
					IEnumerable<CustomBattleSceneData> enumerable2 = CustomGame.Current.CustomBattleScenes.ToList<CustomBattleSceneData>();
					enumerable = enumerable2;
				}
				foreach (CustomBattleSceneData customBattleSceneData in enumerable)
				{
					MapItemVM mapItemVM = new MapItemVM(customBattleSceneData.Name.ToString(), customBattleSceneData.SceneID, customBattleSceneData.ForcedSceneLevel);
					if (customBattleSceneData.IsVillageMap)
					{
						this._villageMaps.Add(mapItemVM);
					}
					else if (customBattleSceneData.IsSiegeMap)
					{
						this._siegeMaps.Add(mapItemVM);
					}
					else if (!customBattleSceneData.IsLordsHallMap)
					{
						this._battleMaps.Add(mapItemVM);
					}
				}
			}
			Comparer<MapItemVM> comparer = Comparer<MapItemVM>.Create((MapItemVM x, MapItemVM y) => x.MapName.CompareTo(y.MapName));
			this._battleMaps.Sort(comparer);
			this._villageMaps.Sort(comparer);
			this._siegeMaps.Sort(comparer);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009B90 File Offset: 0x00007D90
		private void OnMapSelection(SelectorVM<MapItemVM> selector)
		{
			this.SelectedMap = selector.SelectedItem;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009B9E File Offset: 0x00007D9E
		private void OnWallHitpointSelection(SelectorVM<WallHitpointItemVM> selector)
		{
			this.SelectedWallBreachedCount = selector.SelectedItem.BreachedWallCount;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009BB1 File Offset: 0x00007DB1
		private void OnSceneLevelSelection(SelectorVM<SceneLevelItemVM> selector)
		{
			this.SelectedSceneLevel = selector.SelectedItem.Level;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009BC4 File Offset: 0x00007DC4
		private void OnSeasonSelection(SelectorVM<SeasonItemVM> selector)
		{
			this.SelectedSeasonId = selector.SelectedItem.SeasonId;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009BD7 File Offset: 0x00007DD7
		private void OnTimeOfDaySelection(SelectorVM<TimeOfDayItemVM> selector)
		{
			this.SelectedTimeOfDay = selector.SelectedItem.TimeOfDay;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009BEC File Offset: 0x00007DEC
		public void OnGameTypeChange(string gameTypeStringId)
		{
			this.MapSelection.ItemList.Clear();
			if (gameTypeStringId == "Battle")
			{
				this.IsCurrentMapSiege = false;
				this._availableMaps = this._battleMaps;
			}
			else if (gameTypeStringId == "Village")
			{
				this.IsCurrentMapSiege = false;
				this._availableMaps = this._villageMaps;
			}
			else if (gameTypeStringId == "Siege")
			{
				this.IsCurrentMapSiege = true;
				this._availableMaps = this._siegeMaps;
			}
			foreach (MapItemVM mapItemVM in this._availableMaps)
			{
				this.MapSelection.AddItem(mapItemVM);
			}
			this.MapSelection.SelectedIndex = 0;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009CC4 File Offset: 0x00007EC4
		public void RandomizeAll()
		{
			this.MapSelection.ExecuteRandomize();
			this.SceneLevelSelection.ExecuteRandomize();
			this.SeasonSelection.ExecuteRandomize();
			this.WallHitpointSelection.ExecuteRandomize();
			this.TimeOfDaySelection.ExecuteRandomize();
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00009CFD File Offset: 0x00007EFD
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00009D05 File Offset: 0x00007F05
		[DataSourceProperty]
		public SelectorVM<MapItemVM> MapSelection
		{
			get
			{
				return this._mapSelection;
			}
			set
			{
				if (value != this._mapSelection)
				{
					this._mapSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<MapItemVM>>(value, "MapSelection");
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00009D23 File Offset: 0x00007F23
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00009D2B File Offset: 0x00007F2B
		[DataSourceProperty]
		public SelectorVM<SceneLevelItemVM> SceneLevelSelection
		{
			get
			{
				return this._sceneLevelSelection;
			}
			set
			{
				if (value != this._sceneLevelSelection)
				{
					this._sceneLevelSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<SceneLevelItemVM>>(value, "SceneLevelSelection");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00009D49 File Offset: 0x00007F49
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00009D51 File Offset: 0x00007F51
		[DataSourceProperty]
		public SelectorVM<WallHitpointItemVM> WallHitpointSelection
		{
			get
			{
				return this._wallHitpointSelection;
			}
			set
			{
				if (value != this._wallHitpointSelection)
				{
					this._wallHitpointSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<WallHitpointItemVM>>(value, "WallHitpointSelection");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00009D6F File Offset: 0x00007F6F
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00009D77 File Offset: 0x00007F77
		[DataSourceProperty]
		public SelectorVM<SeasonItemVM> SeasonSelection
		{
			get
			{
				return this._seasonSelection;
			}
			set
			{
				if (value != this._seasonSelection)
				{
					this._seasonSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<SeasonItemVM>>(value, "SeasonSelection");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00009D95 File Offset: 0x00007F95
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00009D9D File Offset: 0x00007F9D
		[DataSourceProperty]
		public SelectorVM<TimeOfDayItemVM> TimeOfDaySelection
		{
			get
			{
				return this._timeOfDaySelection;
			}
			set
			{
				if (value != this._timeOfDaySelection)
				{
					this._timeOfDaySelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<TimeOfDayItemVM>>(value, "TimeOfDaySelection");
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00009DBB File Offset: 0x00007FBB
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00009DC3 File Offset: 0x00007FC3
		[DataSourceProperty]
		public bool IsCurrentMapSiege
		{
			get
			{
				return this._isCurrentMapSiege;
			}
			set
			{
				if (value != this._isCurrentMapSiege)
				{
					this._isCurrentMapSiege = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentMapSiege");
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00009DE1 File Offset: 0x00007FE1
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00009DE9 File Offset: 0x00007FE9
		[DataSourceProperty]
		public bool IsSallyOutSelected
		{
			get
			{
				return this._isSallyOutSelected;
			}
			set
			{
				if (value != this._isSallyOutSelected)
				{
					this._isSallyOutSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSallyOutSelected");
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00009E07 File Offset: 0x00008007
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00009E0F File Offset: 0x0000800F
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00009E32 File Offset: 0x00008032
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00009E3A File Offset: 0x0000803A
		[DataSourceProperty]
		public string MapText
		{
			get
			{
				return this._mapText;
			}
			set
			{
				if (value != this._mapText)
				{
					this._mapText = value;
					base.OnPropertyChangedWithValue<string>(value, "MapText");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00009E5D File Offset: 0x0000805D
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00009E65 File Offset: 0x00008065
		[DataSourceProperty]
		public string SeasonText
		{
			get
			{
				return this._seasonText;
			}
			set
			{
				if (value != this._seasonText)
				{
					this._seasonText = value;
					base.OnPropertyChangedWithValue<string>(value, "SeasonText");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00009E88 File Offset: 0x00008088
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00009E90 File Offset: 0x00008090
		[DataSourceProperty]
		public string TimeOfDayText
		{
			get
			{
				return this._timeOfDayText;
			}
			set
			{
				if (value != this._timeOfDayText)
				{
					this._timeOfDayText = value;
					base.OnPropertyChangedWithValue<string>(value, "TimeOfDayText");
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00009EB3 File Offset: 0x000080B3
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00009EBB File Offset: 0x000080BB
		[DataSourceProperty]
		public string SceneLevelText
		{
			get
			{
				return this._sceneLevelText;
			}
			set
			{
				if (value != this._sceneLevelText)
				{
					this._sceneLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SceneLevelText");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00009EDE File Offset: 0x000080DE
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00009EE6 File Offset: 0x000080E6
		[DataSourceProperty]
		public string WallHitpointsText
		{
			get
			{
				return this._wallHitpointsText;
			}
			set
			{
				if (value != this._wallHitpointsText)
				{
					this._wallHitpointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WallHitpointsText");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00009F09 File Offset: 0x00008109
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00009F11 File Offset: 0x00008111
		[DataSourceProperty]
		public string AttackerSiegeMachinesText
		{
			get
			{
				return this._attackerSiegeMachinesText;
			}
			set
			{
				if (value != this._attackerSiegeMachinesText)
				{
					this._attackerSiegeMachinesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerSiegeMachinesText");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009F34 File Offset: 0x00008134
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00009F3C File Offset: 0x0000813C
		[DataSourceProperty]
		public string DefenderSiegeMachinesText
		{
			get
			{
				return this._defenderSiegeMachinesText;
			}
			set
			{
				if (value != this._defenderSiegeMachinesText)
				{
					this._defenderSiegeMachinesText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderSiegeMachinesText");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00009F5F File Offset: 0x0000815F
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00009F67 File Offset: 0x00008167
		[DataSourceProperty]
		public string SalloutText
		{
			get
			{
				return this._salloutText;
			}
			set
			{
				if (value != this._salloutText)
				{
					this._salloutText = value;
					base.OnPropertyChangedWithValue<string>(value, "SalloutText");
				}
			}
		}

		// Token: 0x040000E7 RID: 231
		private bool _isCurrentMapSiege;

		// Token: 0x040000E8 RID: 232
		private bool _isSallyOutSelected;

		// Token: 0x040000E9 RID: 233
		private SelectorVM<MapItemVM> _mapSelection;

		// Token: 0x040000EA RID: 234
		private SelectorVM<SceneLevelItemVM> _sceneLevelSelection;

		// Token: 0x040000EB RID: 235
		private SelectorVM<WallHitpointItemVM> _wallHitpointSelection;

		// Token: 0x040000EC RID: 236
		private SelectorVM<SeasonItemVM> _seasonSelection;

		// Token: 0x040000ED RID: 237
		private SelectorVM<TimeOfDayItemVM> _timeOfDaySelection;

		// Token: 0x040000EE RID: 238
		private string _titleText;

		// Token: 0x040000EF RID: 239
		private string _mapText;

		// Token: 0x040000F0 RID: 240
		private string _seasonText;

		// Token: 0x040000F1 RID: 241
		private string _timeOfDayText;

		// Token: 0x040000F2 RID: 242
		private string _sceneLevelText;

		// Token: 0x040000F3 RID: 243
		private string _wallHitpointsText;

		// Token: 0x040000F4 RID: 244
		private string _attackerSiegeMachinesText;

		// Token: 0x040000F5 RID: 245
		private string _defenderSiegeMachinesText;

		// Token: 0x040000F6 RID: 246
		private string _salloutText;
	}
}
