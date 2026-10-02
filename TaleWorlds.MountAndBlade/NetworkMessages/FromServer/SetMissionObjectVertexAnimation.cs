using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AE RID: 174
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectVertexAnimation : GameNetworkMessage
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0000C48D File Offset: 0x0000A68D
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x0000C495 File Offset: 0x0000A695
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0000C49E File Offset: 0x0000A69E
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x0000C4A6 File Offset: 0x0000A6A6
		public int BeginKey { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0000C4AF File Offset: 0x0000A6AF
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0000C4B7 File Offset: 0x0000A6B7
		public int EndKey { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0000C4C0 File Offset: 0x0000A6C0
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
		public float Speed { get; private set; }

		// Token: 0x06000705 RID: 1797 RVA: 0x0000C4D1 File Offset: 0x0000A6D1
		public SetMissionObjectVertexAnimation(MissionObjectId missionObjectId, int beginKey, int endKey, float speed)
		{
			this.MissionObjectId = missionObjectId;
			this.BeginKey = beginKey;
			this.EndKey = endKey;
			this.Speed = speed;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0000C4F6 File Offset: 0x0000A6F6
		public SetMissionObjectVertexAnimation()
		{
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0000C500 File Offset: 0x0000A700
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.BeginKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref flag);
			this.EndKey = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationKeyCompressionInfo, ref flag);
			this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.VertexAnimationSpeedCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0000C553 File Offset: 0x0000A753
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.BeginKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.EndKey, CompressionBasic.AnimationKeyCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionBasic.VertexAnimationSpeedCompressionInfo);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0000C590 File Offset: 0x0000A790
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0000C598 File Offset: 0x0000A798
		protected override string OnGetLogFormat()
		{
			return "Set Vertex Animation on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
