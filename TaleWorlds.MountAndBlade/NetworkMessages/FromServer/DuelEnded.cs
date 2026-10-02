using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000046 RID: 70
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelEnded : GameNetworkMessage
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00004C92 File Offset: 0x00002E92
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00004C9A File Offset: 0x00002E9A
		public NetworkCommunicator WinnerPeer { get; private set; }

		// Token: 0x06000245 RID: 581 RVA: 0x00004CA3 File Offset: 0x00002EA3
		public DuelEnded(NetworkCommunicator winnerPeer)
		{
			this.WinnerPeer = winnerPeer;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00004CB2 File Offset: 0x00002EB2
		public DuelEnded()
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00004CBC File Offset: 0x00002EBC
		protected override bool OnRead()
		{
			bool flag = true;
			this.WinnerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			return flag;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00004CDA File Offset: 0x00002EDA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.WinnerPeer);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00004CE7 File Offset: 0x00002EE7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00004CEF File Offset: 0x00002EEF
		protected override string OnGetLogFormat()
		{
			return this.WinnerPeer.UserName + "has won the duel";
		}
	}
}
