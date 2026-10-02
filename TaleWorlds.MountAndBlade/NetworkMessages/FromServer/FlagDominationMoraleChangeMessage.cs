using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000043 RID: 67
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagDominationMoraleChangeMessage : GameNetworkMessage
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000492D File Offset: 0x00002B2D
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00004935 File Offset: 0x00002B35
		public float Morale { get; private set; }

		// Token: 0x06000225 RID: 549 RVA: 0x0000493E File Offset: 0x00002B3E
		public FlagDominationMoraleChangeMessage()
		{
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00004946 File Offset: 0x00002B46
		public FlagDominationMoraleChangeMessage(float morale)
		{
			this.Morale = morale;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00004955 File Offset: 0x00002B55
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteFloatToPacket(this.Morale, CompressionMission.FlagDominationMoraleCompressionInfo);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00004968 File Offset: 0x00002B68
		protected override bool OnRead()
		{
			bool flag = true;
			this.Morale = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagDominationMoraleCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000498A File Offset: 0x00002B8A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00004992 File Offset: 0x00002B92
		protected override string OnGetLogFormat()
		{
			return "Morale synched: " + this.Morale;
		}
	}
}
