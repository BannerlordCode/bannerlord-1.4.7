using System;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000070 RID: 112
	public class KingdomTruceItemVM : KingdomDiplomacyItemVM
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x0002912D File Offset: 0x0002732D
		public KingdomTruceItemVM(IFaction faction1, IFaction faction2, Action<KingdomDiplomacyItemVM> onSelection)
			: base(faction1, faction2)
		{
			this._onSelection = onSelection;
			this.UpdateDiplomacyProperties();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00029144 File Offset: 0x00027344
		protected override void OnSelect()
		{
			if (base.IsSelected)
			{
				return;
			}
			this.UpdateDiplomacyProperties();
			this._onSelection(this);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00029164 File Offset: 0x00027364
		protected override void UpdateDiplomacyProperties()
		{
			base.UpdateDiplomacyProperties();
			base.Stats.Add(new KingdomWarComparableStatVM((int)this.Faction1.CurrentTotalStrength, (int)this.Faction2.CurrentTotalStrength, GameTexts.FindText("str_total_strength", null), this._faction1Color, this._faction2Color, 10000, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._faction1Towns.Count, this._faction2Towns.Count, GameTexts.FindText("str_towns", null), this._faction1Color, this._faction2Color, 25, new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction1Towns, this.Faction1.Name, true)), new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction2Towns, this.Faction2.Name, true))));
			base.Stats.Add(new KingdomWarComparableStatVM(this._faction1Castles.Count, this._faction2Castles.Count, GameTexts.FindText("str_castles", null), this._faction1Color, this._faction2Color, 25, new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction1Castles, this.Faction1.Name, false)), new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction2Castles, this.Faction2.Name, false))));
			StanceLink stanceWith = this._playerKingdom.GetStanceWith(this.Faction2);
			this.TributePaid = stanceWith.GetDailyTributeToPay(this._playerKingdom);
			if (stanceWith.IsNeutral && this.TributePaid != 0)
			{
				base.Stats.Add(new KingdomWarComparableStatVM(MathF.Max(stanceWith.GetTotalTributePaid(this.Faction2), 0), MathF.Max(stanceWith.GetTotalTributePaid(this.Faction1), 0), GameTexts.FindText("str_comparison_tribute_received", null), this._faction1Color, this._faction2Color, 10000, null, null));
			}
			if (!this.Faction1.IsKingdomFaction || !this.Faction2.IsKingdomFaction)
			{
				this.HasTradeAgreement = false;
				this.HasAlliance = false;
				this.TradeAgreementEndTimeStr = null;
				this.AllianceEndTimeStr = null;
				return;
			}
			ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement = default(TradeAgreementsCampaignBehavior.TradeAgreement);
			this.HasTradeAgreement = campaignBehavior != null && campaignBehavior.HasTradeAgreement(this.Faction1 as Kingdom, this.Faction2 as Kingdom, out tradeAgreement);
			this.HasAlliance = DiplomacyHelper.HasAllianceWithFaction(this.Faction1, this.Faction2);
			if (this.HasTradeAgreement)
			{
				int num = MathF.Ceiling(tradeAgreement.EndTime.RemainingDaysFromNow);
				this.TradeAgreementEndTimeStr = new TextObject("{=6ayEZQE1}Expires in {DAYS} {?DAYS > 1}days{?}day{\\?}.", null).SetTextVariable("DAYS", num.ToString()).ToString();
				int kingdom1GoldGainedTotal = tradeAgreement.Kingdom1GoldGainedTotal;
				int kingdom2GoldGainedTotal = tradeAgreement.Kingdom2GoldGainedTotal;
				if (kingdom1GoldGainedTotal > 0 || kingdom2GoldGainedTotal > 0)
				{
					base.Stats.Add(new KingdomWarComparableStatVM(MathF.Max((tradeAgreement.Kingdom1 == this.Faction1) ? kingdom1GoldGainedTotal : kingdom2GoldGainedTotal, 0), MathF.Max((tradeAgreement.Kingdom1 == this.Faction2) ? kingdom1GoldGainedTotal : kingdom2GoldGainedTotal, 0), GameTexts.FindText("str_comparison_trade_gold_gained", null), this._faction1Color, this._faction2Color, 10000, null, null));
				}
			}
			else
			{
				this.TradeAgreementEndTimeStr = null;
			}
			IAllianceCampaignBehavior campaignBehavior2 = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
			if (this.HasAlliance && campaignBehavior2 != null)
			{
				int num2 = MathF.Ceiling(campaignBehavior2.GetAllianceEndDate(this.Faction1 as Kingdom, this.Faction2 as Kingdom).RemainingDaysFromNow);
				this.AllianceEndTimeStr = new TextObject("{=6ayEZQE1}Expires in {DAYS} {?DAYS > 1}days{?}day{\\?}.", null).SetTextVariable("DAYS", num2.ToString()).ToString();
				return;
			}
			this.AllianceEndTimeStr = null;
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x000294D4 File Offset: 0x000276D4
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x000294DC File Offset: 0x000276DC
		[DataSourceProperty]
		public int TributePaid
		{
			get
			{
				return this._tributePaid;
			}
			set
			{
				if (value != this._tributePaid)
				{
					this._tributePaid = value;
					base.OnPropertyChangedWithValue(value, "TributePaid");
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x000294FA File Offset: 0x000276FA
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x00029502 File Offset: 0x00027702
		[DataSourceProperty]
		public bool HasTradeAgreement
		{
			get
			{
				return this._hasTradeAgreement;
			}
			set
			{
				if (value != this._hasTradeAgreement)
				{
					this._hasTradeAgreement = value;
					base.OnPropertyChangedWithValue(value, "HasTradeAgreement");
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00029520 File Offset: 0x00027720
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00029528 File Offset: 0x00027728
		[DataSourceProperty]
		public bool HasAlliance
		{
			get
			{
				return this._hasAlliance;
			}
			set
			{
				if (value != this._hasAlliance)
				{
					this._hasAlliance = value;
					base.OnPropertyChangedWithValue(value, "HasAlliance");
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00029546 File Offset: 0x00027746
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x0002954E File Offset: 0x0002774E
		[DataSourceProperty]
		public string AllianceEndTimeStr
		{
			get
			{
				return this._allianceEndTimeStr;
			}
			set
			{
				if (value != this._allianceEndTimeStr)
				{
					this._allianceEndTimeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "AllianceEndTimeStr");
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00029571 File Offset: 0x00027771
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x00029579 File Offset: 0x00027779
		[DataSourceProperty]
		public string TradeAgreementEndTimeStr
		{
			get
			{
				return this._tradeAgreementEndTimeStr;
			}
			set
			{
				if (value != this._tradeAgreementEndTimeStr)
				{
					this._tradeAgreementEndTimeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "TradeAgreementEndTimeStr");
				}
			}
		}

		// Token: 0x04000408 RID: 1032
		private readonly Action<KingdomDiplomacyItemVM> _onSelection;

		// Token: 0x04000409 RID: 1033
		private int _tributePaid;

		// Token: 0x0400040A RID: 1034
		private bool _hasTradeAgreement;

		// Token: 0x0400040B RID: 1035
		private bool _hasAlliance;

		// Token: 0x0400040C RID: 1036
		private string _tradeAgreementEndTimeStr;

		// Token: 0x0400040D RID: 1037
		private string _allianceEndTimeStr;
	}
}
