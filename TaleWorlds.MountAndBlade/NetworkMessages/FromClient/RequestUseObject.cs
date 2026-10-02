using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000031 RID: 49
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestUseObject : GameNetworkMessage
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00003E87 File Offset: 0x00002087
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00003E8F File Offset: 0x0000208F
		public MissionObjectId UsableMissionObjectId { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00003E98 File Offset: 0x00002098
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00003EA0 File Offset: 0x000020A0
		public int UsedObjectPreferenceIndex { get; private set; }

		// Token: 0x06000185 RID: 389 RVA: 0x00003EA9 File Offset: 0x000020A9
		public RequestUseObject(MissionObjectId usableMissionObjectId, int usedObjectPreferenceIndex)
		{
			this.UsableMissionObjectId = usableMissionObjectId;
			this.UsedObjectPreferenceIndex = usedObjectPreferenceIndex;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00003EBF File Offset: 0x000020BF
		public RequestUseObject()
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00003EC8 File Offset: 0x000020C8
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.UsedObjectPreferenceIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00003EF7 File Offset: 0x000020F7
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.UsedObjectPreferenceIndex, CompressionMission.WieldSlotCompressionInfo);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00003F14 File Offset: 0x00002114
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00003F1C File Offset: 0x0000211C
		protected override string OnGetLogFormat()
		{
			return "Request to use UsableMissionObject with ID: " + this.UsableMissionObjectId;
		}
	}
}
