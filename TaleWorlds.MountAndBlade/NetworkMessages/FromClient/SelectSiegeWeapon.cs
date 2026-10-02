using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000035 RID: 53
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectSiegeWeapon : GameNetworkMessage
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00003FE0 File Offset: 0x000021E0
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00003FE8 File Offset: 0x000021E8
		public MissionObjectId SiegeWeaponId { get; private set; }

		// Token: 0x0600019F RID: 415 RVA: 0x00003FF1 File Offset: 0x000021F1
		public SelectSiegeWeapon(MissionObjectId siegeWeaponId)
		{
			this.SiegeWeaponId = siegeWeaponId;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00004000 File Offset: 0x00002200
		public SelectSiegeWeapon()
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00004008 File Offset: 0x00002208
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00004025 File Offset: 0x00002225
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeWeaponId);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004032 File Offset: 0x00002232
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000403A File Offset: 0x0000223A
		protected override string OnGetLogFormat()
		{
			return "Select SiegeWeapon with ID: " + this.SiegeWeaponId;
		}
	}
}
