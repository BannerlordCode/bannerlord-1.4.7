using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000049 RID: 73
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class DuelRequest : GameNetworkMessage
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00004F6E File Offset: 0x0000316E
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00004F76 File Offset: 0x00003176
		public int RequesterAgentIndex { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00004F7F File Offset: 0x0000317F
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00004F87 File Offset: 0x00003187
		public int RequestedAgentIndex { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00004F90 File Offset: 0x00003190
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00004F98 File Offset: 0x00003198
		public TroopType SelectedAreaTroopType { get; private set; }

		// Token: 0x0600026B RID: 619 RVA: 0x00004FA1 File Offset: 0x000031A1
		public DuelRequest(int requesterAgentIndex, int requestedAgentIndex, TroopType selectedAreaTroopType)
		{
			this.RequesterAgentIndex = requesterAgentIndex;
			this.RequestedAgentIndex = requestedAgentIndex;
			this.SelectedAreaTroopType = selectedAreaTroopType;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00004FBE File Offset: 0x000031BE
		public DuelRequest()
		{
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00004FC8 File Offset: 0x000031C8
		protected override bool OnRead()
		{
			bool flag = true;
			this.RequesterAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.RequestedAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SelectedAreaTroopType = (TroopType)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.TroopTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00005004 File Offset: 0x00003204
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.RequesterAgentIndex);
			GameNetworkMessage.WriteAgentIndexToPacket(this.RequestedAgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SelectedAreaTroopType, CompressionBasic.TroopTypeCompressionInfo);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000502C File Offset: 0x0000322C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00005034 File Offset: 0x00003234
		protected override string OnGetLogFormat()
		{
			return "Request duel from agent with index: " + this.RequestedAgentIndex;
		}
	}
}
