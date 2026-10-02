using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000318 RID: 792
	public static class RoundStateExtensions
	{
		// Token: 0x06002D2F RID: 11567 RVA: 0x000AF54C File Offset: 0x000AD74C
		public static bool StateHasVisualTimer(this MultiplayerRoundState roundState)
		{
			return roundState - MultiplayerRoundState.Preparation <= 1;
		}
	}
}
