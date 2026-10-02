using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000096 RID: 150
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveMissionObject : GameNetworkMessage
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0000AB94 File Offset: 0x00008D94
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public MissionObjectId ObjectId { get; private set; }

		// Token: 0x060005F0 RID: 1520 RVA: 0x0000ABA5 File Offset: 0x00008DA5
		public RemoveMissionObject(MissionObjectId objectId)
		{
			this.ObjectId = objectId;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0000ABB4 File Offset: 0x00008DB4
		public RemoveMissionObject()
		{
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0000ABBC File Offset: 0x00008DBC
		protected override bool OnRead()
		{
			bool flag = true;
			this.ObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0000ABD9 File Offset: 0x00008DD9
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.ObjectId);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0000ABE6 File Offset: 0x00008DE6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000ABEE File Offset: 0x00008DEE
		protected override string OnGetLogFormat()
		{
			return "Remove MissionObject with ID: " + this.ObjectId;
		}
	}
}
