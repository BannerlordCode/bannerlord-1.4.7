using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000078 RID: 120
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AttachWeaponToSpawnedWeapon : GameNetworkMessage
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000080ED File Offset: 0x000062ED
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x000080F5 File Offset: 0x000062F5
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000080FE File Offset: 0x000062FE
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00008106 File Offset: 0x00006306
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000810F File Offset: 0x0000630F
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00008117 File Offset: 0x00006317
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x06000452 RID: 1106 RVA: 0x00008120 File Offset: 0x00006320
		public AttachWeaponToSpawnedWeapon(MissionWeapon weapon, MissionObjectId missionObjectId, MatrixFrame attachLocalFrame)
		{
			this.Weapon = weapon;
			this.MissionObjectId = missionObjectId;
			this.AttachLocalFrame = attachLocalFrame;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000813D File Offset: 0x0000633D
		public AttachWeaponToSpawnedWeapon()
		{
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00008145 File Offset: 0x00006345
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteVec3ToPacket(this.AttachLocalFrame.origin, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(this.AttachLocalFrame.rotation);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00008184 File Offset: 0x00006384
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
			if (flag)
			{
				this.AttachLocalFrame = new MatrixFrame(in mat, in vec);
			}
			return flag;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000081DA File Offset: 0x000063DA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000081E4 File Offset: 0x000063E4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"AttachWeaponToSpawnedWeapon with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" to MissionObject: ",
				this.MissionObjectId
			});
		}
	}
}
