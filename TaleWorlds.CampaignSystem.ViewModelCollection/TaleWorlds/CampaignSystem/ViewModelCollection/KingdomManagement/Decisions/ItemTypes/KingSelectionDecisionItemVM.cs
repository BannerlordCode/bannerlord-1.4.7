using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200007E RID: 126
	public class KingSelectionDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0002D1BD File Offset: 0x0002B3BD
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as KingSelectionKingdomDecision).Kingdom;
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0002D1CF File Offset: 0x0002B3CF
		public KingSelectionDecisionItemVM(KingSelectionKingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._kingSelectionDecision = decision;
			base.DecisionType = 6;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002D1E8 File Offset: 0x0002B3E8
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_king_selection", null);
			textObject.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.NameText = textObject.ToString();
			this.FactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.FactionName = this.TargetFaction.Culture.Name.ToString();
			bool flag = true;
			bool flag2 = true;
			int num = 0;
			int num2 = 0;
			foreach (Settlement settlement in this.TargetFaction.Settlements)
			{
				if (settlement.IsTown)
				{
					if (flag)
					{
						this.SettlementsListText = settlement.EncyclopediaLinkWithName.ToString();
						flag = false;
					}
					else
					{
						GameTexts.SetVariable("LEFT", this.SettlementsListText);
						GameTexts.SetVariable("RIGHT", settlement.EncyclopediaLinkWithName);
						this.SettlementsListText = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
					}
					num++;
				}
				else if (settlement.IsCastle)
				{
					if (flag2)
					{
						this.CastlesListText = settlement.EncyclopediaLinkWithName.ToString();
						flag2 = false;
					}
					else
					{
						GameTexts.SetVariable("LEFT", this.CastlesListText);
						GameTexts.SetVariable("RIGHT", settlement.EncyclopediaLinkWithName);
						this.CastlesListText = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
					}
					num2++;
				}
			}
			TextObject textObject2 = GameTexts.FindText("str_settlements", null);
			TextObject textObject3 = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject3.SetTextVariable("STR", num);
			TextObject textObject4 = GameTexts.FindText("str_LEFT_RIGHT", null);
			textObject4.SetTextVariable("LEFT", textObject2);
			textObject4.SetTextVariable("RIGHT", textObject3);
			this.SettlementsText = textObject4.ToString();
			TextObject textObject5 = GameTexts.FindText("str_castles", null);
			TextObject textObject6 = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject6.SetTextVariable("STR", num2);
			TextObject textObject7 = GameTexts.FindText("str_LEFT_RIGHT", null);
			textObject7.SetTextVariable("LEFT", textObject5);
			textObject7.SetTextVariable("RIGHT", textObject6);
			this.CastlesText = textObject7.ToString();
			this.TotalStrengthText = GameTexts.FindText("str_total_strength", null).ToString();
			this.TotalStrength = (int)this.TargetFaction.CurrentTotalStrength;
			this.ActivePoliciesText = GameTexts.FindText("str_active_policies", null).ToString();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			foreach (PolicyObject policyObject in kingdom.ActivePolicies)
			{
				if (policyObject == kingdom.ActivePolicies[0])
				{
					this.ActivePoliciesListText = policyObject.Name.ToString();
				}
				else
				{
					GameTexts.SetVariable("LEFT", this.ActivePoliciesListText);
					GameTexts.SetVariable("RIGHT", policyObject.Name.ToString());
					this.ActivePoliciesListText = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
				}
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0002D518 File Offset: 0x0002B718
		private void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0002D52A File Offset: 0x0002B72A
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0002D532 File Offset: 0x0002B732
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0002D555 File Offset: 0x0002B755
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0002D55D File Offset: 0x0002B75D
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

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0002D580 File Offset: 0x0002B780
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x0002D588 File Offset: 0x0002B788
		[DataSourceProperty]
		public BannerImageIdentifierVM FactionBanner
		{
			get
			{
				return this._factionBanner;
			}
			set
			{
				if (value != this._factionBanner)
				{
					this._factionBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "FactionBanner");
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0002D5A6 File Offset: 0x0002B7A6
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x0002D5AE File Offset: 0x0002B7AE
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._settlementsText;
			}
			set
			{
				if (value != this._settlementsText)
				{
					this._settlementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsText");
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0002D5D1 File Offset: 0x0002B7D1
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x0002D5D9 File Offset: 0x0002B7D9
		[DataSourceProperty]
		public string SettlementsListText
		{
			get
			{
				return this._settlementsListText;
			}
			set
			{
				if (value != this._settlementsListText)
				{
					this._settlementsListText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsListText");
				}
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0002D5FC File Offset: 0x0002B7FC
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x0002D604 File Offset: 0x0002B804
		[DataSourceProperty]
		public string CastlesText
		{
			get
			{
				return this._castlesText;
			}
			set
			{
				if (value != this._castlesText)
				{
					this._castlesText = value;
					base.OnPropertyChangedWithValue<string>(value, "CastlesText");
				}
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0002D627 File Offset: 0x0002B827
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x0002D62F File Offset: 0x0002B82F
		[DataSourceProperty]
		public string CastlesListText
		{
			get
			{
				return this._castlesListText;
			}
			set
			{
				if (value != this._castlesListText)
				{
					this._castlesListText = value;
					base.OnPropertyChangedWithValue<string>(value, "CastlesListText");
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0002D652 File Offset: 0x0002B852
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x0002D65A File Offset: 0x0002B85A
		[DataSourceProperty]
		public string TotalStrengthText
		{
			get
			{
				return this._totalStrengthText;
			}
			set
			{
				if (value != this._totalStrengthText)
				{
					this._totalStrengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalStrengthText");
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0002D67D File Offset: 0x0002B87D
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x0002D685 File Offset: 0x0002B885
		[DataSourceProperty]
		public int TotalStrength
		{
			get
			{
				return this._totalStrength;
			}
			set
			{
				if (value != this._totalStrength)
				{
					this._totalStrength = value;
					base.OnPropertyChangedWithValue(value, "TotalStrength");
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0002D6A3 File Offset: 0x0002B8A3
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x0002D6AB File Offset: 0x0002B8AB
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

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0002D6CE File Offset: 0x0002B8CE
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x0002D6D6 File Offset: 0x0002B8D6
		[DataSourceProperty]
		public string ActivePoliciesListText
		{
			get
			{
				return this._activePoliciesListText;
			}
			set
			{
				if (value != this._activePoliciesListText)
				{
					this._activePoliciesListText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActivePoliciesListText");
				}
			}
		}

		// Token: 0x0400049E RID: 1182
		private readonly KingSelectionKingdomDecision _kingSelectionDecision;

		// Token: 0x0400049F RID: 1183
		private string _nameText;

		// Token: 0x040004A0 RID: 1184
		private string _factionName;

		// Token: 0x040004A1 RID: 1185
		private BannerImageIdentifierVM _factionBanner;

		// Token: 0x040004A2 RID: 1186
		private string _settlementsText;

		// Token: 0x040004A3 RID: 1187
		private string _settlementsListText;

		// Token: 0x040004A4 RID: 1188
		private string _castlesText;

		// Token: 0x040004A5 RID: 1189
		private string _castlesListText;

		// Token: 0x040004A6 RID: 1190
		private int _totalStrength;

		// Token: 0x040004A7 RID: 1191
		private string _totalStrengthText;

		// Token: 0x040004A8 RID: 1192
		private string _activePoliciesText;

		// Token: 0x040004A9 RID: 1193
		private string _activePoliciesListText;
	}
}
