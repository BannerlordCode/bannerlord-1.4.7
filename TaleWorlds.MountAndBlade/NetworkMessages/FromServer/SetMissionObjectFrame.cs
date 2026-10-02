using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A9 RID: 169
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectFrame : GameNetworkMessage
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0000BDFE File Offset: 0x00009FFE
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0000BE06 File Offset: 0x0000A006
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0000BE0F File Offset: 0x0000A00F
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0000BE17 File Offset: 0x0000A017
		public MatrixFrame Frame { get; private set; }

		// Token: 0x060006C9 RID: 1737 RVA: 0x0000BE20 File Offset: 0x0000A020
		public SetMissionObjectFrame(MissionObjectId missionObjectId, ref MatrixFrame frame)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0000BE3B File Offset: 0x0000A03B
		public SetMissionObjectFrame()
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0000BE44 File Offset: 0x0000A044
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0000BE6E File Offset: 0x0000A06E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0000BE86 File Offset: 0x0000A086
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000BE8E File Offset: 0x0000A08E
		protected override string OnGetLogFormat()
		{
			return "Set Frame on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
