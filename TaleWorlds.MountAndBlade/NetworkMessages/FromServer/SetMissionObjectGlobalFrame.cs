using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AB RID: 171
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectGlobalFrame : GameNetworkMessage
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0000BFBC File Offset: 0x0000A1BC
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0000BFCD File Offset: 0x0000A1CD
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		public MatrixFrame Frame { get; private set; }

		// Token: 0x060006DF RID: 1759 RVA: 0x0000BFDE File Offset: 0x0000A1DE
		public SetMissionObjectGlobalFrame(MissionObjectId missionObjectId, ref MatrixFrame frame)
		{
			this.MissionObjectId = missionObjectId;
			this.Frame = frame;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0000BFF9 File Offset: 0x0000A1F9
		public SetMissionObjectGlobalFrame()
		{
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0000C004 File Offset: 0x0000A204
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
			return flag;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0000C094 File Offset: 0x0000A294
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
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0000C159 File Offset: 0x0000A359
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0000C161 File Offset: 0x0000A361
		protected override string OnGetLogFormat()
		{
			return "Set Global Frame on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
