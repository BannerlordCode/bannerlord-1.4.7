using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B8 RID: 184
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeTowerHasArrivedAtTarget : GameNetworkMessage
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0000CCF1 File Offset: 0x0000AEF1
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x0000CCF9 File Offset: 0x0000AEF9
		public MissionObjectId SiegeTowerId { get; private set; }

		// Token: 0x06000765 RID: 1893 RVA: 0x0000CD02 File Offset: 0x0000AF02
		public SetSiegeTowerHasArrivedAtTarget(MissionObjectId siegeTowerId)
		{
			this.SiegeTowerId = siegeTowerId;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000CD11 File Offset: 0x0000AF11
		public SetSiegeTowerHasArrivedAtTarget()
		{
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000CD1C File Offset: 0x0000AF1C
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeTowerId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000CD39 File Offset: 0x0000AF39
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeTowerId);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0000CD46 File Offset: 0x0000AF46
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeapons;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000CD4E File Offset: 0x0000AF4E
		protected override string OnGetLogFormat()
		{
			return "SiegeTower with ID: " + this.SiegeTowerId + " has arrived at its target.";
		}
	}
}
