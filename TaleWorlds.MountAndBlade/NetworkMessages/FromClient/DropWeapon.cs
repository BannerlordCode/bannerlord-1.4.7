using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200002D RID: 45
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class DropWeapon : GameNetworkMessage
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00003CFD File Offset: 0x00001EFD
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00003D05 File Offset: 0x00001F05
		public bool IsDefendPressed { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00003D0E File Offset: 0x00001F0E
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00003D16 File Offset: 0x00001F16
		public EquipmentIndex ForcedSlotIndexToDropWeaponFrom { get; private set; }

		// Token: 0x06000169 RID: 361 RVA: 0x00003D1F File Offset: 0x00001F1F
		public DropWeapon(bool isDefendPressed, EquipmentIndex forcedSlotIndexToDropWeaponFrom)
		{
			this.IsDefendPressed = isDefendPressed;
			this.ForcedSlotIndexToDropWeaponFrom = forcedSlotIndexToDropWeaponFrom;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003D35 File Offset: 0x00001F35
		public DropWeapon()
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003D40 File Offset: 0x00001F40
		protected override bool OnRead()
		{
			bool flag = true;
			this.IsDefendPressed = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.ForcedSlotIndexToDropWeaponFrom = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00003D6F File Offset: 0x00001F6F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.IsDefendPressed);
			GameNetworkMessage.WriteIntToPacket((int)this.ForcedSlotIndexToDropWeaponFrom, CompressionMission.WieldSlotCompressionInfo);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00003D8C File Offset: 0x00001F8C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003D90 File Offset: 0x00001F90
		protected override string OnGetLogFormat()
		{
			bool flag = this.ForcedSlotIndexToDropWeaponFrom != EquipmentIndex.None;
			return "Dropping " + ((!flag) ? "equipped" : "") + " weapon" + (flag ? (" " + (int)this.ForcedSlotIndexToDropWeaponFrom) : "");
		}
	}
}
