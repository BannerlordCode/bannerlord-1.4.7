using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000070 RID: 112
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddMissionObjectBodyFlags : GameNetworkMessage
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00007774 File Offset: 0x00005974
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000777C File Offset: 0x0000597C
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00007785 File Offset: 0x00005985
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000778D File Offset: 0x0000598D
		public BodyFlags BodyFlags { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00007796 File Offset: 0x00005996
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0000779E File Offset: 0x0000599E
		public bool ApplyToChildren { get; private set; }

		// Token: 0x060003EE RID: 1006 RVA: 0x000077A7 File Offset: 0x000059A7
		public AddMissionObjectBodyFlags(MissionObjectId missionObjectId, BodyFlags bodyFlags, bool applyToChildren)
		{
			this.MissionObjectId = missionObjectId;
			this.BodyFlags = bodyFlags;
			this.ApplyToChildren = applyToChildren;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000077C4 File Offset: 0x000059C4
		public AddMissionObjectBodyFlags()
		{
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000077CC File Offset: 0x000059CC
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.BodyFlags = (BodyFlags)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.FlagsCompressionInfo, ref flag);
			this.ApplyToChildren = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00007808 File Offset: 0x00005A08
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket((int)this.BodyFlags, CompressionBasic.FlagsCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.ApplyToChildren);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00007830 File Offset: 0x00005A30
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00007838 File Offset: 0x00005A38
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Add bodyflags: ",
				this.BodyFlags,
				" to MissionObject with ID: ",
				this.MissionObjectId,
				this.ApplyToChildren ? "" : " and to all of its children."
			});
		}
	}
}
