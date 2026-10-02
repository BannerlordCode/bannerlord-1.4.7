using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A1 RID: 161
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetBatteringRamHasArrivedAtTarget : GameNetworkMessage
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0000B6C6 File Offset: 0x000098C6
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x0000B6CE File Offset: 0x000098CE
		public MissionObjectId BatteringRamId { get; private set; }

		// Token: 0x06000670 RID: 1648 RVA: 0x0000B6D7 File Offset: 0x000098D7
		public SetBatteringRamHasArrivedAtTarget(MissionObjectId batteringRamId)
		{
			this.BatteringRamId = batteringRamId;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0000B6E6 File Offset: 0x000098E6
		public SetBatteringRamHasArrivedAtTarget()
		{
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0000B6F0 File Offset: 0x000098F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.BatteringRamId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0000B70D File Offset: 0x0000990D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.BatteringRamId);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0000B71A File Offset: 0x0000991A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0000B722 File Offset: 0x00009922
		protected override string OnGetLogFormat()
		{
			return "Battering Ram with ID: " + this.BatteringRamId + " has arrived at its target.";
		}
	}
}
