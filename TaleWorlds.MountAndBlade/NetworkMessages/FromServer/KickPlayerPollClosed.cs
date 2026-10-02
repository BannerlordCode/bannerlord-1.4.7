using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000055 RID: 85
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class KickPlayerPollClosed : GameNetworkMessage
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00005CE6 File Offset: 0x00003EE6
		// (set) Token: 0x060002EE RID: 750 RVA: 0x00005CEE File Offset: 0x00003EEE
		public NetworkCommunicator PlayerPeer { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00005CF7 File Offset: 0x00003EF7
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x00005CFF File Offset: 0x00003EFF
		public bool Accepted { get; private set; }

		// Token: 0x060002F1 RID: 753 RVA: 0x00005D08 File Offset: 0x00003F08
		public KickPlayerPollClosed(NetworkCommunicator playerPeer, bool accepted)
		{
			this.PlayerPeer = playerPeer;
			this.Accepted = accepted;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00005D1E File Offset: 0x00003F1E
		public KickPlayerPollClosed()
		{
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00005D28 File Offset: 0x00003F28
		protected override bool OnRead()
		{
			bool flag = true;
			this.PlayerPeer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Accepted = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00005D53 File Offset: 0x00003F53
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.PlayerPeer);
			GameNetworkMessage.WriteBoolToPacket(this.Accepted);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00005D6B File Offset: 0x00003F6B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00005D74 File Offset: 0x00003F74
		protected override string OnGetLogFormat()
		{
			string[] array = new string[5];
			array[0] = "Poll is closed. ";
			int num = 1;
			NetworkCommunicator playerPeer = this.PlayerPeer;
			array[num] = ((playerPeer != null) ? playerPeer.UserName : null);
			array[2] = " is ";
			array[3] = (this.Accepted ? "" : "not ");
			array[4] = "kicked.";
			return string.Concat(array);
		}
	}
}
