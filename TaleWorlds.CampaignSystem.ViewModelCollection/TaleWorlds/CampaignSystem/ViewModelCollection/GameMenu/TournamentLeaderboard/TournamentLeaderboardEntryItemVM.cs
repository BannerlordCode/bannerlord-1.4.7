using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard
{
	// Token: 0x020000AD RID: 173
	public class TournamentLeaderboardEntryItemVM : ViewModel
	{
		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0004406E File Offset: 0x0004226E
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x00044076 File Offset: 0x00042276
		public int Rank { get; private set; }

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0004407F File Offset: 0x0004227F
		// (set) Token: 0x060010C6 RID: 4294 RVA: 0x00044087 File Offset: 0x00042287
		public float PrizeValue { get; private set; }

		// Token: 0x060010C7 RID: 4295 RVA: 0x00044090 File Offset: 0x00042290
		public TournamentLeaderboardEntryItemVM(Hero hero, int victories, int placement)
		{
			this._heroObj = hero;
			this.PrizeStr = "-";
			this.Rank = placement;
			this.PlacementOnLeaderboard = placement;
			this.IsChampion = placement == 1;
			this.Victories = victories;
			float num;
			if (float.TryParse(this.PrizeStr, out num))
			{
				this.PrizeValue = num;
			}
			this.IsMainHero = hero == TaleWorlds.CampaignSystem.Hero.MainHero;
			this.Hero = new HeroVM(hero, false);
			this.ChampionRewardsHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTournamentChampionRewardsTooltip(hero, null));
			this.RefreshValues();
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00044140 File Offset: 0x00042340
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._heroObj.Name.ToString();
			GameTexts.SetVariable("RANK", this.Rank);
			this.RankText = GameTexts.FindText("str_leaderboard_rank", null).ToString();
			HeroVM hero = this.Hero;
			if (hero == null)
			{
				return;
			}
			hero.RefreshValues();
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x0004419F File Offset: 0x0004239F
		// (set) Token: 0x060010CA RID: 4298 RVA: 0x000441A7 File Offset: 0x000423A7
		[DataSourceProperty]
		public BasicTooltipViewModel ChampionRewardsHint
		{
			get
			{
				return this._championRewardsHint;
			}
			set
			{
				if (value != this._championRewardsHint)
				{
					this._championRewardsHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ChampionRewardsHint");
				}
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x000441C5 File Offset: 0x000423C5
		// (set) Token: 0x060010CC RID: 4300 RVA: 0x000441CD File Offset: 0x000423CD
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

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x000441F0 File Offset: 0x000423F0
		// (set) Token: 0x060010CE RID: 4302 RVA: 0x000441F8 File Offset: 0x000423F8
		[DataSourceProperty]
		public string RankText
		{
			get
			{
				return this._rankText;
			}
			set
			{
				if (value != this._rankText)
				{
					this._rankText = value;
					base.OnPropertyChangedWithValue<string>(value, "RankText");
				}
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0004421B File Offset: 0x0004241B
		// (set) Token: 0x060010D0 RID: 4304 RVA: 0x00044223 File Offset: 0x00042423
		[DataSourceProperty]
		public int Victories
		{
			get
			{
				return this._victories;
			}
			set
			{
				if (value != this._victories)
				{
					this._victories = value;
					base.OnPropertyChangedWithValue(value, "Victories");
				}
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x00044241 File Offset: 0x00042441
		// (set) Token: 0x060010D2 RID: 4306 RVA: 0x00044249 File Offset: 0x00042449
		[DataSourceProperty]
		public bool IsChampion
		{
			get
			{
				return this._isChampion;
			}
			set
			{
				if (value != this._isChampion)
				{
					this._isChampion = value;
					base.OnPropertyChangedWithValue(value, "IsChampion");
				}
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x00044267 File Offset: 0x00042467
		// (set) Token: 0x060010D4 RID: 4308 RVA: 0x0004426F File Offset: 0x0004246F
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x0004428D File Offset: 0x0004248D
		// (set) Token: 0x060010D6 RID: 4310 RVA: 0x00044295 File Offset: 0x00042495
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

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x000442B3 File Offset: 0x000424B3
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x000442BB File Offset: 0x000424BB
		[DataSourceProperty]
		public string PrizeStr
		{
			get
			{
				return this._prizeStr;
			}
			set
			{
				if (value != this._prizeStr)
				{
					this._prizeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "PrizeStr");
				}
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x000442DE File Offset: 0x000424DE
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x000442E6 File Offset: 0x000424E6
		[DataSourceProperty]
		public int PlacementOnLeaderboard
		{
			get
			{
				return this._placementOnLeaderboard;
			}
			set
			{
				if (value != this._placementOnLeaderboard)
				{
					this._placementOnLeaderboard = value;
					base.OnPropertyChangedWithValue(value, "PlacementOnLeaderboard");
				}
			}
		}

		// Token: 0x040007A7 RID: 1959
		private readonly Hero _heroObj;

		// Token: 0x040007A8 RID: 1960
		private int _placementOnLeaderboard;

		// Token: 0x040007A9 RID: 1961
		private int _victories;

		// Token: 0x040007AA RID: 1962
		private bool _isMainHero;

		// Token: 0x040007AB RID: 1963
		private bool _isChampion;

		// Token: 0x040007AC RID: 1964
		private string _name;

		// Token: 0x040007AD RID: 1965
		private string _rankText;

		// Token: 0x040007AE RID: 1966
		private string _prizeStr;

		// Token: 0x040007AF RID: 1967
		private HeroVM _hero;

		// Token: 0x040007B0 RID: 1968
		private BasicTooltipViewModel _championRewardsHint;
	}
}
