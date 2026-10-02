using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000160 RID: 352
	public class DefaultTroopSacrificeModel : TroopSacrificeModel
	{
		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0008B5AB File Offset: 0x000897AB
		public override int BreakOutArmyLeaderRelationPenalty
		{
			get
			{
				return -5;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0008B5AF File Offset: 0x000897AF
		public override int BreakOutArmyMemberRelationPenalty
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0008B5B2 File Offset: 0x000897B2
		public override ExplainedNumber GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent)
		{
			return this.GetLostTroopCount(party, siegeEvent, party.IsTargetingPort && party.IsCurrentlyAtSea);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0008B5CD File Offset: 0x000897CD
		public override ExplainedNumber GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, bool isBreakingOutFromPort)
		{
			return this.GetLostTroopCount(party, siegeEvent, isBreakingOutFromPort);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0008B5D8 File Offset: 0x000897D8
		public override int GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent)
		{
			mapEvent.RecalculateStrengthOfSides();
			MapEventSide mapEventSide = mapEvent.GetMapEventSide(playerBattleSide);
			float num = mapEvent.StrengthOfSide[(int)playerBattleSide] + 1f;
			float num2 = mapEvent.StrengthOfSide[(int)playerBattleSide.GetOppositeSide()] / num;
			int num3 = PartyBase.MainParty.NumberOfRegularMembers;
			if (MobileParty.MainParty.Army != null)
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					num3 += mobileParty.Party.NumberOfRegularMembers;
				}
			}
			int num4 = mapEventSide.CountTroops((FlattenedTroopRosterElement x) => x.State == RosterTroopState.Active && !x.Troop.IsHero);
			float num5 = (float)num3 * MathF.Pow(MathF.Min(num2, 3f), 1.3f) * 0.1f + 5f;
			ExplainedNumber explainedNumber = new ExplainedNumber(num5, false, null);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.TacticsTroopSacrificeReduction, CharacterObject.PlayerCharacter, ref explainedNumber);
			explainedNumber = new ExplainedNumber((float)MathF.Max(1, MathF.Round(explainedNumber.ResultNumber)), false, null);
			if (!MobileParty.MainParty.IsCurrentlyAtSea)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.SwiftRegroup, MobileParty.MainParty, false, ref explainedNumber, false);
			}
			if (explainedNumber.ResultNumber <= (float)num4)
			{
				return MathF.Round(explainedNumber.ResultNumber);
			}
			return -1;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0008B748 File Offset: 0x00089948
		private ExplainedNumber GetLostTroopCount(MobileParty party, SiegeEvent siegeEvent, bool isFromPort)
		{
			if (isFromPort && !siegeEvent.IsBlockadeActive)
			{
				return new ExplainedNumber(0f, false, null);
			}
			int num = 5;
			float num2 = 0f;
			foreach (PartyBase partyBase in siegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
			{
				num2 += (isFromPort ? partyBase.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.SeaBattle) : partyBase.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.PlainBattle));
			}
			float num3;
			int num4;
			if (party.Army != null && party.Army.LeaderParty == party)
			{
				num3 = (isFromPort ? party.Army.LeaderParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.SeaBattle) : party.Army.LeaderParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.PlainBattle));
				foreach (MobileParty mobileParty in party.Army.LeaderParty.AttachedParties)
				{
					num3 += (isFromPort ? mobileParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.SeaBattle) : mobileParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.PlainBattle));
				}
				num4 = party.Army.TotalRegularCount;
			}
			else
			{
				num3 = (isFromPort ? party.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.SeaBattle) : party.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.PlainBattle));
				num4 = party.MemberRoster.TotalRegulars;
			}
			float num5 = MathF.Clamp(0.12f * MathF.Pow((num2 + 1f) / (num3 + 1f), 0.25f), 0.12f, 0.24f);
			ExplainedNumber explainedNumber = new ExplainedNumber(num5 * (float)num4, false, null);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.TacticsTroopSacrificeReduction, CharacterObject.PlayerCharacter, ref explainedNumber);
			explainedNumber = new ExplainedNumber((float)(num + (int)explainedNumber.ResultNumber), false, null);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Improviser, MobileParty.MainParty, false, ref explainedNumber, isFromPort);
			return explainedNumber;
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0008B948 File Offset: 0x00089B48
		public override bool CanPlayerGetAwayFromEncounter(out TextObject explanation)
		{
			explanation = TextObject.GetEmpty();
			int num = PartyBase.MainParty.NumberOfHealthyMembers - PartyBase.MainParty.MemberRoster.TotalHeroes;
			if (MobileParty.MainParty.Army != null && (MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty || MobileParty.MainParty.AttachedTo != null))
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					num += mobileParty.Party.NumberOfHealthyMembers - mobileParty.Party.MemberRoster.TotalHeroes;
				}
			}
			if (num <= 8 || Campaign.Current.Models.TroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle) == -1)
			{
				explanation = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				return false;
			}
			return true;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0008BA4C File Offset: 0x00089C4C
		public override void GetShipsToSacrificeForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent, out MBList<Ship> shipsToCapture, out Ship shipToTakeDamage, out float damageToApplyForLastShip)
		{
			shipsToCapture = new MBList<Ship>();
			shipToTakeDamage = null;
			damageToApplyForLastShip = 0f;
		}

		// Token: 0x0400091E RID: 2334
		public const int MinimumNumberOfTroopsRequiredForGetAway = 8;
	}
}
