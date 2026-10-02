using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AF RID: 175
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectVertexAnimationProgress : GameNetworkMessage
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0000C5AF File Offset: 0x0000A7AF
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0000C5B7 File Offset: 0x0000A7B7
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0000C5C0 File Offset: 0x0000A7C0
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		public float Progress { get; private set; }

		// Token: 0x0600070F RID: 1807 RVA: 0x0000C5D1 File Offset: 0x0000A7D1
		public SetMissionObjectVertexAnimationProgress(MissionObjectId missionObjectId, float progress)
		{
			this.MissionObjectId = missionObjectId;
			this.Progress = progress;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0000C5E7 File Offset: 0x0000A7E7
		public SetMissionObjectVertexAnimationProgress()
		{
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Progress = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0000C61F File Offset: 0x0000A81F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteFloatToPacket(this.Progress, CompressionBasic.AnimationProgressCompressionInfo);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0000C63C File Offset: 0x0000A83C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0000C644 File Offset: 0x0000A844
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set progress of Vertex Animation on MissionObject with ID: ", this.MissionObjectId, " to: ", this.Progress });
		}
	}
}
