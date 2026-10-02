using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007C RID: 124
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BurstMissionObjectParticles : GameNetworkMessage
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00008539 File Offset: 0x00006739
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x00008541 File Offset: 0x00006741
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0000854A File Offset: 0x0000674A
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x00008552 File Offset: 0x00006752
		public bool DoChildren { get; private set; }

		// Token: 0x0600047C RID: 1148 RVA: 0x0000855B File Offset: 0x0000675B
		public BurstMissionObjectParticles(MissionObjectId missionObjectId, bool doChildren)
		{
			this.MissionObjectId = missionObjectId;
			this.DoChildren = doChildren;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00008571 File Offset: 0x00006771
		public BurstMissionObjectParticles()
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000857C File Offset: 0x0000677C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.DoChildren = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000085A6 File Offset: 0x000067A6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.DoChildren);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000085BE File Offset: 0x000067BE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.Particles;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000085C6 File Offset: 0x000067C6
		protected override string OnGetLogFormat()
		{
			return "Burst MissionObject particles on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
