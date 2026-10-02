using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BF RID: 191
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWeaponReloadPhase : GameNetworkMessage
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0000D322 File Offset: 0x0000B522
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0000D32A File Offset: 0x0000B52A
		public int AgentIndex { get; private set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0000D333 File Offset: 0x0000B533
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0000D33B File Offset: 0x0000B53B
		public EquipmentIndex EquipmentIndex { get; private set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0000D344 File Offset: 0x0000B544
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0000D34C File Offset: 0x0000B54C
		public short ReloadPhase { get; private set; }

		// Token: 0x060007B3 RID: 1971 RVA: 0x0000D355 File Offset: 0x0000B555
		public SetWeaponReloadPhase(int agentIndex, EquipmentIndex equipmentIndex, short reloadPhase)
		{
			this.AgentIndex = agentIndex;
			this.EquipmentIndex = equipmentIndex;
			this.ReloadPhase = reloadPhase;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0000D372 File Offset: 0x0000B572
		public SetWeaponReloadPhase()
		{
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0000D37C File Offset: 0x0000B57C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.EquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.ReloadPhase = (short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponReloadPhaseCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0000D3BE File Offset: 0x0000B5BE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.EquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.ReloadPhase, CompressionMission.WeaponReloadPhaseCompressionInfo);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000D3EB File Offset: 0x0000B5EB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Reload Phase: ", this.ReloadPhase, " for weapon with EquipmentIndex: ", this.EquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
