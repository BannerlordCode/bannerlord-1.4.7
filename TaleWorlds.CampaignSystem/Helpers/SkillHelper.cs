using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000019 RID: 25
	public static class SkillHelper
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x0000C6C8 File Offset: 0x0000A8C8
		public static void AddSkillBonusForSkillLevel(SkillEffect skillEffect, ref ExplainedNumber explainedNumber, int skillLevel)
		{
			float skillEffectValue = skillEffect.GetSkillEffectValue(skillLevel);
			SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000C714 File Offset: 0x0000A914
		public static void AddSkillBonusForParty(SkillEffect skillEffect, MobileParty party, ref ExplainedNumber explainedNumber)
		{
			CharacterObject characterObject;
			if (skillEffect.Role == PartyRole.PartyLeader && party.LeaderHero != null)
			{
				Hero leaderHero = party.LeaderHero;
				characterObject = ((leaderHero != null) ? leaderHero.CharacterObject : null);
			}
			else if (party.GetEffectiveRoleHolder(skillEffect.Role) != null)
			{
				characterObject = party.GetEffectiveRoleHolder(skillEffect.Role).CharacterObject;
			}
			else
			{
				characterObject = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
			}
			if (characterObject != null)
			{
				int skillValue = characterObject.GetSkillValue(skillEffect.EffectedSkill);
				float skillEffectValue = skillEffect.GetSkillEffectValue(skillValue);
				SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		public static void AddSkillBonusForTown(SkillEffect skillEffect, Town town, ref ExplainedNumber explainedNumber)
		{
			CharacterObject characterObject = null;
			if (skillEffect.Role == PartyRole.ClanLeader)
			{
				Clan ownerClan = town.Owner.Settlement.OwnerClan;
				characterObject = ((ownerClan != null) ? ownerClan.Leader.CharacterObject : null);
			}
			else if (skillEffect.Role == PartyRole.Governor)
			{
				Hero governor = town.Governor;
				characterObject = ((governor != null) ? governor.CharacterObject : null);
			}
			if (characterObject != null)
			{
				int skillValue = characterObject.GetSkillValue(skillEffect.EffectedSkill);
				float skillEffectValue = skillEffect.GetSkillEffectValue(skillValue);
				SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000C870 File Offset: 0x0000AA70
		public static void AddSkillBonusForCharacter(SkillEffect skillEffect, CharacterObject character, ref ExplainedNumber explainedNumber)
		{
			int skillValue = character.GetSkillValue(skillEffect.EffectedSkill);
			float skillEffectValue = skillEffect.GetSkillEffectValue(skillValue);
			SkillHelper.AddToStat(ref explainedNumber, skillEffect.IncrementType, skillEffectValue, explainedNumber.IncludeDescriptions ? GameTexts.FindText("role", skillEffect.Role.ToString()) : null);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
		public static TextObject GetEffectDescriptionForSkillLevel(SkillEffect effect, int level)
		{
			float skillEffectValue = effect.GetSkillEffectValue(level);
			float num = ((effect.IncrementType == EffectIncrementType.AddFactor) ? (skillEffectValue * 100f) : skillEffectValue);
			effect.Description.SetTextVariable("a0", MathF.Abs(num).ToString("0.0"));
			return effect.Description;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000C91B File Offset: 0x0000AB1B
		private static void AddToStat(ref ExplainedNumber stat, EffectIncrementType effectIncrementType, float number, TextObject text)
		{
			if (effectIncrementType == EffectIncrementType.Add)
			{
				stat.Add(number, text, null);
				return;
			}
			if (effectIncrementType == EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number, text);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000C936 File Offset: 0x0000AB36
		public static CharacterObject GetEffectivePartyLeaderForSkill(PartyBase party)
		{
			if (party == null)
			{
				return null;
			}
			if (party.LeaderHero != null)
			{
				return party.LeaderHero.CharacterObject;
			}
			TroopRoster memberRoster = party.MemberRoster;
			if (memberRoster == null || memberRoster.TotalManCount <= 0)
			{
				return null;
			}
			return party.MemberRoster.GetCharacterAtIndex(0);
		}
	}
}
