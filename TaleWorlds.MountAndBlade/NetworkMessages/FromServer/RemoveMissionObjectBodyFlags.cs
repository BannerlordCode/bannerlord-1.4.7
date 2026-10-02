using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000097 RID: 151
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveMissionObjectBodyFlags : GameNetworkMessage
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0000AC05 File Offset: 0x00008E05
		// (set) Token: 0x060005F7 RID: 1527 RVA: 0x0000AC0D File Offset: 0x00008E0D
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000AC16 File Offset: 0x00008E16
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x0000AC1E File Offset: 0x00008E1E
		public BodyFlags BodyFlags { get; private set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0000AC27 File Offset: 0x00008E27
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0000AC2F File Offset: 0x00008E2F
		public bool ApplyToChildren { get; private set; }

		// Token: 0x060005FC RID: 1532 RVA: 0x0000AC38 File Offset: 0x00008E38
		public RemoveMissionObjectBodyFlags(MissionObjectId missionObjectId, BodyFlags bodyFlags, bool applyToChildren)
		{
			this.MissionObjectId = missionObjectId;
			this.BodyFlags = bodyFlags;
			this.ApplyToChildren = applyToChildren;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0000AC55 File Offset: 0x00008E55
		public RemoveMissionObjectBodyFlags()
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0000AC60 File Offset: 0x00008E60
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.BodyFlags = (BodyFlags)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.FlagsCompressionInfo, ref flag);
			this.ApplyToChildren = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0000AC9C File Offset: 0x00008E9C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket((int)this.BodyFlags, CompressionBasic.FlagsCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.ApplyToChildren);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0000ACC4 File Offset: 0x00008EC4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0000ACCC File Offset: 0x00008ECC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Remove bodyflags: ",
				this.BodyFlags,
				" from MissionObject with ID: ",
				this.MissionObjectId,
				this.ApplyToChildren ? "" : " and from all of its children."
			});
		}
	}
}
