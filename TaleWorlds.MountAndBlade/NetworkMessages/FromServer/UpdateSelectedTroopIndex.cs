using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200004D RID: 77
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UpdateSelectedTroopIndex : GameNetworkMessage
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00005305 File Offset: 0x00003505
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000530D File Offset: 0x0000350D
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00005316 File Offset: 0x00003516
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000531E File Offset: 0x0000351E
		public int SelectedTroopIndex { get; private set; }

		// Token: 0x06000291 RID: 657 RVA: 0x00005327 File Offset: 0x00003527
		public UpdateSelectedTroopIndex(NetworkCommunicator peer, int selectedTroopIndex)
		{
			this.Peer = peer;
			this.SelectedTroopIndex = selectedTroopIndex;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000533D File Offset: 0x0000353D
		public UpdateSelectedTroopIndex()
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00005348 File Offset: 0x00003548
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.SelectedTroopIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SelectedTroopIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00005378 File Offset: 0x00003578
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.SelectedTroopIndex, CompressionMission.SelectedTroopIndexCompressionInfo);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00005395 File Offset: 0x00003595
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Equipment;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000539C File Offset: 0x0000359C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Update SelectedTroopIndex to: ",
				this.SelectedTroopIndex,
				", on peer: ",
				this.Peer.UserName,
				" with peer-index:",
				this.Peer.Index
			});
		}
	}
}
