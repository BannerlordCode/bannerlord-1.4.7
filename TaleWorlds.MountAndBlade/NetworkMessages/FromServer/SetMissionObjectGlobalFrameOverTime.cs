using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AC RID: 172
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectGlobalFrameOverTime : GameNetworkMessage
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0000C178 File Offset: 0x0000A378
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0000C180 File Offset: 0x0000A380
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0000C189 File Offset: 0x0000A389
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x0000C191 File Offset: 0x0000A391
		public MatrixFrame Frame { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0000C19A File Offset: 0x0000A39A
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0000C1A2 File Offset: 0x0000A3A2
		public float Duration { get; private set; }

		// Token: 0x060006EB RID: 1771 RVA: 0x0000C1AB File Offset: 0x0000A3AB
		public SetMissionObjectGlobalFrameOverTime(MissionObjectId missionObjectId, ref MatrixFrame frame, float duration)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
			this.Duration = duration;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0000C1CD File Offset: 0x0000A3CD
		public SetMissionObjectGlobalFrameOverTime()
		{
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			Vec3 vec2 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			Vec3 vec3 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			Vec3 vec4 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ScaleCompressionInfo, ref flag);
			Vec3 vec5 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			if (flag)
			{
				Mat3 mat = new Mat3(in vec, in vec2, in vec3);
				this.Frame = new MatrixFrame(in mat, in vec5);
				this.Frame.Scale(in vec4);
			}
			this.Duration = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagCapturePointDurationCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0000C27C File Offset: 0x0000A47C
		protected override void OnWrite()
		{
			Vec3 scaleVector = this.Frame.rotation.GetScaleVector();
			MatrixFrame frame = this.Frame;
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			frame.Scale(in vec);
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteVec3ToPacket(frame.rotation.f, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(frame.rotation.s, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(frame.rotation.u, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(scaleVector, CompressionBasic.ScaleCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(frame.origin, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Duration, CompressionMission.FlagCapturePointDurationCompressionInfo);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0000C351 File Offset: 0x0000A551
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0000C35C File Offset: 0x0000A55C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Move-to-global-frame on MissionObject with ID: ", this.MissionObjectId, " over a period of ", this.Duration, " seconds." });
		}
	}
}
