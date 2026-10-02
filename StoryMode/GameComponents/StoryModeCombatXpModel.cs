using System;
using StoryMode.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003F RID: 63
	public class StoryModeCombatXpModel : CombatXpModel
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00018EF8 File Offset: 0x000170F8
		public override float CaptainRadius
		{
			get
			{
				return base.BaseModel.CaptainRadius;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00018F05 File Offset: 0x00017105
		public override SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit)
		{
			return base.BaseModel.GetSkillForWeapon(weapon, isSiegeEngineHit);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00018F14 File Offset: 0x00017114
		public override ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTrainingField())
			{
				return new ExplainedNumber(0f, false, null);
			}
			return base.BaseModel.GetXpFromHit(attackerTroop, captain, attackedTroop, attackerParty, damage, isFatal, missionType);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00018F4C File Offset: 0x0001714C
		public override float GetXpMultiplierFromShotDifficulty(float shotDifficulty)
		{
			return base.BaseModel.GetXpMultiplierFromShotDifficulty(shotDifficulty);
		}
	}
}
