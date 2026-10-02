using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x02000489 RID: 1161
	public class PeaceBarterable : Barterable
	{
		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06004990 RID: 18832 RVA: 0x00174BEA File Offset: 0x00172DEA
		public CampaignTime Duration { get; }

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x00174BF2 File Offset: 0x00172DF2
		public override string StringID
		{
			get
			{
				return "peace_barterable";
			}
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00174BF9 File Offset: 0x00172DF9
		public PeaceBarterable(Hero owner, IFaction peaceOfferingFaction, IFaction offeredFaction, CampaignTime duration)
		{
			MobileParty partyBelongedTo = owner.PartyBelongedTo;
			base..ctor(owner, (partyBelongedTo != null) ? partyBelongedTo.Party : null);
			this.Duration = duration;
			this.PeaceOfferingFaction = peaceOfferingFaction;
			this.OfferedFaction = offeredFaction;
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00174C2C File Offset: 0x00172E2C
		public PeaceBarterable(IFaction peaceOfferingFaction, IFaction offeredFaction, CampaignTime duration)
		{
			Hero leader = peaceOfferingFaction.Leader;
			Hero leader2 = peaceOfferingFaction.Leader;
			PartyBase partyBase;
			if (leader2 == null)
			{
				partyBase = null;
			}
			else
			{
				MobileParty partyBelongedTo = leader2.PartyBelongedTo;
				partyBase = ((partyBelongedTo != null) ? partyBelongedTo.Party : null);
			}
			base..ctor(leader, partyBase);
			this.Duration = duration;
			this.PeaceOfferingFaction = peaceOfferingFaction;
			this.OfferedFaction = offeredFaction;
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06004994 RID: 18836 RVA: 0x00174C78 File Offset: 0x00172E78
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=R0bJS0pn}Make peace with the {OTHER_FACTION}", null);
				textObject.SetTextVariable("OTHER_FACTION", this.OfferedFaction.InformalName);
				return textObject;
			}
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00174C9C File Offset: 0x00172E9C
		public override int GetUnitValueForFaction(IFaction factionToEvaluateFor)
		{
			float num = 0f;
			IFaction faction = this.OfferedFaction;
			IFaction faction2 = this.PeaceOfferingFaction;
			if (factionToEvaluateFor.MapFaction == faction)
			{
				IFaction faction3 = faction2;
				IFaction faction4 = faction;
				faction = faction3;
				faction2 = faction4;
			}
			if (faction == null || faction2 == null)
			{
				return 0;
			}
			TextObject textObject;
			num = (float)((int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeaceForClan(faction2, faction, factionToEvaluateFor.Leader.Clan, out textObject, false));
			if (factionToEvaluateFor.IsKingdomFaction)
			{
				float num2 = 0f;
				int num3 = 0;
				foreach (Clan clan in ((Kingdom)factionToEvaluateFor).Clans)
				{
					float num4 = ((clan.Leader != null) ? ((clan.Leader.Gold < 50000) ? (1f + 0.5f * ((50000f - (float)clan.Leader.Gold) / 50000f)) : ((clan.Leader.Gold > 200000) ? MathF.Max(0.66f, MathF.Pow(200000f / (float)clan.Leader.Gold, 0.4f)) : 1f)) : 1f);
					num2 += num4;
					num3++;
				}
				float num5 = (num2 + 1f) / ((float)num3 + 1f);
				num /= num5;
			}
			return (int)num;
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00174E14 File Offset: 0x00173014
		public override bool IsCompatible(Barterable barterable)
		{
			PeaceBarterable peaceBarterable = barterable as PeaceBarterable;
			return peaceBarterable == null || peaceBarterable.OfferedFaction != base.OriginalOwner.MapFaction;
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00174E43 File Offset: 0x00173043
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00174E46 File Offset: 0x00173046
		public override string GetEncyclopediaLink()
		{
			return base.OriginalOwner.MapFaction.EncyclopediaLink;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00174E58 File Offset: 0x00173058
		public override void Apply()
		{
			if (this.PeaceOfferingFaction.MapFaction.IsAtWarWith(this.OfferedFaction))
			{
				MakePeaceAction.Apply(this.PeaceOfferingFaction.MapFaction, this.OfferedFaction);
				if (PlayerEncounter.Current != null && Hero.OneToOneConversationHero == base.OriginalOwner)
				{
					PlayerEncounter.LeaveEncounter = true;
					PartyBase originalParty = base.OriginalParty;
					bool flag;
					if (originalParty == null)
					{
						flag = null != null;
					}
					else
					{
						MobileParty mobileParty = originalParty.MobileParty;
						flag = ((mobileParty != null) ? mobileParty.Ai.AiBehaviorPartyBase : null) != null;
					}
					if (flag)
					{
						LocatableSearchData<MobileParty> locatableSearchData = Campaign.Current.MobilePartyLocator.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position.ToVec2(), 5f);
						for (MobileParty mobileParty2 = Campaign.Current.MobilePartyLocator.FindNextLocatable(ref locatableSearchData); mobileParty2 != null; mobileParty2 = Campaign.Current.MobilePartyLocator.FindNextLocatable(ref locatableSearchData))
						{
							if (!mobileParty2.IsMainParty && mobileParty2.MapFaction == base.OriginalOwner.MapFaction && (mobileParty2.TargetParty == MobileParty.MainParty || mobileParty2.Ai.AiBehaviorPartyBase == PartyBase.MainParty || mobileParty2.TargetSettlement == MobileParty.MainParty.TargetSettlement))
							{
								mobileParty2.SetMoveModeHold();
							}
						}
						if (base.OriginalParty.MobileParty.Army != null && MobileParty.MainParty.Army != base.OriginalParty.MobileParty.Army)
						{
							base.OriginalParty.MobileParty.Army.LeaderParty.SetMoveModeHold();
						}
					}
				}
			}
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00174FC8 File Offset: 0x001731C8
		internal static void AutoGeneratedStaticCollectObjectsPeaceBarterable(object o, List<object> collectedObjects)
		{
			((PeaceBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00174FD6 File Offset: 0x001731D6
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0400144E RID: 5198
		public readonly IFaction PeaceOfferingFaction;

		// Token: 0x0400144F RID: 5199
		public readonly IFaction OfferedFaction;
	}
}
