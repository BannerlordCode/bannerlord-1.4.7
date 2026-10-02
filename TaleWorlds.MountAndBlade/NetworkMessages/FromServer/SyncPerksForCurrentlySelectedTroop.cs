using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000068 RID: 104
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncPerksForCurrentlySelectedTroop : GameNetworkMessage
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00006FF8 File Offset: 0x000051F8
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00007000 File Offset: 0x00005200
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00007009 File Offset: 0x00005209
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00007011 File Offset: 0x00005211
		public int[] PerkIndices { get; private set; }

		// Token: 0x0600039D RID: 925 RVA: 0x0000701A File Offset: 0x0000521A
		public SyncPerksForCurrentlySelectedTroop()
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00007022 File Offset: 0x00005222
		public SyncPerksForCurrentlySelectedTroop(NetworkCommunicator peer, int[] perkIndices)
		{
			this.Peer = peer;
			this.PerkIndices = perkIndices;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00007038 File Offset: 0x00005238
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			for (int i = 0; i < 3; i++)
			{
				GameNetworkMessage.WriteIntToPacket(this.PerkIndices[i], CompressionMission.PerkIndexCompressionInfo);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00007070 File Offset: 0x00005270
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.PerkIndices = new int[3];
			for (int i = 0; i < 3; i++)
			{
				this.PerkIndices[i] = GameNetworkMessage.ReadIntFromPacket(CompressionMission.PerkIndexCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000070BA File Offset: 0x000052BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000070C4 File Offset: 0x000052C4
		protected override string OnGetLogFormat()
		{
			string text = "";
			for (int i = 0; i < 3; i++)
			{
				text += string.Format("[{0}]", this.PerkIndices[i]);
			}
			return string.Concat(new string[]
			{
				"Selected perks for ",
				this.Peer.UserName,
				" has been updated as ",
				text,
				"."
			});
		}
	}
}
