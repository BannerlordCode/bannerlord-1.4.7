using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C2 RID: 194
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnAttachedWeaponOnSpawnedWeapon : GameNetworkMessage
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0000D6F3 File Offset: 0x0000B8F3
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0000D6FB File Offset: 0x0000B8FB
		public MissionObjectId SpawnedWeaponId { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0000D704 File Offset: 0x0000B904
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0000D70C File Offset: 0x0000B90C
		public int AttachmentIndex { get; private set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0000D715 File Offset: 0x0000B915
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0000D71D File Offset: 0x0000B91D
		public int ForcedIndex { get; private set; }

		// Token: 0x060007DD RID: 2013 RVA: 0x0000D726 File Offset: 0x0000B926
		public SpawnAttachedWeaponOnSpawnedWeapon(MissionObjectId spawnedWeaponId, int attachmentIndex, int forcedIndex)
		{
			this.SpawnedWeaponId = spawnedWeaponId;
			this.AttachmentIndex = attachmentIndex;
			this.ForcedIndex = forcedIndex;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0000D743 File Offset: 0x0000B943
		public SpawnAttachedWeaponOnSpawnedWeapon()
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0000D74C File Offset: 0x0000B94C
		protected override bool OnRead()
		{
			bool flag = true;
			this.SpawnedWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.AttachmentIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponAttachmentIndexCompressionInfo, ref flag);
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0000D78D File Offset: 0x0000B98D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SpawnedWeaponId);
			GameNetworkMessage.WriteIntToPacket(this.AttachmentIndex, CompressionMission.WeaponAttachmentIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0000D7BA File Offset: 0x0000B9BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "SpawnAttachedWeaponOnSpawnedWeapon with Spawned Weapon ID: ", this.SpawnedWeaponId, " AttachmentIndex: ", this.AttachmentIndex, " Attached Weapon ID: ", this.ForcedIndex });
		}
	}
}
