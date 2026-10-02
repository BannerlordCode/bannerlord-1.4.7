using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EF RID: 495
	public abstract class DifficultyModel : MBGameModel<DifficultyModel>
	{
		// Token: 0x06001F3A RID: 7994
		public abstract float GetPlayerTroopsReceivedDamageMultiplier();

		// Token: 0x06001F3B RID: 7995
		public abstract int GetPlayerRecruitSlotBonus();

		// Token: 0x06001F3C RID: 7996
		public abstract float GetPlayerMapMovementSpeedBonusMultiplier();

		// Token: 0x06001F3D RID: 7997
		public abstract float GetCombatAIDifficultyMultiplier();

		// Token: 0x06001F3E RID: 7998
		public abstract float GetPersuasionBonusChance();

		// Token: 0x06001F3F RID: 7999
		public abstract float GetClanMemberDeathChanceMultiplier();

		// Token: 0x06001F40 RID: 8000
		public abstract float GetStealthDifficultyMultiplier();

		// Token: 0x06001F41 RID: 8001
		public abstract float GetDisguiseDifficultyMultiplier();
	}
}
