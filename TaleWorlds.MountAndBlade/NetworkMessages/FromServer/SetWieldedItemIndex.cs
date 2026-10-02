using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C0 RID: 192
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetWieldedItemIndex : GameNetworkMessage
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0000D44A File Offset: 0x0000B64A
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x0000D452 File Offset: 0x0000B652
		public int AgentIndex { get; private set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0000D45B File Offset: 0x0000B65B
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0000D463 File Offset: 0x0000B663
		public bool IsLeftHand { get; private set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000D46C File Offset: 0x0000B66C
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0000D474 File Offset: 0x0000B674
		public bool IsWieldedInstantly { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0000D47D File Offset: 0x0000B67D
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0000D485 File Offset: 0x0000B685
		public bool IsWieldedOnSpawn { get; private set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0000D48E File Offset: 0x0000B68E
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0000D496 File Offset: 0x0000B696
		public EquipmentIndex WieldedItemIndex { get; private set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0000D49F File Offset: 0x0000B69F
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x0000D4A7 File Offset: 0x0000B6A7
		public int MainHandCurrentUsageIndex { get; private set; }

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		public SetWieldedItemIndex(int agentIndex, bool isLeftHand, bool isWieldedInstantly, bool isWieldedOnSpawn, EquipmentIndex wieldedItemIndex, int mainHandCurUsageIndex)
		{
			this.AgentIndex = agentIndex;
			this.IsLeftHand = isLeftHand;
			this.IsWieldedInstantly = isWieldedInstantly;
			this.IsWieldedOnSpawn = isWieldedOnSpawn;
			this.WieldedItemIndex = wieldedItemIndex;
			this.MainHandCurrentUsageIndex = mainHandCurUsageIndex;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0000D4E5 File Offset: 0x0000B6E5
		public SetWieldedItemIndex()
		{
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0000D4F0 File Offset: 0x0000B6F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsLeftHand = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsWieldedInstantly = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsWieldedOnSpawn = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.WieldedItemIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			this.MainHandCurrentUsageIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0000D558 File Offset: 0x0000B758
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsLeftHand);
			GameNetworkMessage.WriteBoolToPacket(this.IsWieldedInstantly);
			GameNetworkMessage.WriteBoolToPacket(this.IsWieldedOnSpawn);
			GameNetworkMessage.WriteIntToPacket((int)this.WieldedItemIndex, CompressionMission.WieldSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.MainHandCurrentUsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0000D5B1 File Offset: 0x0000B7B1
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0000D5B6 File Offset: 0x0000B7B6
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Wielded Item Index to: ", this.WieldedItemIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
