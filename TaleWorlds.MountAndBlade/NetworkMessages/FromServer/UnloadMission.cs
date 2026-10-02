using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CD RID: 205
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UnloadMission : GameNetworkMessage
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0000E5C7 File Offset: 0x0000C7C7
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0000E5CF File Offset: 0x0000C7CF
		public bool UnloadingForBattleIndexMismatch { get; private set; }

		// Token: 0x0600086F RID: 2159 RVA: 0x0000E5D8 File Offset: 0x0000C7D8
		public UnloadMission()
		{
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public UnloadMission(bool unloadingForBattleIndexMismatch)
		{
			this.UnloadingForBattleIndexMismatch = unloadingForBattleIndexMismatch;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.UnloadingForBattleIndexMismatch = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0000E60D File Offset: 0x0000C80D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.UnloadingForBattleIndexMismatch);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0000E61A File Offset: 0x0000C81A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0000E622 File Offset: 0x0000C822
		protected override string OnGetLogFormat()
		{
			return "Unload Mission";
		}
	}
}
