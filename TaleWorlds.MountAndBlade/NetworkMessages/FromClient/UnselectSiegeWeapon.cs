using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003B RID: 59
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class UnselectSiegeWeapon : GameNetworkMessage
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00004348 File Offset: 0x00002548
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00004350 File Offset: 0x00002550
		public MissionObjectId SiegeWeaponId { get; private set; }

		// Token: 0x060001D5 RID: 469 RVA: 0x00004359 File Offset: 0x00002559
		public UnselectSiegeWeapon(MissionObjectId siegeWeaponId)
		{
			this.SiegeWeaponId = siegeWeaponId;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00004368 File Offset: 0x00002568
		public UnselectSiegeWeapon()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00004370 File Offset: 0x00002570
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeWeaponId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000438D File Offset: 0x0000258D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeWeaponId);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000439A File Offset: 0x0000259A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000043A2 File Offset: 0x000025A2
		protected override string OnGetLogFormat()
		{
			return "Deselect SiegeWeapon with ID: " + this.SiegeWeaponId;
		}
	}
}
