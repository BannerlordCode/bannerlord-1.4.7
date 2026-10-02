using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000052 RID: 82
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ChangeGamePoll : GameNetworkMessage
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00005885 File Offset: 0x00003A85
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000588D File Offset: 0x00003A8D
		public string GameType { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00005896 File Offset: 0x00003A96
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000589E File Offset: 0x00003A9E
		public string Map { get; private set; }

		// Token: 0x060002CB RID: 715 RVA: 0x000058A7 File Offset: 0x00003AA7
		public ChangeGamePoll(string gameType, string map)
		{
			this.GameType = gameType;
			this.Map = map;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000058BD File Offset: 0x00003ABD
		public ChangeGamePoll()
		{
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000058C8 File Offset: 0x00003AC8
		protected override bool OnRead()
		{
			bool flag = true;
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000058F2 File Offset: 0x00003AF2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000590A File Offset: 0x00003B0A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00005912 File Offset: 0x00003B12
		protected override string OnGetLogFormat()
		{
			return "Poll started: Change Map to: " + this.Map + " and GameType to: " + this.GameType;
		}
	}
}
