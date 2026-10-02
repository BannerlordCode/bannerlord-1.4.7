using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x02000078 RID: 120
	public class KingdomDecisionsVM : ViewModel
	{
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0002ABA1 File Offset: 0x00028DA1
		public bool IsCurrentDecisionActive
		{
			get
			{
				DecisionItemBaseVM currentDecision = this.CurrentDecision;
				return currentDecision != null && currentDecision.IsActive;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002ABB4 File Offset: 0x00028DB4
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0002ABBC File Offset: 0x00028DBC
		private bool _shouldCheckForDecision { get; set; } = true;

		// Token: 0x060009C9 RID: 2505 RVA: 0x0002ABC8 File Offset: 0x00028DC8
		public KingdomDecisionsVM(Action refreshKingdomManagement)
		{
			this._refreshKingdomManagement = refreshKingdomManagement;
			this._examinedDecisionsSinceInit = new List<KingdomDecision>();
			this._examinedDecisionsSinceInit.AddRange(Clan.PlayerClan.Kingdom.UnresolvedDecisions.Where<KingdomDecision>((KingdomDecision d) => d.ShouldBeCancelled()));
			this._solvedDecisionsSinceInit = new List<KingdomDecision>();
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			this.IsRefreshed = true;
			this.RefreshValues();
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002AC61 File Offset: 0x00028E61
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = GameTexts.FindText("str_kingdom_decisions", null).ToString();
			DecisionItemBaseVM currentDecision = this.CurrentDecision;
			if (currentDecision == null)
			{
				return;
			}
			currentDecision.RefreshValues();
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0002AC90 File Offset: 0x00028E90
		public void OnFrameTick()
		{
			this.IsActive = this.IsCurrentDecisionActive;
			IEnumerable<KingdomDecision> enumerable = Clan.PlayerClan.Kingdom.UnresolvedDecisions.Except<KingdomDecision>(this._examinedDecisionsSinceInit);
			if (this._shouldCheckForDecision)
			{
				if (this.CurrentDecision != null)
				{
					DecisionItemBaseVM currentDecision = this.CurrentDecision;
					if (currentDecision == null || currentDecision.IsActive)
					{
						return;
					}
				}
				if (enumerable.Any<KingdomDecision>())
				{
					KingdomDecision kingdomDecision = this._solvedDecisionsSinceInit.LastOrDefault<KingdomDecision>();
					KingdomDecision kingdomDecision2 = ((kingdomDecision != null) ? kingdomDecision.GetFollowUpDecision() : null);
					if (kingdomDecision2 != null)
					{
						this.HandleDecision(kingdomDecision2);
						return;
					}
					this.HandleNextDecision();
				}
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002AD1C File Offset: 0x00028F1C
		public void HandleNextDecision()
		{
			this.HandleDecision(Clan.PlayerClan.Kingdom.UnresolvedDecisions.Except<KingdomDecision>(this._examinedDecisionsSinceInit).FirstOrDefault<KingdomDecision>());
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002AD44 File Offset: 0x00028F44
		public void HandleDecision(KingdomDecision curDecision)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				this._shouldCheckForDecision = false;
				return;
			}
			KingdomDecision curDecision2 = curDecision;
			if (curDecision2 != null && !curDecision2.ShouldBeCancelled())
			{
				this._shouldCheckForDecision = false;
				this._examinedDecisionsSinceInit.Add(curDecision);
				if (curDecision.IsPlayerParticipant)
				{
					TextObject generalTitle = new KingdomElection(curDecision).GetGeneralTitle();
					GameTexts.SetVariable("DECISION_NAME", generalTitle.ToString());
					string text = (curDecision.NeedsPlayerResolution ? GameTexts.FindText("str_you_need_to_resolve_decision", null).ToString() : GameTexts.FindText("str_do_you_want_to_resolve_decision", null).ToString());
					if (!curDecision.NeedsPlayerResolution && curDecision.TriggerTime.IsFuture)
					{
						GameTexts.SetVariable("HOUR", ((int)curDecision.TriggerTime.RemainingHoursFromNow).ToString());
						GameTexts.SetVariable("newline", "\n");
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", GameTexts.FindText("str_decision_will_be_resolved_in_hours", null));
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
					}
					this._queryData = new InquiryData(GameTexts.FindText("str_decision", null).ToString(), text, true, !curDecision.NeedsPlayerResolution, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), delegate
					{
						this.RefreshWith(curDecision);
					}, delegate
					{
						this._shouldCheckForDecision = true;
					}, "", 0f, null, null, null);
					this._shouldCheckForDecision = false;
					InformationManager.ShowInquiry(this._queryData, false, false);
					return;
				}
			}
			else
			{
				this._shouldCheckForDecision = false;
				this._queryData = null;
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002AF28 File Offset: 0x00029128
		public void RefreshWith(KingdomDecision decision)
		{
			if (decision.IsSingleClanDecision())
			{
				KingdomElection kingdomElection = new KingdomElection(decision);
				kingdomElection.StartElection();
				kingdomElection.ApplySelection();
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision_outcome", null).ToString(), kingdomElection.GetChosenOutcomeText().ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", delegate
				{
					this.OnSingleDecisionOver();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			this._shouldCheckForDecision = false;
			this.CurrentDecision = this.GetDecisionItemBasedOnType(decision);
			this.CurrentDecision.SetDoneInputKey(this.DoneInputKey);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002AFCE File Offset: 0x000291CE
		private void OnSingleDecisionOver()
		{
			this._refreshKingdomManagement();
			this._shouldCheckForDecision = true;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002AFE2 File Offset: 0x000291E2
		private void OnDecisionOver()
		{
			this._refreshKingdomManagement();
			DecisionItemBaseVM currentDecision = this.CurrentDecision;
			if (currentDecision != null)
			{
				currentDecision.OnFinalize();
			}
			this.CurrentDecision = null;
			this._shouldCheckForDecision = true;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002B00E File Offset: 0x0002920E
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome outcome, bool isPlayerInvolved)
		{
			if (isPlayerInvolved)
			{
				this._solvedDecisionsSinceInit.Add(decision);
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002B020 File Offset: 0x00029220
		private DecisionItemBaseVM GetDecisionItemBasedOnType(KingdomDecision decision)
		{
			SettlementClaimantDecision settlementClaimantDecision;
			if ((settlementClaimantDecision = decision as SettlementClaimantDecision) != null)
			{
				return new SettlementDecisionItemVM(settlementClaimantDecision.Settlement, decision, new Action(this.OnDecisionOver));
			}
			SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
			if ((settlementClaimantPreliminaryDecision = decision as SettlementClaimantPreliminaryDecision) != null)
			{
				return new SettlementDecisionItemVM(settlementClaimantPreliminaryDecision.Settlement, decision, new Action(this.OnDecisionOver));
			}
			ExpelClanFromKingdomDecision expelClanFromKingdomDecision;
			if ((expelClanFromKingdomDecision = decision as ExpelClanFromKingdomDecision) != null)
			{
				return new ExpelClanDecisionItemVM(expelClanFromKingdomDecision, new Action(this.OnDecisionOver));
			}
			KingdomPolicyDecision kingdomPolicyDecision;
			if ((kingdomPolicyDecision = decision as KingdomPolicyDecision) != null)
			{
				return new PolicyDecisionItemVM(kingdomPolicyDecision, new Action(this.OnDecisionOver));
			}
			DeclareWarDecision declareWarDecision;
			if ((declareWarDecision = decision as DeclareWarDecision) != null)
			{
				return new DeclareWarDecisionItemVM(declareWarDecision, new Action(this.OnDecisionOver));
			}
			MakePeaceKingdomDecision makePeaceKingdomDecision;
			if ((makePeaceKingdomDecision = decision as MakePeaceKingdomDecision) != null)
			{
				return new MakePeaceDecisionItemVM(makePeaceKingdomDecision, new Action(this.OnDecisionOver));
			}
			KingSelectionKingdomDecision kingSelectionKingdomDecision;
			if ((kingSelectionKingdomDecision = decision as KingSelectionKingdomDecision) != null)
			{
				return new KingSelectionDecisionItemVM(kingSelectionKingdomDecision, new Action(this.OnDecisionOver));
			}
			StartAllianceDecision startAllianceDecision;
			if ((startAllianceDecision = decision as StartAllianceDecision) != null)
			{
				return new StartAllianceDecisionItemVM(startAllianceDecision, new Action(this.OnDecisionOver));
			}
			ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision;
			if ((proposeCallToWarAgreementDecision = decision as ProposeCallToWarAgreementDecision) != null)
			{
				return new ProposeCallToWarAgreementDecisionItemVM(proposeCallToWarAgreementDecision, new Action(this.OnDecisionOver));
			}
			AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision;
			if ((acceptCallToWarAgreementDecision = decision as AcceptCallToWarAgreementDecision) != null)
			{
				return new AcceptingCallToWarAgreementDecisionItemVM(acceptCallToWarAgreementDecision, new Action(this.OnDecisionOver));
			}
			TradeAgreementDecision tradeAgreementDecision;
			if ((tradeAgreementDecision = decision as TradeAgreementDecision) != null)
			{
				return new TradeAgreementDecisionItemVM(tradeAgreementDecision, new Action(this.OnDecisionOver));
			}
			Debug.FailedAssert("No defined decision type for this decision! This shouldn't happen", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\Decisions\\KingdomDecisionsVM.cs", "GetDecisionItemBasedOnType", 215);
			return new DecisionItemBaseVM(decision, new Action(this.OnDecisionOver));
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002B1B1 File Offset: 0x000293B1
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
			DecisionItemBaseVM currentDecision = this.CurrentDecision;
			if (currentDecision != null)
			{
				currentDecision.OnFinalize();
			}
			this.CurrentDecision = null;
			CampaignEvents.KingdomDecisionConcluded.ClearListeners(this);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002B1E7 File Offset: 0x000293E7
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0002B1F6 File Offset: 0x000293F6
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0002B1FE File Offset: 0x000293FE
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0002B21C File Offset: 0x0002941C
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x0002B224 File Offset: 0x00029424
		[DataSourceProperty]
		public DecisionItemBaseVM CurrentDecision
		{
			get
			{
				return this._currentDecision;
			}
			set
			{
				if (value != this._currentDecision)
				{
					this._currentDecision = value;
					base.OnPropertyChangedWithValue<DecisionItemBaseVM>(value, "CurrentDecision");
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0002B242 File Offset: 0x00029442
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x0002B24A File Offset: 0x0002944A
		[DataSourceProperty]
		public int NotificationCount
		{
			get
			{
				return this._notificationCount;
			}
			set
			{
				if (value != this._notificationCount)
				{
					this._notificationCount = value;
					base.OnPropertyChangedWithValue(value, "NotificationCount");
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0002B268 File Offset: 0x00029468
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x0002B270 File Offset: 0x00029470
		[DataSourceProperty]
		public bool IsRefreshed
		{
			get
			{
				return this._isRefreshed;
			}
			set
			{
				if (value != this._isRefreshed)
				{
					this._isRefreshed = value;
					base.OnPropertyChangedWithValue(value, "IsRefreshed");
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0002B28E File Offset: 0x0002948E
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x0002B296 File Offset: 0x00029496
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0002B2B4 File Offset: 0x000294B4
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0002B2BC File Offset: 0x000294BC
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

		// Token: 0x04000459 RID: 1113
		private List<KingdomDecision> _examinedDecisionsSinceInit;

		// Token: 0x0400045A RID: 1114
		private List<KingdomDecision> _solvedDecisionsSinceInit;

		// Token: 0x0400045B RID: 1115
		private readonly Action _refreshKingdomManagement;

		// Token: 0x0400045D RID: 1117
		private InquiryData _queryData;

		// Token: 0x0400045E RID: 1118
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400045F RID: 1119
		private bool _isRefreshed;

		// Token: 0x04000460 RID: 1120
		private bool _isActive;

		// Token: 0x04000461 RID: 1121
		private int _notificationCount;

		// Token: 0x04000462 RID: 1122
		private string _titleText;

		// Token: 0x04000463 RID: 1123
		private DecisionItemBaseVM _currentDecision;
	}
}
