using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x0200037D RID: 893
	public class DefaultIssueEffects
	{
		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600345C RID: 13404 RVA: 0x000D8688 File Offset: 0x000D6888
		private static DefaultIssueEffects Instance
		{
			get
			{
				return Campaign.Current.DefaultIssueEffects;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x000D8694 File Offset: 0x000D6894
		public static IssueEffect SettlementLoyalty
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementLoyalty;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600345E RID: 13406 RVA: 0x000D86A0 File Offset: 0x000D68A0
		public static IssueEffect SettlementSecurity
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementSecurity;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x000D86AC File Offset: 0x000D68AC
		public static IssueEffect SettlementMilitia
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementMilitia;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06003460 RID: 13408 RVA: 0x000D86B8 File Offset: 0x000D68B8
		public static IssueEffect SettlementProsperity
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementProsperity;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06003461 RID: 13409 RVA: 0x000D86C4 File Offset: 0x000D68C4
		public static IssueEffect VillageHearth
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectVillageHearth;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003462 RID: 13410 RVA: 0x000D86D0 File Offset: 0x000D68D0
		public static IssueEffect SettlementFood
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementFood;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x000D86DC File Offset: 0x000D68DC
		public static IssueEffect SettlementTax
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementTax;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x000D86E8 File Offset: 0x000D68E8
		public static IssueEffect SettlementGarrison
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementGarrison;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06003465 RID: 13413 RVA: 0x000D86F4 File Offset: 0x000D68F4
		public static IssueEffect HalfVillageProduction
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectHalfVillageProduction;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06003466 RID: 13414 RVA: 0x000D8700 File Offset: 0x000D6900
		public static IssueEffect IssueOwnerPower
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectIssueOwnerPower;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x000D870C File Offset: 0x000D690C
		public static IssueEffect ClanInfluence
		{
			get
			{
				return DefaultIssueEffects.Instance._clanInfluence;
			}
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x000D8718 File Offset: 0x000D6918
		public DefaultIssueEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x000D8728 File Offset: 0x000D6928
		private void RegisterAll()
		{
			this._issueEffectSettlementLoyalty = this.Create("issue_effect_settlement_loyalty");
			this._issueEffectSettlementSecurity = this.Create("issue_effect_settlement_security");
			this._issueEffectSettlementMilitia = this.Create("issue_effect_settlement_militia");
			this._issueEffectSettlementProsperity = this.Create("issue_effect_settlement_prosperity");
			this._issueEffectVillageHearth = this.Create("issue_effect_village_hearth");
			this._issueEffectSettlementFood = this.Create("issue_effect_settlement_food");
			this._issueEffectSettlementTax = this.Create("issue_effect_settlement_tax");
			this._issueEffectSettlementGarrison = this.Create("issue_effect_settlement_garrison");
			this._issueEffectHalfVillageProduction = this.Create("issue_effect_half_village_production");
			this._issueEffectIssueOwnerPower = this.Create("issue_effect_issue_owner_power");
			this._clanInfluence = this.Create("issue_effect_clan_influence");
			this.InitializeAll();
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000D87F6 File Offset: 0x000D69F6
		private IssueEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<IssueEffect>(new IssueEffect(stringId));
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000D8810 File Offset: 0x000D6A10
		private void InitializeAll()
		{
			this._issueEffectSettlementLoyalty.Initialize(new TextObject("{=YO0x7ZAo}Loyalty", null), new TextObject("{=xAWvm25T}Effects settlement's loyalty.", null));
			this._issueEffectSettlementSecurity.Initialize(new TextObject("{=MqCH7R4A}Security", null), new TextObject("{=h117Qj3E}Effects settlement's security.", null));
			this._issueEffectSettlementMilitia.Initialize(new TextObject("{=gsVtO9A7}Militia", null), new TextObject("{=dTmPV82D}Effects settlement's militia.", null));
			this._issueEffectSettlementProsperity.Initialize(new TextObject("{=IagYTD5O}Prosperity", null), new TextObject("{=ETye0JMY}Effects settlement's prosperity.", null));
			this._issueEffectVillageHearth.Initialize(new TextObject("{=f5X5uU0m}Village Hearth", null), new TextObject("{=7TbVhbT9}Effects village's hearth.", null));
			this._issueEffectSettlementFood.Initialize(new TextObject("{=qSi4DlT4}Food", null), new TextObject("{=onDsUkUl}Effects settlement's food.", null));
			this._issueEffectSettlementTax.Initialize(new TextObject("{=2awf1tei}Tax", null), new TextObject("{=q2Ovtr1s}Effects settlement's tax.", null));
			this._issueEffectSettlementGarrison.Initialize(new TextObject("{=jlgjLDo7}Garrison", null), new TextObject("{=WJ7SnBgN}Effects settlement's garrison.", null));
			this._issueEffectHalfVillageProduction.Initialize(new TextObject("{=bGyrPe8c}Production", null), new TextObject("{=arbaXvQf}Effects village's production.", null));
			this._issueEffectIssueOwnerPower.Initialize(new TextObject("{=gGXelWQX}Issue owner power", null), new TextObject("{=tjudHtDB}Effects the power of issue owner in the settlement.", null));
			this._clanInfluence.Initialize(new TextObject("{=KN6khbSl}Clan Influence", null), new TextObject("{=y2aLOwOs}Effects the influence of clan.", null));
		}

		// Token: 0x04000EEE RID: 3822
		private IssueEffect _issueEffectSettlementGarrison;

		// Token: 0x04000EEF RID: 3823
		private IssueEffect _issueEffectSettlementLoyalty;

		// Token: 0x04000EF0 RID: 3824
		private IssueEffect _issueEffectSettlementSecurity;

		// Token: 0x04000EF1 RID: 3825
		private IssueEffect _issueEffectSettlementMilitia;

		// Token: 0x04000EF2 RID: 3826
		private IssueEffect _issueEffectSettlementProsperity;

		// Token: 0x04000EF3 RID: 3827
		private IssueEffect _issueEffectVillageHearth;

		// Token: 0x04000EF4 RID: 3828
		private IssueEffect _issueEffectSettlementFood;

		// Token: 0x04000EF5 RID: 3829
		private IssueEffect _issueEffectSettlementTax;

		// Token: 0x04000EF6 RID: 3830
		private IssueEffect _issueEffectHalfVillageProduction;

		// Token: 0x04000EF7 RID: 3831
		private IssueEffect _issueEffectIssueOwnerPower;

		// Token: 0x04000EF8 RID: 3832
		private IssueEffect _clanInfluence;
	}
}
