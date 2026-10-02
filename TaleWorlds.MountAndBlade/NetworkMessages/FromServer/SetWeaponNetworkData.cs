using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BE RID: 190
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWeaponNetworkData : GameNetworkMessage
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0000D1FA File Offset: 0x0000B3FA
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x0000D202 File Offset: 0x0000B402
		public int AgentIndex { get; private set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0000D20B File Offset: 0x0000B40B
		// (set) Token: 0x060007A4 RID: 1956 RVA: 0x0000D213 File Offset: 0x0000B413
		public EquipmentIndex WeaponEquipmentIndex { get; private set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0000D21C File Offset: 0x0000B41C
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x0000D224 File Offset: 0x0000B424
		public short DataValue { get; private set; }

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000D22D File Offset: 0x0000B42D
		public SetWeaponNetworkData(int agent, EquipmentIndex weaponEquipmentIndex, short dataValue)
		{
			this.AgentIndex = agent;
			this.WeaponEquipmentIndex = weaponEquipmentIndex;
			this.DataValue = dataValue;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0000D24A File Offset: 0x0000B44A
		public SetWeaponNetworkData()
		{
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0000D254 File Offset: 0x0000B454
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.WeaponEquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.DataValue = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemDataCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0000D296 File Offset: 0x0000B496
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponEquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.DataValue, CompressionMission.ItemDataCompressionInfo);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0000D2C3 File Offset: 0x0000B4C3
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Network data: ", this.DataValue, " for weapon with EquipmentIndex: ", this.WeaponEquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
