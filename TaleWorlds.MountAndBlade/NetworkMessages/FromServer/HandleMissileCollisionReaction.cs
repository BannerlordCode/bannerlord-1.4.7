using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008D RID: 141
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class HandleMissileCollisionReaction : GameNetworkMessage
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00009FA2 File Offset: 0x000081A2
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x00009FAA File Offset: 0x000081AA
		public int MissileIndex { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00009FB3 File Offset: 0x000081B3
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x00009FBB File Offset: 0x000081BB
		public Mission.MissileCollisionReaction CollisionReaction { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00009FC4 File Offset: 0x000081C4
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x00009FCC File Offset: 0x000081CC
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00009FD5 File Offset: 0x000081D5
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x00009FDD File Offset: 0x000081DD
		public bool IsAttachedFrameLocal { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00009FE6 File Offset: 0x000081E6
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x00009FEE File Offset: 0x000081EE
		public int AttackerAgentIndex { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00009FF7 File Offset: 0x000081F7
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00009FFF File Offset: 0x000081FF
		public int AttachedAgentIndex { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000A008 File Offset: 0x00008208
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x0000A010 File Offset: 0x00008210
		public bool AttachedToShield { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0000A019 File Offset: 0x00008219
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x0000A021 File Offset: 0x00008221
		public sbyte AttachedBoneIndex { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0000A02A File Offset: 0x0000822A
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x0000A032 File Offset: 0x00008232
		public MissionObjectId AttachedMissionObjectId { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0000A03B File Offset: 0x0000823B
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0000A043 File Offset: 0x00008243
		public Vec3 BounceBackVelocity { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0000A04C File Offset: 0x0000824C
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x0000A054 File Offset: 0x00008254
		public Vec3 BounceBackAngularVelocity { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0000A05D File Offset: 0x0000825D
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x0000A065 File Offset: 0x00008265
		public int ForcedSpawnIndex { get; private set; }

		// Token: 0x0600058E RID: 1422 RVA: 0x0000A070 File Offset: 0x00008270
		public HandleMissileCollisionReaction(int missileIndex, Mission.MissileCollisionReaction collisionReaction, MatrixFrame attachLocalFrame, bool isAttachedFrameLocal, int attackerAgentIndex, int attachedAgentIndex, bool attachedToShield, sbyte attachedBoneIndex, MissionObjectId attachedMissionObjectId, Vec3 bounceBackVelocity, Vec3 bounceBackAngularVelocity, int forcedSpawnIndex)
		{
			this.MissileIndex = missileIndex;
			this.CollisionReaction = collisionReaction;
			this.AttachLocalFrame = attachLocalFrame;
			this.IsAttachedFrameLocal = isAttachedFrameLocal;
			this.AttackerAgentIndex = attackerAgentIndex;
			this.AttachedAgentIndex = attachedAgentIndex;
			this.AttachedToShield = attachedToShield;
			this.AttachedBoneIndex = attachedBoneIndex;
			this.AttachedMissionObjectId = attachedMissionObjectId;
			this.BounceBackVelocity = bounceBackVelocity;
			this.BounceBackAngularVelocity = bounceBackAngularVelocity;
			this.ForcedSpawnIndex = forcedSpawnIndex;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000A0E0 File Offset: 0x000082E0
		public HandleMissileCollisionReaction()
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0000A0E8 File Offset: 0x000082E8
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissileIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissileCompressionInfo, ref flag);
			this.CollisionReaction = (Mission.MissileCollisionReaction)GameNetworkMessage.ReadIntFromPacket(CompressionMission.MissileCollisionReactionCompressionInfo, ref flag);
			this.AttackerAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.AttachedAgentIndex = -1;
			this.AttachedToShield = false;
			this.AttachedBoneIndex = -1;
			this.AttachedMissionObjectId = MissionObjectId.Invalid;
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				if (GameNetworkMessage.ReadBoolFromPacket(ref flag))
				{
					this.AttachedAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
					this.AttachedToShield = GameNetworkMessage.ReadBoolFromPacket(ref flag);
					if (!this.AttachedToShield)
					{
						this.AttachedBoneIndex = (sbyte)GameNetworkMessage.ReadIntFromPacket(CompressionMission.BoneIndexCompressionInfo, ref flag);
					}
				}
				else
				{
					this.AttachedMissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
				}
			}
			if (this.CollisionReaction != Mission.MissileCollisionReaction.BecomeInvisible && this.CollisionReaction != Mission.MissileCollisionReaction.PassThrough)
			{
				this.IsAttachedFrameLocal = GameNetworkMessage.ReadBoolFromPacket(ref flag);
				if (this.IsAttachedFrameLocal)
				{
					this.AttachLocalFrame = GameNetworkMessage.ReadNonUniformTransformFromPacket(CompressionBasic.BigRangeLowResLocalPositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo, ref flag);
				}
				else
				{
					this.AttachLocalFrame = GameNetworkMessage.ReadNonUniformTransformFromPacket(CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo, ref flag);
				}
			}
			else
			{
				this.AttachLocalFrame = MatrixFrame.Identity;
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				this.BounceBackVelocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemVelocityCompressionInfo, ref flag);
				this.BounceBackAngularVelocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemAngularVelocityCompressionInfo, ref flag);
			}
			else
			{
				this.BounceBackVelocity = Vec3.Zero;
				this.BounceBackAngularVelocity = Vec3.Zero;
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				this.ForcedSpawnIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0000A274 File Offset: 0x00008474
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MissileIndex, CompressionMission.MissileCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.CollisionReaction, CompressionMission.MissileCollisionReactionCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AttackerAgentIndex);
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				bool flag = this.AttachedAgentIndex >= 0;
				GameNetworkMessage.WriteBoolToPacket(flag);
				if (flag)
				{
					GameNetworkMessage.WriteAgentIndexToPacket(this.AttachedAgentIndex);
					GameNetworkMessage.WriteBoolToPacket(this.AttachedToShield);
					if (!this.AttachedToShield)
					{
						GameNetworkMessage.WriteIntToPacket((int)this.AttachedBoneIndex, CompressionMission.BoneIndexCompressionInfo);
					}
				}
				else
				{
					GameNetworkMessage.WriteMissionObjectIdToPacket((this.AttachedMissionObjectId.Id >= 0) ? this.AttachedMissionObjectId : MissionObjectId.Invalid);
				}
			}
			if (this.CollisionReaction != Mission.MissileCollisionReaction.BecomeInvisible && this.CollisionReaction != Mission.MissileCollisionReaction.PassThrough)
			{
				GameNetworkMessage.WriteBoolToPacket(this.IsAttachedFrameLocal);
				if (this.IsAttachedFrameLocal)
				{
					GameNetworkMessage.WriteNonUniformTransformToPacket(this.AttachLocalFrame, CompressionBasic.BigRangeLowResLocalPositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo);
				}
				else
				{
					GameNetworkMessage.WriteNonUniformTransformToPacket(this.AttachLocalFrame, CompressionBasic.PositionCompressionInfo, CompressionBasic.LowResQuaternionCompressionInfo);
				}
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				GameNetworkMessage.WriteVec3ToPacket(this.BounceBackVelocity, CompressionMission.SpawnedItemVelocityCompressionInfo);
				GameNetworkMessage.WriteVec3ToPacket(this.BounceBackAngularVelocity, CompressionMission.SpawnedItemAngularVelocityCompressionInfo);
			}
			if (this.CollisionReaction == Mission.MissileCollisionReaction.Stick || this.CollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				GameNetworkMessage.WriteIntToPacket(this.ForcedSpawnIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0000A3BC File Offset: 0x000085BC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0000A3C0 File Offset: 0x000085C0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Handle Missile Collision with index: ",
				this.MissileIndex,
				" collision reaction: ",
				this.CollisionReaction,
				" AttackerAgent index: ",
				this.AttackerAgentIndex,
				" AttachedAgent index: ",
				this.AttachedAgentIndex,
				" AttachedToShield: ",
				this.AttachedToShield.ToString(),
				" AttachedBoneIndex: ",
				this.AttachedBoneIndex,
				" AttachedMissionObject id: ",
				(this.AttachedMissionObjectId != MissionObjectId.Invalid) ? this.AttachedMissionObjectId.Id.ToString() : "-1",
				" ForcedSpawnIndex: ",
				this.ForcedSpawnIndex
			});
		}
	}
}
