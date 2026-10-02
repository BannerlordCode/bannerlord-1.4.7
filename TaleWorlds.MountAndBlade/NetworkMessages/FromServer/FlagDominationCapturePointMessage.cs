using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008B RID: 139
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagDominationCapturePointMessage : GameNetworkMessage
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00009EE8 File Offset: 0x000080E8
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x00009EF0 File Offset: 0x000080F0
		public int FlagIndex { get; private set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x00009EF9 File Offset: 0x000080F9
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x00009F01 File Offset: 0x00008101
		public int OwnerTeamIndex { get; private set; }

		// Token: 0x0600056B RID: 1387 RVA: 0x00009F0A File Offset: 0x0000810A
		public FlagDominationCapturePointMessage()
		{
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00009F12 File Offset: 0x00008112
		public FlagDominationCapturePointMessage(int flagIndex, int ownerTeamIndex)
		{
			this.FlagIndex = flagIndex;
			this.OwnerTeamIndex = ownerTeamIndex;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00009F28 File Offset: 0x00008128
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FlagIndex, CompressionMission.FlagCapturePointIndexCompressionInfo);
			GameNetworkMessage.WriteTeamIndexToPacket(this.OwnerTeamIndex);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00009F48 File Offset: 0x00008148
		protected override bool OnRead()
		{
			bool flag = true;
			this.FlagIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FlagCapturePointIndexCompressionInfo, ref flag);
			this.OwnerTeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00009F77 File Offset: 0x00008177
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00009F7F File Offset: 0x0000817F
		protected override string OnGetLogFormat()
		{
			return "Flag owner changed.";
		}
	}
}
