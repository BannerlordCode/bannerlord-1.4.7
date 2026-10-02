using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B1 RID: 177
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetPeerTeam : GameNetworkMessage
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0000C759 File Offset: 0x0000A959
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0000C761 File Offset: 0x0000A961
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0000C76A File Offset: 0x0000A96A
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0000C772 File Offset: 0x0000A972
		public int TeamIndex { get; private set; }

		// Token: 0x06000723 RID: 1827 RVA: 0x0000C77B File Offset: 0x0000A97B
		public SetPeerTeam(NetworkCommunicator peer, int teamIndex)
		{
			this.Peer = peer;
			this.TeamIndex = teamIndex;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000C791 File Offset: 0x0000A991
		public SetPeerTeam()
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0000C79C File Offset: 0x0000A99C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0000C7C7 File Offset: 0x0000A9C7
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000C7DF File Offset: 0x0000A9DF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set Team: ",
				this.TeamIndex,
				" of NetworkPeer with name: ",
				this.Peer.UserName,
				" and peer-index",
				this.Peer.Index
			});
		}
	}
}
