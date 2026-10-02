using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010F RID: 271
	public class DefaultDifficultyModel : DifficultyModel
	{
		// Token: 0x0600176F RID: 5999 RVA: 0x0006E404 File Offset: 0x0006C604
		public override float GetPlayerTroopsReceivedDamageMultiplier()
		{
			switch (CampaignOptions.PlayerTroopsReceivedDamage)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.5f;
			case CampaignOptions.Difficulty.Easy:
				return 0.75f;
			case CampaignOptions.Difficulty.Realistic:
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0006E444 File Offset: 0x0006C644
		public override int GetPlayerRecruitSlotBonus()
		{
			switch (CampaignOptions.RecruitmentDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 2;
			case CampaignOptions.Difficulty.Easy:
				return 1;
			case CampaignOptions.Difficulty.Realistic:
				return 0;
			default:
				return 0;
			}
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0006E474 File Offset: 0x0006C674
		public override float GetPlayerMapMovementSpeedBonusMultiplier()
		{
			switch (CampaignOptions.PlayerMapMovementSpeed)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.05f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0006E4B4 File Offset: 0x0006C6B4
		public override float GetStealthDifficultyMultiplier()
		{
			switch (CampaignOptions.StealthAndDisguiseDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.5f;
			case CampaignOptions.Difficulty.Easy:
				return 0.75f;
			case CampaignOptions.Difficulty.Realistic:
				return 1f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0006E4F4 File Offset: 0x0006C6F4
		public override float GetDisguiseDifficultyMultiplier()
		{
			switch (CampaignOptions.StealthAndDisguiseDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.4f;
			case CampaignOptions.Difficulty.Easy:
				return 1f;
			case CampaignOptions.Difficulty.Realistic:
				return 1.2f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0006E534 File Offset: 0x0006C734
		public override float GetCombatAIDifficultyMultiplier()
		{
			switch (CampaignOptions.CombatAIDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0f;
			case CampaignOptions.Difficulty.Easy:
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0006E56C File Offset: 0x0006C76C
		public override float GetPersuasionBonusChance()
		{
			switch (CampaignOptions.PersuasionSuccessChance)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.05f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0006E5AC File Offset: 0x0006C7AC
		public override float GetClanMemberDeathChanceMultiplier()
		{
			switch (CampaignOptions.ClanMemberDeathChance)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return -1f;
			case CampaignOptions.Difficulty.Easy:
				return -0.5f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}
	}
}
