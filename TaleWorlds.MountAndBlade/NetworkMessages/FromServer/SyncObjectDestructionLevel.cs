using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CA RID: 202
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncObjectDestructionLevel : GameNetworkMessage
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0000E1AF File Offset: 0x0000C3AF
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x0000E1B7 File Offset: 0x0000C3B7
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0000E1C0 File Offset: 0x0000C3C0
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
		public int DestructionLevel { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0000E1D1 File Offset: 0x0000C3D1
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0000E1D9 File Offset: 0x0000C3D9
		public int ForcedIndex { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0000E1E2 File Offset: 0x0000C3E2
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0000E1EA File Offset: 0x0000C3EA
		public float BlowMagnitude { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0000E1F3 File Offset: 0x0000C3F3
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0000E1FB File Offset: 0x0000C3FB
		public Vec3 BlowPosition { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0000E204 File Offset: 0x0000C404
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0000E20C File Offset: 0x0000C40C
		public Vec3 BlowDirection { get; private set; }

		// Token: 0x06000851 RID: 2129 RVA: 0x0000E215 File Offset: 0x0000C415
		public SyncObjectDestructionLevel(MissionObjectId missionObjectId, int destructionLevel, int forcedIndex, float blowMagnitude, Vec3 blowPosition, Vec3 blowDirection)
		{
			this.MissionObjectId = missionObjectId;
			this.DestructionLevel = destructionLevel;
			this.ForcedIndex = forcedIndex;
			this.BlowMagnitude = blowMagnitude;
			this.BlowPosition = blowPosition;
			this.BlowDirection = blowDirection;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0000E24A File Offset: 0x0000C44A
		public SyncObjectDestructionLevel()
		{
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0000E254 File Offset: 0x0000C454
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.DestructionLevel = GameNetworkMessage.ReadIntFromPacket(CompressionMission.UsableGameObjectDestructionStateCompressionInfo, ref flag);
			this.ForcedIndex = (GameNetworkMessage.ReadBoolFromPacket(ref flag) ? GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag) : (-1));
			this.BlowMagnitude = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.UsableGameObjectBlowMagnitude, ref flag);
			this.BlowPosition = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.BlowDirection = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.UsableGameObjectBlowDirection, ref flag);
			return flag;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.DestructionLevel, CompressionMission.UsableGameObjectDestructionStateCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.ForcedIndex != -1);
			if (this.ForcedIndex != -1)
			{
				GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			}
			GameNetworkMessage.WriteFloatToPacket(this.BlowMagnitude, CompressionMission.UsableGameObjectBlowMagnitude);
			GameNetworkMessage.WriteVec3ToPacket(this.BlowPosition, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.BlowDirection, CompressionMission.UsableGameObjectBlowDirection);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0000E35A File Offset: 0x0000C55A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0000E364 File Offset: 0x0000C564
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Synchronize DestructionLevel: ",
				this.DestructionLevel,
				" of MissionObject with Id: ",
				this.MissionObjectId,
				(this.ForcedIndex != -1) ? (" (New object will have ID: " + this.ForcedIndex + ")") : ""
			});
		}
	}
}
