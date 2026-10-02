using System;
using Helpers;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000072 RID: 114
	public class KingdomWarItemVM : KingdomDiplomacyItemVM
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x00029814 File Offset: 0x00027A14
		public KingdomWarItemVM(StanceLink war, Action<KingdomWarItemVM> onSelect)
			: base(war.Faction1, war.Faction2)
		{
			this._war = war;
			this._onSelect = onSelect;
			this.IsBehaviorSelectionEnabled = this.Faction1.IsKingdomFaction && this.Faction1.Leader == Hero.MainHero;
			StanceLink stanceWith = this.Faction1.GetStanceWith(this.Faction2);
			this._warProgressOfFaction1 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(this.Faction1, this.Faction2, true);
			this._warProgressOfFaction2 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(this.Faction2, this.Faction1, true);
			this._numberOfTownsCapturedByFaction1 = stanceWith.GetSuccessfulTownSieges(this.Faction1);
			this._numberOfTownsCapturedByFaction2 = stanceWith.GetSuccessfulTownSieges(this.Faction2);
			this._numberOfCastlesCapturedByFaction1 = stanceWith.GetSuccessfulSieges(this.Faction1) - this._numberOfTownsCapturedByFaction1;
			this._numberOfCastlesCapturedByFaction2 = stanceWith.GetSuccessfulSieges(this.Faction2) - this._numberOfTownsCapturedByFaction2;
			this._numberOfRaidsMadeByFaction1 = stanceWith.GetSuccessfulRaids(this.Faction1);
			this._numberOfRaidsMadeByFaction2 = stanceWith.GetSuccessfulRaids(this.Faction2);
			this.RefreshValues();
			this.WarLog = new MBBindingList<KingdomWarLogItemVM>();
			foreach (ValueTuple<LogEntry, IFaction, IFaction> valueTuple in DiplomacyHelper.GetLogsForWar(war))
			{
				LogEntry item = valueTuple.Item1;
				IFaction item2 = valueTuple.Item2;
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = item as IEncyclopediaLog) != null)
				{
					this.WarLog.Add(new KingdomWarLogItemVM(encyclopediaLog, item2));
				}
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000299C0 File Offset: 0x00027BC0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateDiplomacyProperties();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000299CE File Offset: 0x00027BCE
		protected override void OnSelect()
		{
			if (base.IsSelected)
			{
				return;
			}
			this.UpdateDiplomacyProperties();
			this._onSelect(this);
			base.IsSelected = true;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000299F4 File Offset: 0x00027BF4
		protected override void UpdateDiplomacyProperties()
		{
			base.UpdateDiplomacyProperties();
			GameTexts.SetVariable("FACTION_1_NAME", this.Faction1.Name.ToString());
			GameTexts.SetVariable("FACTION_2_NAME", this.Faction2.Name.ToString());
			this.WarName = GameTexts.FindText("str_war_faction_versus_faction", null).ToString();
			StanceLink stanceWith = this.Faction1.GetStanceWith(this.Faction2);
			this.Score = stanceWith.GetSuccessfulSieges(this.Faction1) + stanceWith.GetSuccessfulRaids(this.Faction1);
			this.CasualtiesOfFaction1 = stanceWith.GetCasualties(this.Faction1);
			this.CasualtiesOfFaction2 = stanceWith.GetCasualties(this.Faction2);
			int num = MathF.Ceiling(this._war.WarStartDate.ElapsedDaysUntilNow + 0.01f);
			TextObject textObject = GameTexts.FindText("str_for_DAY_days", null);
			textObject.SetTextVariable("DAY", num.ToString());
			textObject.SetTextVariable("DAY_IS_PLURAL", (num > 1) ? 1 : 0);
			this.NumberOfDaysSinceWarBegan = textObject.ToString();
			base.Stats.Add(new KingdomWarComparableStatVM((int)this.Faction1.CurrentTotalStrength, (int)this.Faction2.CurrentTotalStrength, GameTexts.FindText("str_total_strength", null), this._faction1Color, this._faction2Color, 10000, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(stanceWith.GetCasualties(this.Faction2), stanceWith.GetCasualties(this.Faction1), GameTexts.FindText("str_war_casualties_inflicted", null), this._faction1Color, this._faction2Color, 10000, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._numberOfTownsCapturedByFaction1, this._numberOfTownsCapturedByFaction2, GameTexts.FindText("str_war_captured_towns", null), this._faction1Color, this._faction2Color, 25, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._numberOfCastlesCapturedByFaction1, this._numberOfCastlesCapturedByFaction2, GameTexts.FindText("str_war_captured_castles", null), this._faction1Color, this._faction2Color, 25, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._numberOfRaidsMadeByFaction1, this._numberOfRaidsMadeByFaction2, GameTexts.FindText("str_war_successful_raids", null), this._faction1Color, this._faction2Color, 10, null, null));
			int num2 = (int)(this._warProgressOfFaction1.ResultNumber * 100f / this._warProgressOfFaction1.LimitMaxValue);
			int num3 = (int)(this._warProgressOfFaction2.ResultNumber * 100f / this._warProgressOfFaction2.LimitMaxValue);
			int num4 = MathF.Max(0, num2 - num3);
			int num5 = MathF.Max(0, num3 - num2);
			base.Stats.Add(new KingdomWarComparableStatVM(num4, num5, new TextObject("{=8qbkS5D2}War Progress", null), this._faction1Color, this._faction2Color, 100, new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(this._warProgressOfFaction1, this._warProgressOfFaction2, this._warProgressOfFaction1.LimitMaxValue, this.Faction1.Name, this.Faction2.Name)), new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(this._warProgressOfFaction2, this._warProgressOfFaction1, this._warProgressOfFaction2.LimitMaxValue, this.Faction2.Name, this.Faction1.Name))));
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x00029CDC File Offset: 0x00027EDC
		// (set) Token: 0x0600095A RID: 2394 RVA: 0x00029CE4 File Offset: 0x00027EE4
		[DataSourceProperty]
		public string WarName
		{
			get
			{
				return this._warName;
			}
			set
			{
				if (value != this._warName)
				{
					this._warName = value;
					base.OnPropertyChangedWithValue<string>(value, "WarName");
				}
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x00029D07 File Offset: 0x00027F07
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x00029D0F File Offset: 0x00027F0F
		[DataSourceProperty]
		public string NumberOfDaysSinceWarBegan
		{
			get
			{
				return this._numberOfDaysSinceWarBegan;
			}
			set
			{
				if (value != this._numberOfDaysSinceWarBegan)
				{
					this._numberOfDaysSinceWarBegan = value;
					base.OnPropertyChangedWithValue<string>(value, "NumberOfDaysSinceWarBegan");
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x00029D32 File Offset: 0x00027F32
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x00029D3A File Offset: 0x00027F3A
		[DataSourceProperty]
		public bool IsBehaviorSelectionEnabled
		{
			get
			{
				return this._isBehaviorSelectionEnabled;
			}
			set
			{
				if (value != this._isBehaviorSelectionEnabled)
				{
					this._isBehaviorSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBehaviorSelectionEnabled");
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00029D58 File Offset: 0x00027F58
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x00029D60 File Offset: 0x00027F60
		[DataSourceProperty]
		public int Score
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
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x00029D7E File Offset: 0x00027F7E
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x00029D86 File Offset: 0x00027F86
		[DataSourceProperty]
		public int CasualtiesOfFaction1
		{
			get
			{
				return this._casualtiesOfFaction1;
			}
			set
			{
				if (value != this._casualtiesOfFaction1)
				{
					this._casualtiesOfFaction1 = value;
					base.OnPropertyChangedWithValue(value, "CasualtiesOfFaction1");
				}
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x00029DA4 File Offset: 0x00027FA4
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x00029DAC File Offset: 0x00027FAC
		[DataSourceProperty]
		public int CasualtiesOfFaction2
		{
			get
			{
				return this._casualtiesOfFaction2;
			}
			set
			{
				if (value != this._casualtiesOfFaction2)
				{
					this._casualtiesOfFaction2 = value;
					base.OnPropertyChangedWithValue(value, "CasualtiesOfFaction2");
				}
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x00029DCA File Offset: 0x00027FCA
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x00029DD2 File Offset: 0x00027FD2
		[DataSourceProperty]
		public MBBindingList<KingdomWarLogItemVM> WarLog
		{
			get
			{
				return this._warLog;
			}
			set
			{
				if (value != this._warLog)
				{
					this._warLog = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarLogItemVM>>(value, "WarLog");
				}
			}
		}

		// Token: 0x04000419 RID: 1049
		private readonly Action<KingdomWarItemVM> _onSelect;

		// Token: 0x0400041A RID: 1050
		private readonly StanceLink _war;

		// Token: 0x0400041B RID: 1051
		private ExplainedNumber _warProgressOfFaction1;

		// Token: 0x0400041C RID: 1052
		private ExplainedNumber _warProgressOfFaction2;

		// Token: 0x0400041D RID: 1053
		private int _numberOfTownsCapturedByFaction1;

		// Token: 0x0400041E RID: 1054
		private int _numberOfTownsCapturedByFaction2;

		// Token: 0x0400041F RID: 1055
		private int _numberOfCastlesCapturedByFaction1;

		// Token: 0x04000420 RID: 1056
		private int _numberOfCastlesCapturedByFaction2;

		// Token: 0x04000421 RID: 1057
		private int _numberOfRaidsMadeByFaction1;

		// Token: 0x04000422 RID: 1058
		private int _numberOfRaidsMadeByFaction2;

		// Token: 0x04000423 RID: 1059
		private string _warName;

		// Token: 0x04000424 RID: 1060
		private string _numberOfDaysSinceWarBegan;

		// Token: 0x04000425 RID: 1061
		private int _score;

		// Token: 0x04000426 RID: 1062
		private bool _isBehaviorSelectionEnabled;

		// Token: 0x04000427 RID: 1063
		private int _casualtiesOfFaction1;

		// Token: 0x04000428 RID: 1064
		private int _casualtiesOfFaction2;

		// Token: 0x04000429 RID: 1065
		private MBBindingList<KingdomWarLogItemVM> _warLog;
	}
}
