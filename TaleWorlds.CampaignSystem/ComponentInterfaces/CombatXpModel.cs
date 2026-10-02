using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A1 RID: 417
	public abstract class CombatXpModel : MBGameModel<CombatXpModel>
	{
		// Token: 0x06001CAF RID: 7343
		public abstract SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit);

		// Token: 0x06001CB0 RID: 7344
		public abstract ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType);

		// Token: 0x06001CB1 RID: 7345
		public abstract float GetXpMultiplierFromShotDifficulty(float shotDifficulty);

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001CB2 RID: 7346
		public abstract float CaptainRadius { get; }

		// Token: 0x020005FF RID: 1535
		public enum MissionTypeEnum
		{
			// Token: 0x040018FD RID: 6397
			Battle,
			// Token: 0x040018FE RID: 6398
			PracticeFight,
			// Token: 0x040018FF RID: 6399
			Tournament,
			// Token: 0x04001900 RID: 6400
			SimulationBattle,
			// Token: 0x04001901 RID: 6401
			NoXp
		}
	}
}
