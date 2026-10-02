using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CB RID: 203
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncObjectHitpoints : GameNetworkMessage
	{
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0000E3D5 File Offset: 0x0000C5D5
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0000E3E6 File Offset: 0x0000C5E6
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0000E3EE File Offset: 0x0000C5EE
		public float Hitpoints { get; private set; }

		// Token: 0x0600085B RID: 2139 RVA: 0x0000E3F7 File Offset: 0x0000C5F7
		public SyncObjectHitpoints(MissionObjectId missionObjectId, float hitpoints)
		{
			this.MissionObjectId = missionObjectId;
			this.Hitpoints = hitpoints;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000E40D File Offset: 0x0000C60D
		public SyncObjectHitpoints()
		{
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0000E418 File Offset: 0x0000C618
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Hitpoints = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.UsableGameObjectHealthCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0000E447 File Offset: 0x0000C647
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteFloatToPacket(MathF.Max(this.Hitpoints, 0f), CompressionMission.UsableGameObjectHealthCompressionInfo);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0000E46E File Offset: 0x0000C66E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0000E476 File Offset: 0x0000C676
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Synchronize HitPoints: ", this.Hitpoints, " of MissionObject with Id: ", this.MissionObjectId });
		}
	}
}
