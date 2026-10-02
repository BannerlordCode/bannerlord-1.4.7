using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C9 RID: 201
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizeMissionObject : GameNetworkMessage
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0000E0C1 File Offset: 0x0000C2C1
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0000E0C9 File Offset: 0x0000C2C9
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0000E0D2 File Offset: 0x0000C2D2
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x0000E0DA File Offset: 0x0000C2DA
		public int RecordTypeIndex { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0000E0E3 File Offset: 0x0000C2E3
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x0000E0EB File Offset: 0x0000C2EB
		public ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> RecordPair { get; private set; }

		// Token: 0x0600083F RID: 2111 RVA: 0x0000E0F4 File Offset: 0x0000C2F4
		public SynchronizeMissionObject(SynchedMissionObject synchedMissionObject)
		{
			this._synchedMissionObject = synchedMissionObject;
			this.MissionObjectId = synchedMissionObject.Id;
			this.RecordTypeIndex = GameNetwork.GetSynchedMissionObjectReadableRecordIndexFromType(synchedMissionObject.GetType());
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0000E120 File Offset: 0x0000C320
		public SynchronizeMissionObject()
		{
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0000E128 File Offset: 0x0000C328
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.RecordTypeIndex, CompressionMission.SynchedMissionObjectReadableRecordTypeIndex);
			this._synchedMissionObject.WriteToNetwork();
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000E150 File Offset: 0x0000C350
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.RecordTypeIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SynchedMissionObjectReadableRecordTypeIndex, ref flag);
			this.RecordPair = BaseSynchedMissionObjectReadableRecord.CreateFromNetworkWithTypeIndex(this.RecordTypeIndex);
			return flag;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0000E190 File Offset: 0x0000C390
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0000E198 File Offset: 0x0000C398
		protected override string OnGetLogFormat()
		{
			return "Synchronize MissionObject with Id: " + this.MissionObjectId;
		}

		// Token: 0x040001DF RID: 479
		private SynchedMissionObject _synchedMissionObject;
	}
}
