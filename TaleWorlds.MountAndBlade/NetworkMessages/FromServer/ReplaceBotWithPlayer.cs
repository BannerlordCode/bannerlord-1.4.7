using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000098 RID: 152
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ReplaceBotWithPlayer : GameNetworkMessage
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0000AD27 File Offset: 0x00008F27
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x0000AD2F File Offset: 0x00008F2F
		public int BotAgentIndex { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0000AD38 File Offset: 0x00008F38
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x0000AD40 File Offset: 0x00008F40
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0000AD49 File Offset: 0x00008F49
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0000AD51 File Offset: 0x00008F51
		public int Health { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0000AD5A File Offset: 0x00008F5A
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0000AD62 File Offset: 0x00008F62
		public int MountHealth { get; private set; }

		// Token: 0x0600060A RID: 1546 RVA: 0x0000AD6B File Offset: 0x00008F6B
		public ReplaceBotWithPlayer(NetworkCommunicator peer, int botAgentIndex, float botAgentHealth, float botAgentMountHealth = -1f)
		{
			this.Peer = peer;
			this.BotAgentIndex = botAgentIndex;
			this.Health = MathF.Ceiling(botAgentHealth);
			this.MountHealth = MathF.Ceiling(botAgentMountHealth);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000AD9A File Offset: 0x00008F9A
		public ReplaceBotWithPlayer()
		{
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.BotAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Health = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentHealthCompressionInfo, ref flag);
			this.MountHealth = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentHealthCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0000ADF3 File Offset: 0x00008FF3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteAgentIndexToPacket(this.BotAgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.Health, CompressionMission.AgentHealthCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.MountHealth, CompressionMission.AgentHealthCompressionInfo);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0000AE2B File Offset: 0x0000902B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0000AE33 File Offset: 0x00009033
		protected override string OnGetLogFormat()
		{
			return "Replace a bot with a player";
		}
	}
}
