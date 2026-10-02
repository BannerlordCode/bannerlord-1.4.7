using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000029 RID: 41
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class BarkSelected : GameNetworkMessage
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00003BF3 File Offset: 0x00001DF3
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00003BFB File Offset: 0x00001DFB
		public int IndexOfBark { get; private set; }

		// Token: 0x0600014D RID: 333 RVA: 0x00003C04 File Offset: 0x00001E04
		public BarkSelected(int indexOfBark)
		{
			this.IndexOfBark = indexOfBark;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00003C13 File Offset: 0x00001E13
		public BarkSelected()
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00003C1C File Offset: 0x00001E1C
		protected override bool OnRead()
		{
			bool flag = true;
			this.IndexOfBark = GameNetworkMessage.ReadIntFromPacket(CompressionMission.BarkIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00003C3E File Offset: 0x00001E3E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.IndexOfBark, CompressionMission.BarkIndexCompressionInfo);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00003C50 File Offset: 0x00001E50
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00003C54 File Offset: 0x00001E54
		protected override string OnGetLogFormat()
		{
			return "FromClient.BarkSelected: " + this.IndexOfBark;
		}
	}
}
