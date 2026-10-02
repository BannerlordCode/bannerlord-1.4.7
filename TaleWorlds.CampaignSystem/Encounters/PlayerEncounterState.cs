using System;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x020002ED RID: 749
	public enum PlayerEncounterState
	{
		// Token: 0x04000C22 RID: 3106
		Begin,
		// Token: 0x04000C23 RID: 3107
		Wait,
		// Token: 0x04000C24 RID: 3108
		PrepareResults,
		// Token: 0x04000C25 RID: 3109
		ApplyResults,
		// Token: 0x04000C26 RID: 3110
		PlayerVictory,
		// Token: 0x04000C27 RID: 3111
		PlayerTotalDefeat,
		// Token: 0x04000C28 RID: 3112
		CaptureHeroes,
		// Token: 0x04000C29 RID: 3113
		FreeHeroes,
		// Token: 0x04000C2A RID: 3114
		LootParty,
		// Token: 0x04000C2B RID: 3115
		LootInventory,
		// Token: 0x04000C2C RID: 3116
		LootShips,
		// Token: 0x04000C2D RID: 3117
		End
	}
}
