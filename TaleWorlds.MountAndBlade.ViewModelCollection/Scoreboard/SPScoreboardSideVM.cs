using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000012 RID: 18
	public class SPScoreboardSideVM : ViewModel
	{
		// Token: 0x06000150 RID: 336 RVA: 0x00005754 File Offset: 0x00003954
		public SPScoreboardSideVM(TextObject name, Banner sideFlag, bool isSimulation, bool isPlayerSide)
		{
			SPScoreboardSideVM <>4__this = this;
			this.Parties = new MBBindingList<SPScoreboardPartyVM>();
			this.Ships = new MBBindingList<SPScoreboardShipVM>();
			this.Score = new SPScoreboardStatsVM(name);
			this.IsPlayerSide = isPlayerSide;
			MBBindingList<SPScoreboardPartyVM> parties = this.Parties;
			this.SortController = new SPScoreboardSortControllerVM(ref parties);
			this.Parties = parties;
			if (sideFlag != null)
			{
				this.BannerVisual = new BannerImageIdentifierVM(sideFlag, true);
				this.BannerVisualSmall = new BannerImageIdentifierVM(sideFlag, false);
			}
			this.MoraleHint = new BasicTooltipViewModel(() => <>4__this.GetMoraleHintStr(isSimulation));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000057F4 File Offset: 0x000039F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Score.RefreshValues();
			this.Parties.ApplyActionOnAllItems(delegate(SPScoreboardPartyVM x)
			{
				x.RefreshValues();
			});
			this.Ships.ApplyActionOnAllItems(delegate(SPScoreboardShipVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005868 File Offset: 0x00003A68
		private string GetMoraleHintStr(bool isSimulation)
		{
			return GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).SetTextVariable("LEFT", isSimulation ? GameTexts.FindText("str_morale", null).ToString() : new TextObject("{=trPyg7mr}Battle Morale", null).ToString()).SetTextVariable("RIGHT", MathF.Round(this.Morale).ToString())
				.ToString();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000058D1 File Offset: 0x00003AD1
		public void UpdateScores(IBattleCombatant battleCombatant, bool isPlayerParty, BasicCharacterObject character, int numberRemaining, int numberDead, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
		{
			this.GetPartyAddIfNotExists(battleCombatant, isPlayerParty).UpdateScores(character, numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			this.Score.UpdateScores(numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			this.RefreshPower();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000590A File Offset: 0x00003B0A
		public void UpdateHeroSkills(IBattleCombatant battleCombatant, bool isPlayerParty, BasicCharacterObject heroCharacter, SkillObject upgradedSkill)
		{
			this.GetPartyAddIfNotExists(battleCombatant, isPlayerParty).UpdateHeroSkills(heroCharacter, upgradedSkill);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000591C File Offset: 0x00003B1C
		public SPScoreboardPartyVM GetPartyAddIfNotExists(IBattleCombatant battleCombatant, bool isPlayerParty)
		{
			SPScoreboardPartyVM spscoreboardPartyVM = this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant);
			if (spscoreboardPartyVM == null)
			{
				spscoreboardPartyVM = new SPScoreboardPartyVM(battleCombatant);
				if (isPlayerParty)
				{
					this.Parties.Insert(0, spscoreboardPartyVM);
				}
				else
				{
					this.Parties.Add(spscoreboardPartyVM);
				}
			}
			return spscoreboardPartyVM;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000597C File Offset: 0x00003B7C
		public SPScoreboardPartyVM GetParty(IBattleCombatant battleCombatant)
		{
			return this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000059B0 File Offset: 0x00003BB0
		public SPScoreboardStatsVM RemoveTroop(IBattleCombatant battleCombatant, BasicCharacterObject troop)
		{
			SPScoreboardPartyVM spscoreboardPartyVM = this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant);
			SPScoreboardStatsVM spscoreboardStatsVM = spscoreboardPartyVM.RemoveUnit(troop);
			if (spscoreboardPartyVM.Members.Count == 0)
			{
				this.Parties.Remove(spscoreboardPartyVM);
			}
			this.Score.UpdateScores(-spscoreboardStatsVM.Remaining, -spscoreboardStatsVM.Dead, -spscoreboardStatsVM.Wounded, -spscoreboardStatsVM.Routed, -spscoreboardStatsVM.Kill, -spscoreboardStatsVM.ReadyToUpgrade);
			return spscoreboardStatsVM;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005A3C File Offset: 0x00003C3C
		public void AddTroop(IBattleCombatant battleCombatant, BasicCharacterObject currentTroop, SPScoreboardStatsVM scoreToBringOver)
		{
			this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant).AddUnit(currentTroop, scoreToBringOver);
			this.Score.UpdateScores(scoreToBringOver.Remaining, scoreToBringOver.Dead, scoreToBringOver.Wounded, scoreToBringOver.Routed, scoreToBringOver.Kill, scoreToBringOver.ReadyToUpgrade);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public SPScoreboardShipVM GetShipAddIfNotExists(IShipOrigin ship, string shipType, IBattleCombatant owner, TeamSideEnum teamSideEnum)
		{
			SPScoreboardShipVM spscoreboardShipVM = this.Ships.FirstOrDefault<SPScoreboardShipVM>((SPScoreboardShipVM p) => p.Ship == ship);
			if (spscoreboardShipVM == null)
			{
				spscoreboardShipVM = new SPScoreboardShipVM(ship, shipType, owner, teamSideEnum);
				this.Ships.Add(spscoreboardShipVM);
			}
			return spscoreboardShipVM;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005AF8 File Offset: 0x00003CF8
		private void RefreshPower()
		{
			this.CurrentPower = 0f;
			this.InitialPower = 0f;
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._parties)
			{
				this.InitialPower += spscoreboardPartyVM.InitialPower;
				this.CurrentPower += spscoreboardPartyVM.CurrentPower;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00005B7C File Offset: 0x00003D7C
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00005B84 File Offset: 0x00003D84
		public float CurrentPower { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005B8D File Offset: 0x00003D8D
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005B95 File Offset: 0x00003D95
		public float InitialPower { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005B9E File Offset: 0x00003D9E
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00005BA6 File Offset: 0x00003DA6
		[DataSourceProperty]
		public BannerImageIdentifierVM BannerVisual
		{
			get
			{
				return this._bannerVisual;
			}
			set
			{
				if (value != this._bannerVisual)
				{
					this._bannerVisual = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "BannerVisual");
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005BC4 File Offset: 0x00003DC4
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00005BCC File Offset: 0x00003DCC
		[DataSourceProperty]
		public BannerImageIdentifierVM BannerVisualSmall
		{
			get
			{
				return this._bannerVisualSmall;
			}
			set
			{
				if (value != this._bannerVisualSmall)
				{
					this._bannerVisualSmall = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "BannerVisualSmall");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00005BEA File Offset: 0x00003DEA
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00005BF2 File Offset: 0x00003DF2
		[DataSourceProperty]
		public SPScoreboardStatsVM Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue<SPScoreboardStatsVM>(value, "Score");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00005C10 File Offset: 0x00003E10
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00005C18 File Offset: 0x00003E18
		[DataSourceProperty]
		public MBBindingList<SPScoreboardPartyVM> Parties
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
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardPartyVM>>(value, "Parties");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00005C36 File Offset: 0x00003E36
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00005C3E File Offset: 0x00003E3E
		[DataSourceProperty]
		public MBBindingList<SPScoreboardShipVM> Ships
		{
			get
			{
				return this._ships;
			}
			set
			{
				if (value != this._ships)
				{
					this._ships = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardShipVM>>(value, "Ships");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00005C5C File Offset: 0x00003E5C
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00005C64 File Offset: 0x00003E64
		[DataSourceProperty]
		public SPScoreboardSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChanged("SortController");
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00005C81 File Offset: 0x00003E81
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00005C89 File Offset: 0x00003E89
		[DataSourceProperty]
		public float Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				if (value != this._morale)
				{
					this._morale = value;
					base.OnPropertyChangedWithValue(value, "Morale");
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00005CA7 File Offset: 0x00003EA7
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00005CAF File Offset: 0x00003EAF
		[DataSourceProperty]
		public BasicTooltipViewModel MoraleHint
		{
			get
			{
				return this._moraleHint;
			}
			set
			{
				if (value != this._moraleHint)
				{
					this._moraleHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "MoraleHint");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00005CCD File Offset: 0x00003ECD
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00005CD5 File Offset: 0x00003ED5
		[DataSourceProperty]
		public bool IsPlayerSide
		{
			get
			{
				return this._isPlayerSide;
			}
			set
			{
				if (value != this._isPlayerSide)
				{
					this._isPlayerSide = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerSide");
				}
			}
		}

		// Token: 0x0400009F RID: 159
		private MBBindingList<SPScoreboardPartyVM> _parties;

		// Token: 0x040000A0 RID: 160
		private MBBindingList<SPScoreboardShipVM> _ships;

		// Token: 0x040000A1 RID: 161
		private SPScoreboardStatsVM _score;

		// Token: 0x040000A2 RID: 162
		private BannerImageIdentifierVM _bannerVisual;

		// Token: 0x040000A3 RID: 163
		private BannerImageIdentifierVM _bannerVisualSmall;

		// Token: 0x040000A4 RID: 164
		private SPScoreboardSortControllerVM _sortController;

		// Token: 0x040000A5 RID: 165
		private float _morale;

		// Token: 0x040000A6 RID: 166
		private BasicTooltipViewModel _moraleHint;

		// Token: 0x040000A7 RID: 167
		private bool _isPlayerSide;
	}
}
