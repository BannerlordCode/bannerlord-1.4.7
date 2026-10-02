using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B5 RID: 181
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeLadderState : GameNetworkMessage
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0000CA81 File Offset: 0x0000AC81
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x0000CA89 File Offset: 0x0000AC89
		public MissionObjectId SiegeLadderId { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0000CA92 File Offset: 0x0000AC92
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x0000CA9A File Offset: 0x0000AC9A
		public SiegeLadder.LadderState State { get; private set; }

		// Token: 0x06000749 RID: 1865 RVA: 0x0000CAA3 File Offset: 0x0000ACA3
		public SetSiegeLadderState(MissionObjectId siegeLadderId, SiegeLadder.LadderState state)
		{
			this.SiegeLadderId = siegeLadderId;
			this.State = state;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0000CAB9 File Offset: 0x0000ACB9
		public SetSiegeLadderState()
		{
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeLadderId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.State = (SiegeLadder.LadderState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeLadderStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0000CAF3 File Offset: 0x0000ACF3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeLadderId);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeLadderStateCompressionInfo);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0000CB10 File Offset: 0x0000AD10
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000CB18 File Offset: 0x0000AD18
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set SiegeLadder State to: ", this.State, " on SiegeLadderState with ID: ", this.SiegeLadderId });
		}
	}
}
