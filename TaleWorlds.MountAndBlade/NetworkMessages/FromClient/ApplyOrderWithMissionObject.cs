using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000025 RID: 37
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplyOrderWithMissionObject : GameNetworkMessage
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000038F2 File Offset: 0x00001AF2
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000038FA File Offset: 0x00001AFA
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x06000127 RID: 295 RVA: 0x00003903 File Offset: 0x00001B03
		public ApplyOrderWithMissionObject(MissionObjectId missionObjectId)
		{
			this.MissionObjectId = missionObjectId;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00003912 File Offset: 0x00001B12
		public ApplyOrderWithMissionObject()
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000391C File Offset: 0x00001B1C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00003939 File Offset: 0x00001B39
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00003946 File Offset: 0x00001B46
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000394E File Offset: 0x00001B4E
		protected override string OnGetLogFormat()
		{
			return "Apply order to MissionObject with ID: " + this.MissionObjectId + " and with name ";
		}
	}
}
