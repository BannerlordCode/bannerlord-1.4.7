using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000056 RID: 86
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class KickPlayerPollOpened : GameNetworkMessage
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00005DD0 File Offset: 0x00003FD0
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x00005DD8 File Offset: 0x00003FD8
		public NetworkCommunicator InitiatorPeer { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00005DE1 File Offset: 0x00003FE1
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00005DE9 File Offset: 0x00003FE9
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00005DF2 File Offset: 0x00003FF2
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00005DFA File Offset: 0x00003FFA
		public bool BanPlayer { get; private set; }

		// Token: 0x060002FD RID: 765 RVA: 0x00005E03 File Offset: 0x00004003
		public KickPlayerPollOpened(NetworkCommunicator initiatorPeer, NetworkCommunicator playerPeer, bool banPlayer)
		{
			this.InitiatorPeer = initiatorPeer;
			this.PlayerPeer = playerPeer;
			this.BanPlayer = banPlayer;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00005E20 File Offset: 0x00004020
		public KickPlayerPollOpened()
		{
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00005E28 File Offset: 0x00004028
		protected override bool OnRead()
		{
			bool flag = true;
			this.InitiatorPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BanPlayer = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00005E61 File Offset: 0x00004061
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.InitiatorPeer);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.BanPlayer);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00005E84 File Offset: 0x00004084
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00005E8C File Offset: 0x0000408C
		protected override string OnGetLogFormat()
		{
			string[] array = new string[5];
			int num = 0;
			NetworkCommunicator initiatorPeer = this.InitiatorPeer;
			array[num] = ((initiatorPeer != null) ? initiatorPeer.UserName : null);
			array[1] = " wants to start poll to kick";
			array[2] = (this.BanPlayer ? " and ban" : "");
			array[3] = " player: ";
			int num2 = 4;
			NetworkCommunicator playerPeer = this.PlayerPeer;
			array[num2] = ((playerPeer != null) ? playerPeer.UserName : null);
			return string.Concat(array);
		}
	}
}
