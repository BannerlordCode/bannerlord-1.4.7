using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B2 RID: 178
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetRangedSiegeWeaponState : GameNetworkMessage
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0000C843 File Offset: 0x0000AA43
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0000C84B File Offset: 0x0000AA4B
		public MissionObjectId RangedSiegeWeaponId { get; private set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0000C854 File Offset: 0x0000AA54
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0000C85C File Offset: 0x0000AA5C
		public RangedSiegeWeapon.WeaponState State { get; private set; }

		// Token: 0x0600072D RID: 1837 RVA: 0x0000C865 File Offset: 0x0000AA65
		public SetRangedSiegeWeaponState(MissionObjectId rangedSiegeWeaponId, RangedSiegeWeapon.WeaponState state)
		{
			this.RangedSiegeWeaponId = rangedSiegeWeaponId;
			this.State = state;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000C87B File Offset: 0x0000AA7B
		public SetRangedSiegeWeaponState()
		{
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000C884 File Offset: 0x0000AA84
		protected override bool OnRead()
		{
			bool flag = true;
			this.RangedSiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.State = (RangedSiegeWeapon.WeaponState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000C8B3 File Offset: 0x0000AAB3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.RangedSiegeWeaponId);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.RangedSiegeWeaponStateCompressionInfo);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set RangedSiegeWeapon State to: ", this.State, " on RangedSiegeWeapon with ID: ", this.RangedSiegeWeaponId });
		}
	}
}
