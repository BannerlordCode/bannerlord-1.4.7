using System;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000012 RID: 18
	[OverrideView(typeof(MissionCheatView))]
	public class MissionGauntletMultiplayerCheatView : MissionCheatView
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00005CA9 File Offset: 0x00003EA9
		public override bool GetIsCheatsAvailable()
		{
			return false;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005CAC File Offset: 0x00003EAC
		public override void InitializeScreen()
		{
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005CAE File Offset: 0x00003EAE
		public override void FinalizeScreen()
		{
		}
	}
}
