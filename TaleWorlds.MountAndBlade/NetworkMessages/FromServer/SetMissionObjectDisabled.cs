using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A8 RID: 168
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectDisabled : GameNetworkMessage
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0000BD85 File Offset: 0x00009F85
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x0000BD8D File Offset: 0x00009F8D
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x060006BF RID: 1727 RVA: 0x0000BD96 File Offset: 0x00009F96
		public SetMissionObjectDisabled(MissionObjectId missionObjectId)
		{
			this.MissionObjectId = missionObjectId;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0000BDA5 File Offset: 0x00009FA5
		public SetMissionObjectDisabled()
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0000BDB0 File Offset: 0x00009FB0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0000BDCD File Offset: 0x00009FCD
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0000BDDA File Offset: 0x00009FDA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0000BDE2 File Offset: 0x00009FE2
		protected override string OnGetLogFormat()
		{
			return "Mission Object with ID: " + this.MissionObjectId + " has been disabled.";
		}
	}
}
