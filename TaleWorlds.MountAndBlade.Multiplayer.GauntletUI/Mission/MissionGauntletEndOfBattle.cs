using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000F RID: 15
	[OverrideView(typeof(MultiplayerEndOfBattleUIHandler))]
	public class MissionGauntletEndOfBattle : MissionView
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x0000553C File Offset: 0x0000373C
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 30;
			this._dataSource = new MultiplayerEndOfBattleVM();
			this._gauntletLayer = new GauntletLayer("MultiplayerEndOfBattle", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerEndOfBattle", this._dataSource);
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._lobbyComponent.OnPostMatchEnded += this.OnPostMatchEnded;
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000055C9 File Offset: 0x000037C9
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._lobbyComponent.OnPostMatchEnded -= this.OnPostMatchEnded;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000055E8 File Offset: 0x000037E8
		private void OnPostMatchEnded()
		{
			this._dataSource.OnBattleEnded();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000055F5 File Offset: 0x000037F5
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.OnTick(dt);
		}

		// Token: 0x0400004B RID: 75
		private MultiplayerEndOfBattleVM _dataSource;

		// Token: 0x0400004C RID: 76
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400004D RID: 77
		private MissionLobbyComponent _lobbyComponent;
	}
}
