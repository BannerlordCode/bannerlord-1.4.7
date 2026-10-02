using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A3 RID: 163
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMachineTargetRotation : GameNetworkMessage
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0000B808 File Offset: 0x00009A08
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x0000B810 File Offset: 0x00009A10
		public MissionObjectId UsableMachineId { get; private set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0000B819 File Offset: 0x00009A19
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0000B821 File Offset: 0x00009A21
		public float HorizontalRotation { get; private set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0000B82A File Offset: 0x00009A2A
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0000B832 File Offset: 0x00009A32
		public float VerticalRotation { get; private set; }

		// Token: 0x06000687 RID: 1671 RVA: 0x0000B83B File Offset: 0x00009A3B
		public SetMachineTargetRotation(MissionObjectId usableMachineId, float horizontalRotaiton, float verticalRotation)
		{
			this.UsableMachineId = usableMachineId;
			this.HorizontalRotation = horizontalRotaiton;
			this.VerticalRotation = verticalRotation;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0000B858 File Offset: 0x00009A58
		public SetMachineTargetRotation()
		{
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0000B860 File Offset: 0x00009A60
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMachineId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.HorizontalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			this.VerticalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0000B8A1 File Offset: 0x00009AA1
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMachineId);
			GameNetworkMessage.WriteFloatToPacket(this.HorizontalRotation, CompressionBasic.HighResRadianCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.VerticalRotation, CompressionBasic.HighResRadianCompressionInfo);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0000B8CE File Offset: 0x00009ACE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0000B8D6 File Offset: 0x00009AD6
		protected override string OnGetLogFormat()
		{
			return "Set target rotation of UsableMachine with ID: " + this.UsableMachineId;
		}
	}
}
