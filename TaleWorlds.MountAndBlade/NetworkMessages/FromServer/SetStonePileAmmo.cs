using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BA RID: 186
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetStonePileAmmo : GameNetworkMessage
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0000CE0D File Offset: 0x0000B00D
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x0000CE15 File Offset: 0x0000B015
		public MissionObjectId StonePileId { get; private set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0000CE1E File Offset: 0x0000B01E
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x0000CE26 File Offset: 0x0000B026
		public int AmmoCount { get; private set; }

		// Token: 0x06000779 RID: 1913 RVA: 0x0000CE2F File Offset: 0x0000B02F
		public SetStonePileAmmo(MissionObjectId stonePileId, int ammoCount)
		{
			this.StonePileId = stonePileId;
			this.AmmoCount = ammoCount;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0000CE45 File Offset: 0x0000B045
		public SetStonePileAmmo()
		{
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0000CE50 File Offset: 0x0000B050
		protected override bool OnRead()
		{
			bool flag = true;
			this.StonePileId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.AmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0000CE7F File Offset: 0x0000B07F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.StonePileId);
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000CE9C File Offset: 0x0000B09C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ammo left to: ", this.AmmoCount, " on StonePile with ID: ", this.StonePileId });
		}
	}
}
