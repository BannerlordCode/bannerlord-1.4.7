using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B7 RID: 183
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeTowerGateState : GameNetworkMessage
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0000CC21 File Offset: 0x0000AE21
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0000CC29 File Offset: 0x0000AE29
		public MissionObjectId SiegeTowerId { get; private set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0000CC32 File Offset: 0x0000AE32
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0000CC3A File Offset: 0x0000AE3A
		public SiegeTower.GateState State { get; private set; }

		// Token: 0x0600075D RID: 1885 RVA: 0x0000CC43 File Offset: 0x0000AE43
		public SetSiegeTowerGateState(MissionObjectId siegeTowerId, SiegeTower.GateState state)
		{
			this.SiegeTowerId = siegeTowerId;
			this.State = state;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0000CC59 File Offset: 0x0000AE59
		public SetSiegeTowerGateState()
		{
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0000CC64 File Offset: 0x0000AE64
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeTowerId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.State = (SiegeTower.GateState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeTowerGateStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0000CC93 File Offset: 0x0000AE93
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeTowerId);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeTowerGateStateCompressionInfo);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0000CCB8 File Offset: 0x0000AEB8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set SiegeTower State to: ", this.State, " on SiegeTower with ID: ", this.SiegeTowerId });
		}
	}
}
