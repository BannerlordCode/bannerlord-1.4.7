using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BD RID: 189
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWeaponAmmoData : GameNetworkMessage
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0000D095 File Offset: 0x0000B295
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0000D09D File Offset: 0x0000B29D
		public int AgentIndex { get; private set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0000D0A6 File Offset: 0x0000B2A6
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0000D0AE File Offset: 0x0000B2AE
		public EquipmentIndex WeaponEquipmentIndex { get; private set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0000D0B7 File Offset: 0x0000B2B7
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x0000D0BF File Offset: 0x0000B2BF
		public EquipmentIndex AmmoEquipmentIndex { get; private set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x0000D0D0 File Offset: 0x0000B2D0
		public short Ammo { get; private set; }

		// Token: 0x0600079B RID: 1947 RVA: 0x0000D0D9 File Offset: 0x0000B2D9
		public SetWeaponAmmoData(int agentIndex, EquipmentIndex weaponEquipmentIndex, EquipmentIndex ammoEquipmentIndex, short ammo)
		{
			this.AgentIndex = agentIndex;
			this.WeaponEquipmentIndex = weaponEquipmentIndex;
			this.AmmoEquipmentIndex = ammoEquipmentIndex;
			this.Ammo = ammo;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000D0FE File Offset: 0x0000B2FE
		public SetWeaponAmmoData()
		{
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0000D108 File Offset: 0x0000B308
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.WeaponEquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.AmmoEquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			this.Ammo = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemDataCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0000D15C File Offset: 0x0000B35C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponEquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.AmmoEquipmentIndex, CompressionMission.WieldSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.Ammo, CompressionMission.ItemDataCompressionInfo);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0000D199 File Offset: 0x0000B399
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ammo: ", this.Ammo, " for weapon with EquipmentIndex: ", this.WeaponEquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
