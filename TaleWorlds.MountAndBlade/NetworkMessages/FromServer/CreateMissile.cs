using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000085 RID: 133
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateMissile : GameNetworkMessage
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x000096AD File Offset: 0x000078AD
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x000096B5 File Offset: 0x000078B5
		public int MissileIndex { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000096BE File Offset: 0x000078BE
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x000096C6 File Offset: 0x000078C6
		public int AgentIndex { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x000096CF File Offset: 0x000078CF
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x000096D7 File Offset: 0x000078D7
		public EquipmentIndex WeaponIndex { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x000096E0 File Offset: 0x000078E0
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x000096E8 File Offset: 0x000078E8
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x000096F1 File Offset: 0x000078F1
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x000096F9 File Offset: 0x000078F9
		public Vec3 Position { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00009702 File Offset: 0x00007902
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x0000970A File Offset: 0x0000790A
		public Vec3 Direction { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00009713 File Offset: 0x00007913
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x0000971B File Offset: 0x0000791B
		public float Speed { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00009724 File Offset: 0x00007924
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x0000972C File Offset: 0x0000792C
		public Mat3 Orientation { get; private set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00009735 File Offset: 0x00007935
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x0000973D File Offset: 0x0000793D
		public bool HasRigidBody { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00009746 File Offset: 0x00007946
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x0000974E File Offset: 0x0000794E
		public MissionObjectId MissionObjectToIgnoreId { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00009757 File Offset: 0x00007957
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x0000975F File Offset: 0x0000795F
		public bool IsPrimaryWeaponShot { get; private set; }

		// Token: 0x0600052F RID: 1327 RVA: 0x00009768 File Offset: 0x00007968
		public CreateMissile(int missileIndex, int agentIndex, EquipmentIndex weaponIndex, MissionWeapon weapon, Vec3 position, Vec3 direction, float speed, Mat3 orientation, bool hasRigidBody, MissionObjectId missionObjectToIgnoreId, bool isPrimaryWeaponShot)
		{
			this.MissileIndex = missileIndex;
			this.AgentIndex = agentIndex;
			this.WeaponIndex = weaponIndex;
			this.Weapon = weapon;
			this.Position = position;
			this.Direction = direction;
			this.Speed = speed;
			this.Orientation = orientation;
			this.HasRigidBody = hasRigidBody;
			this.MissionObjectToIgnoreId = missionObjectToIgnoreId;
			this.IsPrimaryWeaponShot = isPrimaryWeaponShot;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000097D0 File Offset: 0x000079D0
		public CreateMissile()
		{
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000097D8 File Offset: 0x000079D8
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissileIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissileCompressionInfo, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.WeaponIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			if (this.WeaponIndex == EquipmentIndex.None)
			{
				this.Weapon = ModuleNetworkData.ReadMissileWeaponReferenceFromPacket(Game.Current.ObjectManager, ref flag);
			}
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.MissileSpeedCompressionInfo, ref flag);
			this.HasRigidBody = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (this.HasRigidBody)
			{
				this.Orientation = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
				this.MissionObjectToIgnoreId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			}
			else
			{
				Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
				this.Orientation = new Mat3(in Vec3.Side, in vec, in Vec3.Up);
				this.Orientation.Orthonormalize();
				this.MissionObjectToIgnoreId = MissionObjectId.Invalid;
			}
			this.IsPrimaryWeaponShot = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000098EC File Offset: 0x00007AEC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MissileIndex, CompressionMission.MissileCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponIndex, CompressionMission.WieldSlotCompressionInfo);
			if (this.WeaponIndex == EquipmentIndex.None)
			{
				ModuleNetworkData.WriteMissileWeaponReferenceToPacket(this.Weapon);
			}
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionMission.MissileSpeedCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.HasRigidBody);
			if (this.HasRigidBody)
			{
				GameNetworkMessage.WriteRotationMatrixToPacket(this.Orientation);
				GameNetworkMessage.WriteMissionObjectIdToPacket((this.MissionObjectToIgnoreId.Id >= 0) ? this.MissionObjectToIgnoreId : MissionObjectId.Invalid);
			}
			else
			{
				GameNetworkMessage.WriteVec3ToPacket(this.Orientation.f, CompressionBasic.UnitVectorCompressionInfo);
			}
			GameNetworkMessage.WriteBoolToPacket(this.IsPrimaryWeaponShot);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000099C8 File Offset: 0x00007BC8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000099D0 File Offset: 0x00007BD0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Create a missile with index: ", this.MissileIndex, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
