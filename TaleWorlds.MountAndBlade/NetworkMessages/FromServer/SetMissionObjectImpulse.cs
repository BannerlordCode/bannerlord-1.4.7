using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AD RID: 173
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectImpulse : GameNetworkMessage
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0000C3A8 File Offset: 0x0000A5A8
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0000C3B9 File Offset: 0x0000A5B9
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0000C3C1 File Offset: 0x0000A5C1
		public Vec3 Position { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0000C3CA File Offset: 0x0000A5CA
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0000C3D2 File Offset: 0x0000A5D2
		public Vec3 Impulse { get; private set; }

		// Token: 0x060006F7 RID: 1783 RVA: 0x0000C3DB File Offset: 0x0000A5DB
		public SetMissionObjectImpulse(MissionObjectId missionObjectId, Vec3 position, Vec3 impulse)
		{
			this.MissionObjectId = missionObjectId;
			this.Position = position;
			this.Impulse = impulse;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public SetMissionObjectImpulse()
		{
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0000C400 File Offset: 0x0000A600
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			this.Impulse = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ImpulseCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0000C441 File Offset: 0x0000A641
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Impulse, CompressionBasic.ImpulseCompressionInfo);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0000C46E File Offset: 0x0000A66E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0000C476 File Offset: 0x0000A676
		protected override string OnGetLogFormat()
		{
			return "Set impulse on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
