using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Policies
{
	// Token: 0x0200006A RID: 106
	public class KingdomPoliciesVM : KingdomCategoryVM
	{
		// Token: 0x0600086A RID: 2154 RVA: 0x0002642C File Offset: 0x0002462C
		public KingdomPoliciesVM(Action<KingdomDecision> forceDecide)
		{
			this._forceDecide = forceDecide;
			this.ActivePolicies = new MBBindingList<KingdomPolicyItemVM>();
			this.OtherPolicies = new MBBindingList<KingdomPolicyItemVM>();
			this.DoneHint = new HintViewModel();
			this._playerKingdom = Hero.MainHero.MapFaction as Kingdom;
			this.ProposalAndDisavowalCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(Clan.PlayerClan);
			base.IsAcceptableItemSelected = false;
			this.RefreshValues();
			this.ExecuteSwitchMode();
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000264B8 File Offset: 0x000246B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PoliciesText = GameTexts.FindText("str_policies", null).ToString();
			this.ActivePoliciesText = GameTexts.FindText("str_active_policies", null).ToString();
			this.OtherPoliciesText = GameTexts.FindText("str_other_policies", null).ToString();
			this.ProposeNewPolicyText = GameTexts.FindText("str_propose_new_policy", null).ToString();
			this.DisavowPolicyText = GameTexts.FindText("str_disavow_a_policy", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_policy_selected", null).ToString();
			base.CategoryNameText = new TextObject("{=Sls0KQVn}Elections", null).ToString();
			this.RefreshPolicyList();
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002656C File Offset: 0x0002476C
		public void SelectPolicy(PolicyObject policy)
		{
			bool flag = false;
			foreach (KingdomPolicyItemVM kingdomPolicyItemVM in this.ActivePolicies)
			{
				if (kingdomPolicyItemVM.Policy == policy)
				{
					this.OnPolicySelect(kingdomPolicyItemVM);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (KingdomPolicyItemVM kingdomPolicyItemVM2 in this.OtherPolicies)
				{
					if (kingdomPolicyItemVM2.Policy == policy)
					{
						this.OnPolicySelect(kingdomPolicyItemVM2);
						flag = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00026614 File Offset: 0x00024814
		private void OnPolicySelect(KingdomPolicyItemVM policy)
		{
			if (this.CurrentSelectedPolicy != policy)
			{
				if (this.CurrentSelectedPolicy != null)
				{
					this.CurrentSelectedPolicy.IsSelected = false;
				}
				this.CurrentSelectedPolicy = policy;
				if (this.CurrentSelectedPolicy != null)
				{
					this.CurrentSelectedPolicy.IsSelected = true;
					this._currentSelectedPolicyObject = policy.Policy;
					this._currentItemsUnresolvedDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
					{
						KingdomPolicyDecision kingdomPolicyDecision;
						return (kingdomPolicyDecision = d as KingdomPolicyDecision) != null && kingdomPolicyDecision.Policy == this._currentSelectedPolicyObject && !d.ShouldBeCancelled();
					});
					if (this._currentItemsUnresolvedDecision != null)
					{
						TextObject textObject;
						this.CanProposeOrDisavowPolicy = this.GetCanProposeOrDisavowPolicyWithReason(true, out textObject);
						this.DoneHint.HintText = textObject;
						this.ProposeOrDisavowText = GameTexts.FindText("str_resolve", null).ToString();
						this.ProposeActionExplanationText = GameTexts.FindText("str_resolve_explanation", null).ToString();
						this.PolicyLikelihood = KingdomPoliciesVM.CalculateLikelihood(policy.Policy);
					}
					else
					{
						float influence = Clan.PlayerClan.Influence;
						int proposalAndDisavowalCost = this.ProposalAndDisavowalCost;
						bool isUnderMercenaryService = Clan.PlayerClan.IsUnderMercenaryService;
						TextObject textObject2;
						this.CanProposeOrDisavowPolicy = this.GetCanProposeOrDisavowPolicyWithReason(false, out textObject2);
						this.DoneHint.HintText = textObject2;
						if (this.IsPolicyActive(policy.Policy))
						{
							this.ProposeActionExplanationText = GameTexts.FindText("str_policy_propose_again_action_explanation", null).SetTextVariable("SUPPORT", KingdomPoliciesVM.GetSupportText(policy.Policy)).ToString();
						}
						else
						{
							this.ProposeActionExplanationText = GameTexts.FindText("str_policy_propose_action_explanation", null).SetTextVariable("SUPPORT", KingdomPoliciesVM.GetSupportText(policy.Policy)).ToString();
						}
						this.ProposeOrDisavowText = ((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null).ToString() : GameTexts.FindText("str_policy_enact", null).ToString());
						base.NotificationCount = Clan.PlayerClan.Kingdom.UnresolvedDecisions.Count<KingdomDecision>((KingdomDecision d) => !d.ShouldBeCancelled());
						this.PolicyLikelihood = KingdomPoliciesVM.CalculateLikelihood(policy.Policy);
					}
					GameTexts.SetVariable("NUMBER", this.PolicyLikelihood);
					this.PolicyLikelihoodText = GameTexts.FindText("str_NUMBER_percent", null).ToString();
				}
				base.IsAcceptableItemSelected = this.CurrentSelectedPolicy != null;
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00026850 File Offset: 0x00024A50
		private bool GetCanProposeOrDisavowPolicyWithReason(bool hasUnresolvedDecision, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_mercenaries_cannot_propose_policies", null);
				return false;
			}
			if (!hasUnresolvedDecision && Clan.PlayerClan.Influence < (float)this.ProposalAndDisavowalCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000268B4 File Offset: 0x00024AB4
		public void RefreshPolicyList()
		{
			this.ActivePolicies.Clear();
			this.OtherPolicies.Clear();
			if (this._playerKingdom != null)
			{
				foreach (PolicyObject policyObject in this._playerKingdom.ActivePolicies)
				{
					this.ActivePolicies.Add(new KingdomPolicyItemVM(policyObject, new Action<KingdomPolicyItemVM>(this.OnPolicySelect), new Func<PolicyObject, bool>(this.IsPolicyActive)));
				}
				foreach (PolicyObject policyObject2 in PolicyObject.All.Where<PolicyObject>((PolicyObject p) => !this.IsPolicyActive(p)))
				{
					this.OtherPolicies.Add(new KingdomPolicyItemVM(policyObject2, new Action<KingdomPolicyItemVM>(this.OnPolicySelect), new Func<PolicyObject, bool>(this.IsPolicyActive)));
				}
			}
			GameTexts.SetVariable("STR", this.ActivePolicies.Count);
			this.NumOfActivePoliciesText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			GameTexts.SetVariable("STR", this.OtherPolicies.Count);
			this.NumOfOtherPoliciesText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			this.SetDefaultSelectedPolicy();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00026A14 File Offset: 0x00024C14
		private bool IsPolicyActive(PolicyObject policy)
		{
			return this._playerKingdom.ActivePolicies.Contains(policy);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00026A28 File Offset: 0x00024C28
		private void SetDefaultSelectedPolicy()
		{
			KingdomPolicyItemVM kingdomPolicyItemVM = (this.IsInProposeMode ? this.OtherPolicies.FirstOrDefault<KingdomPolicyItemVM>() : this.ActivePolicies.FirstOrDefault<KingdomPolicyItemVM>());
			this.OnPolicySelect(kingdomPolicyItemVM);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00026A60 File Offset: 0x00024C60
		private void ExecuteSwitchMode()
		{
			this.IsInProposeMode = !this.IsInProposeMode;
			this.CurrentActiveModeText = (this.IsInProposeMode ? this.OtherPoliciesText : this.ActivePoliciesText);
			this.CurrentActionText = (this.IsInProposeMode ? this.DisavowPolicyText : this.ProposeNewPolicyText);
			this.SetDefaultSelectedPolicy();
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00026ABC File Offset: 0x00024CBC
		private void ExecuteProposeOrDisavow()
		{
			if (this._currentItemsUnresolvedDecision != null)
			{
				this._forceDecide(this._currentItemsUnresolvedDecision);
				return;
			}
			if (this.CanProposeOrDisavowPolicy)
			{
				KingdomDecision kingdomDecision = new KingdomPolicyDecision(Clan.PlayerClan, this._currentSelectedPolicyObject, this.IsPolicyActive(this._currentSelectedPolicyObject));
				Clan.PlayerClan.Kingdom.AddDecision(kingdomDecision, false);
				this._forceDecide(kingdomDecision);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00026B25 File Offset: 0x00024D25
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x00026B2D File Offset: 0x00024D2D
		[DataSourceProperty]
		public HintViewModel DoneHint
		{
			get
			{
				return this._doneHint;
			}
			set
			{
				if (value != this._doneHint)
				{
					this._doneHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneHint");
				}
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00026B4B File Offset: 0x00024D4B
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x00026B53 File Offset: 0x00024D53
		[DataSourceProperty]
		public MBBindingList<KingdomPolicyItemVM> ActivePolicies
		{
			get
			{
				return this._activePolicies;
			}
			set
			{
				if (value != this._activePolicies)
				{
					this._activePolicies = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomPolicyItemVM>>(value, "ActivePolicies");
				}
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00026B71 File Offset: 0x00024D71
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x00026B79 File Offset: 0x00024D79
		[DataSourceProperty]
		public MBBindingList<KingdomPolicyItemVM> OtherPolicies
		{
			get
			{
				return this._otherPolicies;
			}
			set
			{
				if (value != this._otherPolicies)
				{
					this._otherPolicies = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomPolicyItemVM>>(value, "OtherPolicies");
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00026B97 File Offset: 0x00024D97
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x00026B9F File Offset: 0x00024D9F
		[DataSourceProperty]
		public KingdomPolicyItemVM CurrentSelectedPolicy
		{
			get
			{
				return this._currentSelectedPolicy;
			}
			set
			{
				if (value != this._currentSelectedPolicy)
				{
					this._currentSelectedPolicy = value;
					base.OnPropertyChangedWithValue<KingdomPolicyItemVM>(value, "CurrentSelectedPolicy");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00026BBD File Offset: 0x00024DBD
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00026BC5 File Offset: 0x00024DC5
		[DataSourceProperty]
		public bool CanProposeOrDisavowPolicy
		{
			get
			{
				return this._canProposeOrDisavowPolicy;
			}
			set
			{
				if (value != this._canProposeOrDisavowPolicy)
				{
					this._canProposeOrDisavowPolicy = value;
					base.OnPropertyChangedWithValue(value, "CanProposeOrDisavowPolicy");
				}
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00026BE3 File Offset: 0x00024DE3
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x00026BEB File Offset: 0x00024DEB
		[DataSourceProperty]
		public int ProposalAndDisavowalCost
		{
			get
			{
				return this._proposalAndDisavowalCost;
			}
			set
			{
				if (value != this._proposalAndDisavowalCost)
				{
					this._proposalAndDisavowalCost = value;
					base.OnPropertyChangedWithValue(value, "ProposalAndDisavowalCost");
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00026C09 File Offset: 0x00024E09
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x00026C11 File Offset: 0x00024E11
		[DataSourceProperty]
		public string NumOfActivePoliciesText
		{
			get
			{
				return this._numOfActivePoliciesText;
			}
			set
			{
				if (value != this._numOfActivePoliciesText)
				{
					this._numOfActivePoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfActivePoliciesText");
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00026C34 File Offset: 0x00024E34
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x00026C3C File Offset: 0x00024E3C
		[DataSourceProperty]
		public string NumOfOtherPoliciesText
		{
			get
			{
				return this._numOfOtherPoliciesText;
			}
			set
			{
				if (value != this._numOfOtherPoliciesText)
				{
					this._numOfOtherPoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfOtherPoliciesText");
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00026C5F File Offset: 0x00024E5F
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00026C67 File Offset: 0x00024E67
		[DataSourceProperty]
		public bool IsInProposeMode
		{
			get
			{
				return this._isInProposeMode;
			}
			set
			{
				if (value != this._isInProposeMode)
				{
					this._isInProposeMode = value;
					base.OnPropertyChangedWithValue(value, "IsInProposeMode");
				}
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00026C85 File Offset: 0x00024E85
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00026C8D File Offset: 0x00024E8D
		[DataSourceProperty]
		public string DisavowPolicyText
		{
			get
			{
				return this._disavowPolicyText;
			}
			set
			{
				if (value != this._disavowPolicyText)
				{
					this._disavowPolicyText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisavowPolicyText");
				}
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00026CB0 File Offset: 0x00024EB0
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00026CB8 File Offset: 0x00024EB8
		[DataSourceProperty]
		public string CurrentActiveModeText
		{
			get
			{
				return this._currentActiveModeText;
			}
			set
			{
				if (value != this._currentActiveModeText)
				{
					this._currentActiveModeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentActiveModeText");
				}
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00026CDB File Offset: 0x00024EDB
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00026CE3 File Offset: 0x00024EE3
		[DataSourceProperty]
		public string CurrentActionText
		{
			get
			{
				return this._currentActionText;
			}
			set
			{
				if (value != this._currentActionText)
				{
					this._currentActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentActionText");
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00026D06 File Offset: 0x00024F06
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00026D0E File Offset: 0x00024F0E
		[DataSourceProperty]
		public string ProposeNewPolicyText
		{
			get
			{
				return this._proposeNewPolicyText;
			}
			set
			{
				if (value != this._proposeNewPolicyText)
				{
					this._proposeNewPolicyText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeNewPolicyText");
				}
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00026D31 File Offset: 0x00024F31
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00026D39 File Offset: 0x00024F39
		[DataSourceProperty]
		public string BackText
		{
			get
			{
				return this._backText;
			}
			set
			{
				if (value != this._backText)
				{
					this._backText = value;
					base.OnPropertyChangedWithValue<string>(value, "BackText");
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00026D5C File Offset: 0x00024F5C
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00026D64 File Offset: 0x00024F64
		[DataSourceProperty]
		public string PoliciesText
		{
			get
			{
				return this._policiesText;
			}
			set
			{
				if (value != this._policiesText)
				{
					this._policiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "PoliciesText");
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00026D87 File Offset: 0x00024F87
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00026D8F File Offset: 0x00024F8F
		[DataSourceProperty]
		public string ActivePoliciesText
		{
			get
			{
				return this._activePoliciesText;
			}
			set
			{
				if (value != this._activePoliciesText)
				{
					this._activePoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActivePoliciesText");
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00026DB2 File Offset: 0x00024FB2
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00026DBA File Offset: 0x00024FBA
		[DataSourceProperty]
		public string PolicyLikelihoodText
		{
			get
			{
				return this._policyLikelihoodText;
			}
			set
			{
				if (value != this._policyLikelihoodText)
				{
					this._policyLikelihoodText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyLikelihoodText");
				}
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00026DDD File Offset: 0x00024FDD
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00026DE5 File Offset: 0x00024FE5
		[DataSourceProperty]
		public HintViewModel LikelihoodHint
		{
			get
			{
				return this._likelihoodHint;
			}
			set
			{
				if (value != this._likelihoodHint)
				{
					this._likelihoodHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LikelihoodHint");
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00026E03 File Offset: 0x00025003
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00026E0B File Offset: 0x0002500B
		[DataSourceProperty]
		public int PolicyLikelihood
		{
			get
			{
				return this._policyLikelihood;
			}
			set
			{
				if (value != this._policyLikelihood)
				{
					this._policyLikelihood = value;
					base.OnPropertyChangedWithValue(value, "PolicyLikelihood");
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00026E29 File Offset: 0x00025029
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x00026E31 File Offset: 0x00025031
		[DataSourceProperty]
		public string OtherPoliciesText
		{
			get
			{
				return this._otherPoliciesText;
			}
			set
			{
				if (value != this._otherPoliciesText)
				{
					this._otherPoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "OtherPoliciesText");
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00026E54 File Offset: 0x00025054
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00026E5C File Offset: 0x0002505C
		[DataSourceProperty]
		public string ProposeOrDisavowText
		{
			get
			{
				return this._proposeOrDisavowText;
			}
			set
			{
				if (value != this._proposeOrDisavowText)
				{
					this._proposeOrDisavowText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeOrDisavowText");
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00026E7F File Offset: 0x0002507F
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00026E87 File Offset: 0x00025087
		[DataSourceProperty]
		public string ProposeActionExplanationText
		{
			get
			{
				return this._proposeActionExplanationText;
			}
			set
			{
				if (value != this._proposeActionExplanationText)
				{
					this._proposeActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeActionExplanationText");
				}
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00026EAC File Offset: 0x000250AC
		private static int CalculateLikelihood(PolicyObject policy)
		{
			KingdomElection kingdomElection = new KingdomElection(new KingdomPolicyDecision(Clan.PlayerClan, policy, Clan.PlayerClan.Kingdom.ActivePolicies.Contains(policy)));
			kingdomElection.SetupResultWithoutPlayerSupport();
			return MathF.Round(kingdomElection.GetWinChanceForSponsor(Clan.PlayerClan) * 100f);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00026EFC File Offset: 0x000250FC
		private static TextObject GetSupportText(PolicyObject policy)
		{
			KingdomPolicyDecision kingdomPolicyDecision = new KingdomPolicyDecision(Clan.PlayerClan, policy, Clan.PlayerClan.Kingdom.ActivePolicies.Contains(policy));
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(kingdomPolicyDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x040003AA RID: 938
		private readonly Action<KingdomDecision> _forceDecide;

		// Token: 0x040003AB RID: 939
		private readonly Kingdom _playerKingdom;

		// Token: 0x040003AC RID: 940
		private PolicyObject _currentSelectedPolicyObject;

		// Token: 0x040003AD RID: 941
		private KingdomDecision _currentItemsUnresolvedDecision;

		// Token: 0x040003AE RID: 942
		private MBBindingList<KingdomPolicyItemVM> _activePolicies;

		// Token: 0x040003AF RID: 943
		private MBBindingList<KingdomPolicyItemVM> _otherPolicies;

		// Token: 0x040003B0 RID: 944
		private KingdomPolicyItemVM _currentSelectedPolicy;

		// Token: 0x040003B1 RID: 945
		private bool _canProposeOrDisavowPolicy;

		// Token: 0x040003B2 RID: 946
		private bool _isInProposeMode = true;

		// Token: 0x040003B3 RID: 947
		private string _proposeOrDisavowText;

		// Token: 0x040003B4 RID: 948
		private string _proposeActionExplanationText;

		// Token: 0x040003B5 RID: 949
		private string _activePoliciesText;

		// Token: 0x040003B6 RID: 950
		private string _otherPoliciesText;

		// Token: 0x040003B7 RID: 951
		private string _currentActiveModeText;

		// Token: 0x040003B8 RID: 952
		private string _currentActionText;

		// Token: 0x040003B9 RID: 953
		private string _proposeNewPolicyText;

		// Token: 0x040003BA RID: 954
		private string _disavowPolicyText;

		// Token: 0x040003BB RID: 955
		private string _policiesText;

		// Token: 0x040003BC RID: 956
		private string _backText;

		// Token: 0x040003BD RID: 957
		private int _proposalAndDisavowalCost;

		// Token: 0x040003BE RID: 958
		private string _numOfActivePoliciesText;

		// Token: 0x040003BF RID: 959
		private string _numOfOtherPoliciesText;

		// Token: 0x040003C0 RID: 960
		private HintViewModel _doneHint;

		// Token: 0x040003C1 RID: 961
		private string _policyLikelihoodText;

		// Token: 0x040003C2 RID: 962
		private HintViewModel _likelihoodHint;

		// Token: 0x040003C3 RID: 963
		private int _policyLikelihood;
	}
}
