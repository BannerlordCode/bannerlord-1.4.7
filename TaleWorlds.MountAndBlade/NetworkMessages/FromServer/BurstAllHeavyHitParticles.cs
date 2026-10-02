using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007B RID: 123
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BurstAllHeavyHitParticles : GameNetworkMessage
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x000084C5 File Offset: 0x000066C5
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x000084CD File Offset: 0x000066CD
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x06000472 RID: 1138 RVA: 0x000084D6 File Offset: 0x000066D6
		public BurstAllHeavyHitParticles(MissionObjectId missionObjectId)
		{
			this.MissionObjectId = missionObjectId;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000084E5 File Offset: 0x000066E5
		public BurstAllHeavyHitParticles()
		{
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000084F0 File Offset: 0x000066F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000850D File Offset: 0x0000670D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000851A File Offset: 0x0000671A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00008522 File Offset: 0x00006722
		protected override string OnGetLogFormat()
		{
			return "Bursting all heavy-hit particles for the DestructableComponent of MissionObject with Id: " + this.MissionObjectId;
		}
	}
}
