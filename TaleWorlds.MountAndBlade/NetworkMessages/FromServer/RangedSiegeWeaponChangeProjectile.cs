using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000092 RID: 146
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RangedSiegeWeaponChangeProjectile : GameNetworkMessage
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0000A8B2 File Offset: 0x00008AB2
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0000A8BA File Offset: 0x00008ABA
		public MissionObjectId RangedSiegeWeaponId { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0000A8C3 File Offset: 0x00008AC3
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0000A8CB File Offset: 0x00008ACB
		public int Index { get; private set; }

		// Token: 0x060005CC RID: 1484 RVA: 0x0000A8D4 File Offset: 0x00008AD4
		public RangedSiegeWeaponChangeProjectile(MissionObjectId rangedSiegeWeaponId, int index)
		{
			this.RangedSiegeWeaponId = rangedSiegeWeaponId;
			this.Index = index;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000A8EA File Offset: 0x00008AEA
		public RangedSiegeWeaponChangeProjectile()
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		protected override bool OnRead()
		{
			bool flag = true;
			this.RangedSiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Index = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000A923 File Offset: 0x00008B23
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.RangedSiegeWeaponId);
			GameNetworkMessage.WriteIntToPacket(this.Index, CompressionMission.RangedSiegeWeaponAmmoIndexCompressionInfo);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000A940 File Offset: 0x00008B40
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000A948 File Offset: 0x00008B48
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Changed Projectile Type Index to: ", this.Index, " on RangedSiegeWeapon with ID: ", this.RangedSiegeWeaponId });
		}
	}
}
