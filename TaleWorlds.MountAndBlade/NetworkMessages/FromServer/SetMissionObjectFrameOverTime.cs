using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AA RID: 170
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectFrameOverTime : GameNetworkMessage
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0000BEA5 File Offset: 0x0000A0A5
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0000BEAD File Offset: 0x0000A0AD
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0000BEB6 File Offset: 0x0000A0B6
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		public MatrixFrame Frame { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0000BEC7 File Offset: 0x0000A0C7
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0000BECF File Offset: 0x0000A0CF
		public float Duration { get; private set; }

		// Token: 0x060006D5 RID: 1749 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		public SetMissionObjectFrameOverTime(MissionObjectId missionObjectId, ref MatrixFrame frame, float duration)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
			this.Duration = duration;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0000BEFA File Offset: 0x0000A0FA
		public SetMissionObjectFrameOverTime()
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0000BF04 File Offset: 0x0000A104
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			this.Duration = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagCapturePointDurationCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0000BF40 File Offset: 0x0000A140
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
			GameNetworkMessage.WriteFloatToPacket(this.Duration, CompressionMission.FlagCapturePointDurationCompressionInfo);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0000BF68 File Offset: 0x0000A168
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0000BF70 File Offset: 0x0000A170
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Move-to-frame on MissionObject with ID: ", this.MissionObjectId, " over a period of ", this.Duration, " seconds." });
		}
	}
}
