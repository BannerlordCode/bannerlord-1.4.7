using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B6 RID: 182
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeMachineMovementDistance : GameNetworkMessage
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0000CB51 File Offset: 0x0000AD51
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0000CB59 File Offset: 0x0000AD59
		public MissionObjectId UsableMachineId { get; private set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0000CB62 File Offset: 0x0000AD62
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x0000CB6A File Offset: 0x0000AD6A
		public float Distance { get; private set; }

		// Token: 0x06000753 RID: 1875 RVA: 0x0000CB73 File Offset: 0x0000AD73
		public SetSiegeMachineMovementDistance(MissionObjectId usableMachineId, float distance)
		{
			this.UsableMachineId = usableMachineId;
			this.Distance = distance;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000CB89 File Offset: 0x0000AD89
		public SetSiegeMachineMovementDistance()
		{
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000CB94 File Offset: 0x0000AD94
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMachineId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Distance = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000CBC3 File Offset: 0x0000ADC3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMachineId);
			GameNetworkMessage.WriteFloatToPacket(this.Distance, CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000CBE8 File Offset: 0x0000ADE8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Movement Distance: ", this.Distance, " of SiegeMachine with ID: ", this.UsableMachineId });
		}
	}
}
