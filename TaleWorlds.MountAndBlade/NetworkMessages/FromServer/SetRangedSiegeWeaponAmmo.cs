using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B3 RID: 179
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetRangedSiegeWeaponAmmo : GameNetworkMessage
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0000C911 File Offset: 0x0000AB11
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0000C919 File Offset: 0x0000AB19
		public MissionObjectId RangedSiegeWeaponId { get; private set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0000C922 File Offset: 0x0000AB22
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0000C92A File Offset: 0x0000AB2A
		public int AmmoCount { get; private set; }

		// Token: 0x06000737 RID: 1847 RVA: 0x0000C933 File Offset: 0x0000AB33
		public SetRangedSiegeWeaponAmmo(MissionObjectId rangedSiegeWeaponId, int ammoCount)
		{
			this.RangedSiegeWeaponId = rangedSiegeWeaponId;
			this.AmmoCount = ammoCount;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0000C949 File Offset: 0x0000AB49
		public SetRangedSiegeWeaponAmmo()
		{
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000C954 File Offset: 0x0000AB54
		protected override bool OnRead()
		{
			bool flag = true;
			this.RangedSiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.AmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0000C983 File Offset: 0x0000AB83
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.RangedSiegeWeaponId);
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ammo left to: ", this.AmmoCount, " on RangedSiegeWeapon with ID: ", this.RangedSiegeWeaponId });
		}
	}
}
