using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004A RID: 74
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelRoundEnded : GameNetworkMessage
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000504B File Offset: 0x0000324B
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00005053 File Offset: 0x00003253
		public NetworkCommunicator WinnerPeer { get; private set; }

		// Token: 0x06000273 RID: 627 RVA: 0x0000505C File Offset: 0x0000325C
		public DuelRoundEnded(NetworkCommunicator winnerPeer)
		{
			this.WinnerPeer = winnerPeer;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000506B File Offset: 0x0000326B
		public DuelRoundEnded()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00005074 File Offset: 0x00003274
		protected override bool OnRead()
		{
			bool flag = true;
			this.WinnerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00005092 File Offset: 0x00003292
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.WinnerPeer);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000509F File Offset: 0x0000329F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000050A7 File Offset: 0x000032A7
		protected override string OnGetLogFormat()
		{
			return this.WinnerPeer.UserName + "has won the duel against round.";
		}
	}
}
