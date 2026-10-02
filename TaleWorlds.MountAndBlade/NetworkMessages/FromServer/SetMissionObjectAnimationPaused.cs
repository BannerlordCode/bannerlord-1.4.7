using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A6 RID: 166
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationPaused : GameNetworkMessage
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0000BBC2 File Offset: 0x00009DC2
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0000BBCA File Offset: 0x00009DCA
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0000BBD3 File Offset: 0x00009DD3
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x0000BBDB File Offset: 0x00009DDB
		public bool IsPaused { get; private set; }

		// Token: 0x060006AB RID: 1707 RVA: 0x0000BBE4 File Offset: 0x00009DE4
		public SetMissionObjectAnimationPaused(MissionObjectId missionObjectId, bool isPaused)
		{
			this.MissionObjectId = missionObjectId;
			this.IsPaused = isPaused;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0000BBFA File Offset: 0x00009DFA
		public SetMissionObjectAnimationPaused()
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0000BC04 File Offset: 0x00009E04
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsPaused = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0000BC2E File Offset: 0x00009E2E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.IsPaused);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0000BC46 File Offset: 0x00009E46
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0000BC50 File Offset: 0x00009E50
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set animation to be: ",
				this.IsPaused ? "Paused" : "Not paused",
				" on MissionObject with ID: ",
				this.MissionObjectId
			});
		}
	}
}
