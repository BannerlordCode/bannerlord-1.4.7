using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A7 RID: 167
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectColors : GameNetworkMessage
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0000BC9D File Offset: 0x00009E9D
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0000BCA5 File Offset: 0x00009EA5
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0000BCAE File Offset: 0x00009EAE
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0000BCB6 File Offset: 0x00009EB6
		public uint Color { get; private set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0000BCBF File Offset: 0x00009EBF
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0000BCC7 File Offset: 0x00009EC7
		public uint Color2 { get; private set; }

		// Token: 0x060006B7 RID: 1719 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		public SetMissionObjectColors(MissionObjectId missionObjectId, uint color, uint color2)
		{
			this.MissionObjectId = missionObjectId;
			this.Color = color;
			this.Color2 = color2;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0000BCED File Offset: 0x00009EED
		public SetMissionObjectColors()
		{
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0000BCF8 File Offset: 0x00009EF8
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Color = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.Color2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0000BD39 File Offset: 0x00009F39
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteUintToPacket(this.Color, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.Color2, CompressionBasic.ColorCompressionInfo);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0000BD66 File Offset: 0x00009F66
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0000BD6E File Offset: 0x00009F6E
		protected override string OnGetLogFormat()
		{
			return "Set Colors of MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
