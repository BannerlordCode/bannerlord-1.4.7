using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004C RID: 76
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class EquipEquipmentToPeer : GameNetworkMessage
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000283 RID: 643 RVA: 0x000051C6 File Offset: 0x000033C6
		// (set) Token: 0x06000284 RID: 644 RVA: 0x000051CE File Offset: 0x000033CE
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000285 RID: 645 RVA: 0x000051D7 File Offset: 0x000033D7
		// (set) Token: 0x06000286 RID: 646 RVA: 0x000051DF File Offset: 0x000033DF
		public Equipment Equipment { get; private set; }

		// Token: 0x06000287 RID: 647 RVA: 0x000051E8 File Offset: 0x000033E8
		public EquipEquipmentToPeer(NetworkCommunicator peer, Equipment equipment)
		{
			this.Peer = peer;
			this.Equipment = new Equipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				this.Equipment[equipmentIndex] = equipment.GetEquipmentFromSlot(equipmentIndex);
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000522D File Offset: 0x0000342D
		public EquipEquipmentToPeer()
		{
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00005238 File Offset: 0x00003438
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			if (flag)
			{
				this.Equipment = new Equipment();
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
				{
					if (flag)
					{
						this.Equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag));
					}
				}
			}
			return flag;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000528C File Offset: 0x0000348C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.Equipment.GetEquipmentFromSlot(equipmentIndex));
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000052C2 File Offset: 0x000034C2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000052C7 File Offset: 0x000034C7
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Equip equipment to peer: ",
				this.Peer.UserName,
				" with peer-index:",
				this.Peer.Index
			});
		}
	}
}
