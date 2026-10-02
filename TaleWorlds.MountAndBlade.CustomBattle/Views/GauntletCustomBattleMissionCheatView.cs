using System;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.CustomBattle.Views
{
	// Token: 0x02000012 RID: 18
	[OverrideView(typeof(MissionCheatView))]
	internal class GauntletCustomBattleMissionCheatView : MissionCheatView
	{
		// Token: 0x06000109 RID: 265 RVA: 0x000086E3 File Offset: 0x000068E3
		public override void InitializeScreen()
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000086E5 File Offset: 0x000068E5
		public override void FinalizeScreen()
		{
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000086E7 File Offset: 0x000068E7
		public override bool GetIsCheatsAvailable()
		{
			return false;
		}
	}
}
