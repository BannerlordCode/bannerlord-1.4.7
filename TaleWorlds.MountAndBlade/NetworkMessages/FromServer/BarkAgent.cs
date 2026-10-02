using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007A RID: 122
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BarkAgent : GameNetworkMessage
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000083F9 File Offset: 0x000065F9
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00008401 File Offset: 0x00006601
		public int AgentIndex { get; private set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000840A File Offset: 0x0000660A
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00008412 File Offset: 0x00006612
		public int IndexOfBark { get; private set; }

		// Token: 0x0600046A RID: 1130 RVA: 0x0000841B File Offset: 0x0000661B
		public BarkAgent(int agent, int indexOfBark)
		{
			this.AgentIndex = agent;
			this.IndexOfBark = indexOfBark;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00008431 File Offset: 0x00006631
		public BarkAgent()
		{
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000843C File Offset: 0x0000663C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IndexOfBark = GameNetworkMessage.ReadIntFromPacket(CompressionMission.BarkIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000846B File Offset: 0x0000666B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.IndexOfBark, CompressionMission.BarkIndexCompressionInfo);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00008488 File Offset: 0x00006688
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000848C File Offset: 0x0000668C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "FromServer.BarkAgent agent-index: ", this.AgentIndex, ", IndexOfBark", this.IndexOfBark });
		}
	}
}
