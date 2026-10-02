using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C6 RID: 198
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class StopPhysicsAndSetFrameOfMissionObject : GameNetworkMessage
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0000DDDA File Offset: 0x0000BFDA
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x0000DDE2 File Offset: 0x0000BFE2
		public MissionObjectId ObjectId { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0000DDEB File Offset: 0x0000BFEB
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0000DDF3 File Offset: 0x0000BFF3
		public MissionObjectId ParentId { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0000DDFC File Offset: 0x0000BFFC
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0000DE04 File Offset: 0x0000C004
		public MatrixFrame Frame { get; private set; }

		// Token: 0x0600081F RID: 2079 RVA: 0x0000DE0D File Offset: 0x0000C00D
		public StopPhysicsAndSetFrameOfMissionObject(MissionObjectId objectId, MissionObjectId parentId, MatrixFrame frame)
		{
			this.ObjectId = objectId;
			this.ParentId = parentId;
			this.Frame = frame;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0000DE2A File Offset: 0x0000C02A
		public StopPhysicsAndSetFrameOfMissionObject()
		{
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0000DE34 File Offset: 0x0000C034
		protected override bool OnRead()
		{
			bool flag = true;
			this.ObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.ParentId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadNonUniformTransformFromPacket(CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0000DE78 File Offset: 0x0000C078
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.ObjectId);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.ParentId.Id >= 0) ? this.ParentId : MissionObjectId.Invalid);
			GameNetworkMessage.WriteNonUniformTransformToPacket(this.Frame, CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0000DEC5 File Offset: 0x0000C0C5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0000DECD File Offset: 0x0000C0CD
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Stop physics and set frame of MissionObject with ID: ", this.ObjectId, " Parent Index: ", this.ParentId });
		}
	}
}
