using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000080 RID: 128
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ConsumeWeaponAmount : GameNetworkMessage
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00008AED File Offset: 0x00006CED
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00008AF5 File Offset: 0x00006CF5
		public MissionObjectId SpawnedItemEntityId { get; private set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00008AFE File Offset: 0x00006CFE
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00008B06 File Offset: 0x00006D06
		public short ConsumedAmount { get; private set; }

		// Token: 0x060004BB RID: 1211 RVA: 0x00008B0F File Offset: 0x00006D0F
		public ConsumeWeaponAmount(MissionObjectId spawnedItemEntityId, short consumedAmount)
		{
			this.SpawnedItemEntityId = spawnedItemEntityId;
			this.ConsumedAmount = consumedAmount;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00008B25 File Offset: 0x00006D25
		public ConsumeWeaponAmount()
		{
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00008B2D File Offset: 0x00006D2D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SpawnedItemEntityId);
			GameNetworkMessage.WriteIntToPacket((int)this.ConsumedAmount, CompressionBasic.ItemDataValueCompressionInfo);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00008B4C File Offset: 0x00006D4C
		protected override bool OnRead()
		{
			bool flag = true;
			this.SpawnedItemEntityId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.ConsumedAmount = (short)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ItemDataValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00008B7C File Offset: 0x00006D7C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00008B81 File Offset: 0x00006D81
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Consumed ", this.ConsumedAmount, " from ", this.SpawnedItemEntityId });
		}
	}
}
