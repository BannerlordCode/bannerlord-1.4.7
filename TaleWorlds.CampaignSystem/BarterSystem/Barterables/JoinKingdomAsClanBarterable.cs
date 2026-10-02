using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x02000484 RID: 1156
	public class JoinKingdomAsClanBarterable : Barterable
	{
		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x00173B96 File Offset: 0x00171D96
		public override string StringID
		{
			get
			{
				return "join_faction_barterable";
			}
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x00173B9D File Offset: 0x00171D9D
		public JoinKingdomAsClanBarterable(Hero owner, Kingdom targetKingdom, bool isDefecting = false)
			: base(owner, null)
		{
			this.TargetKingdom = targetKingdom;
			this.IsDefecting = isDefecting;
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x00173BB5 File Offset: 0x00171DB5
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=8Az4q2wp}Join {FACTION}", null);
				textObject.SetTextVariable("FACTION", this.TargetKingdom.Name);
				return textObject;
			}
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x00173BDC File Offset: 0x00171DDC
		public override int GetUnitValueForFaction(IFaction factionForEvaluation)
		{
			float num = -1000000f;
			if (factionForEvaluation == base.OriginalOwner.Clan)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetScoreOfClanToJoinKingdom(base.OriginalOwner.Clan, this.TargetKingdom);
				if (base.OriginalOwner.Clan.Kingdom != null)
				{
					int valueForFaction = new LeaveKingdomAsClanBarterable(base.OriginalOwner, base.OriginalParty).GetValueForFaction(factionForEvaluation);
					if (!this.TargetKingdom.IsAtWarWith(base.OriginalOwner.Clan.Kingdom))
					{
						float num2 = base.OriginalOwner.Clan.CalculateTotalSettlementValueForFaction(base.OriginalOwner.Clan.Kingdom);
						num -= num2 * ((this.TargetKingdom.Leader == Hero.MainHero) ? 0.5f : 1f);
					}
					num += (float)valueForFaction;
				}
			}
			else if (factionForEvaluation.MapFaction == this.TargetKingdom)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetScoreOfKingdomToGetClan(this.TargetKingdom, base.OriginalOwner.Clan);
			}
			if (this.TargetKingdom == Clan.PlayerClan.Kingdom && Hero.MainHero.GetPerkValue(DefaultPerks.Trade.SilverTongue))
			{
				num += num * DefaultPerks.Trade.SilverTongue.PrimaryBonus;
			}
			return (int)num;
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x00173D29 File Offset: 0x00171F29
		public override void CheckBarterLink(Barterable linkedBarterable)
		{
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x00173D2C File Offset: 0x00171F2C
		public override bool IsCompatible(Barterable barterable)
		{
			LeaveKingdomAsClanBarterable leaveKingdomAsClanBarterable = barterable as LeaveKingdomAsClanBarterable;
			return leaveKingdomAsClanBarterable == null || leaveKingdomAsClanBarterable.OriginalOwner.MapFaction != this.TargetKingdom;
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x00173D5B File Offset: 0x00171F5B
		public override ImageIdentifier GetVisualIdentifier()
		{
			return new BannerImageIdentifier(this.TargetKingdom.Banner, false);
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x00173D6E File Offset: 0x00171F6E
		public override string GetEncyclopediaLink()
		{
			return this.TargetKingdom.EncyclopediaLink;
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x00173D7C File Offset: 0x00171F7C
		public override void Apply()
		{
			if (this.TargetKingdom != null && this.TargetKingdom != null && this.TargetKingdom.Leader == Hero.MainHero)
			{
				int valueForFaction = base.GetValueForFaction(base.OriginalOwner.Clan);
				int num = ((valueForFaction < 0) ? (20 - valueForFaction / 20000) : 20);
				ChangeRelationAction.ApplyPlayerRelation(base.OriginalOwner.Clan.Leader, num, true, true);
				if (base.OriginalOwner.Clan.MapFaction != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(base.OriginalOwner.Clan.Leader, base.OriginalOwner.Clan.MapFaction.Leader, -100, true);
				}
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker)
			{
				PlayerEncounter.Current.SetPlayerSiegeInterruptedByEnemyDefection();
			}
			bool flag = base.OriginalOwner.Clan.IsMinorFaction && base.OriginalOwner.Clan != Clan.PlayerClan;
			Kingdom kingdom = base.OriginalOwner.Clan.Kingdom;
			if (!this.IsDefecting && base.OriginalOwner.Clan.Kingdom != null)
			{
				if (flag)
				{
					ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(base.OriginalOwner.Clan, true);
				}
				else if (base.OriginalOwner.Clan.Kingdom != null && this.TargetKingdom != null && base.OriginalOwner.Clan.Kingdom.IsAtWarWith(this.TargetKingdom))
				{
					ChangeKingdomAction.ApplyByLeaveWithRebellionAgainstKingdom(base.OriginalOwner.Clan, true);
				}
				else
				{
					ChangeKingdomAction.ApplyByLeaveKingdom(base.OriginalOwner.Clan, true);
				}
			}
			if (flag)
			{
				ChangeKingdomAction.ApplyByJoinFactionAsMercenary(base.OriginalOwner.Clan, this.TargetKingdom, default(CampaignTime), Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(base.OriginalOwner.Clan, this.TargetKingdom, false), true);
				return;
			}
			if (this.IsDefecting)
			{
				ChangeKingdomAction.ApplyByJoinToKingdomByDefection(base.OriginalOwner.Clan, kingdom, this.TargetKingdom, default(CampaignTime), true);
				return;
			}
			ChangeKingdomAction.ApplyByJoinToKingdom(base.OriginalOwner.Clan, this.TargetKingdom, default(CampaignTime), true);
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x00173FBA File Offset: 0x001721BA
		internal static void AutoGeneratedStaticCollectObjectsJoinKingdomAsClanBarterable(object o, List<object> collectedObjects)
		{
			((JoinKingdomAsClanBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00173FC8 File Offset: 0x001721C8
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x04001444 RID: 5188
		public readonly Kingdom TargetKingdom;

		// Token: 0x04001445 RID: 5189
		public readonly bool IsDefecting;
	}
}
