using System;
using NetworkMessages.FromServer;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200001A RID: 26
	[OverrideView(typeof(MissionMultiplayerServerStatusUIHandler))]
	public class MissionGauntletServerStatus : MissionView
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00007633 File Offset: 0x00005833
		private bool IsOptionEnabled
		{
			get
			{
				return BannerlordConfig.EnableNetworkAlertIcons;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000763C File Offset: 0x0000583C
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MultiplayerMissionServerStatusVM();
			this._gauntletLayer = new GauntletLayer("MultiplayerServerStatus", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerServerStatus", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			NetworkCommunicator.OnPeerAveragePingUpdated += this.OnPeerPingUpdated;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000076AA File Offset: 0x000058AA
		private void OnPeerPingUpdated(NetworkCommunicator obj)
		{
			if (this.IsOptionEnabled && obj.IsMine)
			{
				this._dataSource.UpdatePeerPing(obj.AveragePingInMilliseconds);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000076D0 File Offset: 0x000058D0
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this.IsOptionEnabled && GameNetwork.IsClient && GameNetwork.IsMyPeerReady)
			{
				this._dataSource.UpdatePacketLossRatio((GameNetwork.MyPeer != null) ? ((float)GameNetwork.MyPeer.AverageLossPercent) : 0f);
				MultiplayerMissionServerStatusVM dataSource = this._dataSource;
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				dataSource.UpdateServerPerformanceState((myPeer != null) ? myPeer.ServerPerformanceProblemState : ServerPerformanceState.High);
				return;
			}
			this._dataSource.ResetStates();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007746 File Offset: 0x00005946
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			NetworkCommunicator.OnPeerAveragePingUpdated -= this.OnPeerPingUpdated;
		}

		// Token: 0x0400007F RID: 127
		private MultiplayerMissionServerStatusVM _dataSource;

		// Token: 0x04000080 RID: 128
		private GauntletLayer _gauntletLayer;
	}
}
