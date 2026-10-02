using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000129 RID: 297
	public class DefaultMapVisibilityModel : MapVisibilityModel
	{
		// Token: 0x06001899 RID: 6297 RVA: 0x000772C3 File Offset: 0x000754C3
		public override float MaximumSeeingRange()
		{
			return 60f;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000772CA File Offset: 0x000754CA
		public override float GetPartySeeingRangeBase(MobileParty party)
		{
			if (!Campaign.Current.IsNight)
			{
				return 12f;
			}
			return 6f;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000772E4 File Offset: 0x000754E4
		public override ExplainedNumber GetPartySpottingRange(MobileParty party, bool includeDescriptions = false)
		{
			float partySeeingRangeBase = Campaign.Current.Models.MapVisibilityModel.GetPartySeeingRangeBase(party);
			ExplainedNumber explainedNumber = new ExplainedNumber(partySeeingRangeBase, includeDescriptions, null);
			SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.TrackingSpottingDistance, party, ref explainedNumber);
			if (!party.IsCurrentlyAtSea)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.EagleEye, party, false, ref explainedNumber, false);
			}
			Hero effectiveScout = party.EffectiveScout;
			if (effectiveScout != null)
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
				if (faceTerrainType == TerrainType.Forest && PartyBaseHelper.HasFeat(party.Party, DefaultCulturalFeats.BattanianForestSpeedFeat))
				{
					explainedNumber.AddFactor(0.15f, GameTexts.FindText("str_culture", null));
				}
				if (!party.IsCurrentlyAtSea)
				{
					if ((faceTerrainType == TerrainType.Plain || faceTerrainType == TerrainType.Steppe) && effectiveScout.GetPerkValue(DefaultPerks.Scouting.WaterDiviner))
					{
						explainedNumber.AddFactor(DefaultPerks.Scouting.WaterDiviner.PrimaryBonus, DefaultPerks.Scouting.WaterDiviner.Name);
					}
					if (Campaign.Current.IsNight)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.NightRunner, party, false, ref explainedNumber, false);
					}
					else
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.DayTraveler, party, false, ref explainedNumber, false);
					}
				}
				if (!party.IsMoving && !party.IsCurrentlyAtSea && party.StationaryStartTime.ElapsedHoursUntilNow >= 1f)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.VantagePoint, party, false, ref explainedNumber, false);
				}
				if (effectiveScout.GetPerkValue(DefaultPerks.Scouting.MountedScouts) && !party.IsCurrentlyAtSea)
				{
					float num = 0f;
					for (int i = 0; i < party.MemberRoster.Count; i++)
					{
						if (party.MemberRoster.GetCharacterAtIndex(i).DefaultFormationClass.Equals(FormationClass.Cavalry))
						{
							num += (float)party.MemberRoster.GetElementNumber(i);
						}
					}
					if (num / (float)party.MemberRoster.TotalManCount >= 0.5f)
					{
						explainedNumber.AddFactor(DefaultPerks.Scouting.MountedScouts.PrimaryBonus, DefaultPerks.Scouting.MountedScouts.Name);
					}
				}
			}
			explainedNumber.LimitMax(Campaign.Current.Models.MapVisibilityModel.MaximumSeeingRange(), new TextObject("{=6qv6Hdww}Limit", null));
			return explainedNumber;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000774F0 File Offset: 0x000756F0
		public override float GetPartySpottingRatioForMainPartySeeingRange(MobileParty party)
		{
			float num = 1f;
			if (Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace) == TerrainType.Forest)
			{
				float num2 = -0.3f;
				if (MobileParty.MainParty.HasPerk(DefaultPerks.Scouting.KeenSight, false))
				{
					num2 += num2 * DefaultPerks.Scouting.KeenSight.PrimaryBonus;
				}
				num += num2;
			}
			int num3 = ((party.Army != null && party.Army.LeaderParty == party) ? party.Army.TotalManCount : party.MemberRoster.TotalManCount);
			return MBMath.ClampFloat(1.1f - 0.5f * MathF.Pow(2.7182817f, (float)(-(float)num3) / 200f), 0f, 1f) * num;
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000775A8 File Offset: 0x000757A8
		public override float GetHideoutSpottingDistance()
		{
			if (MobileParty.MainParty.HasPerk(DefaultPerks.Scouting.RumourNetwork, true))
			{
				return MobileParty.MainParty.SeeingRange * 1.2f * (1f + DefaultPerks.Scouting.RumourNetwork.SecondaryBonus);
			}
			return MobileParty.MainParty.SeeingRange * 1.2f;
		}

		// Token: 0x04000809 RID: 2057
		private const float PartySpottingDifficultyInForests = 0.3f;
	}
}
