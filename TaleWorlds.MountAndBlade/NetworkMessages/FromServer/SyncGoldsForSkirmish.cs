using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000065 RID: 101
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncGoldsForSkirmish : GameNetworkMessage
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00006CF4 File Offset: 0x00004EF4
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00006CFC File Offset: 0x00004EFC
		public VirtualPlayer VirtualPlayer { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00006D05 File Offset: 0x00004F05
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00006D0D File Offset: 0x00004F0D
		public int GoldAmount { get; private set; }

		// Token: 0x06000381 RID: 897 RVA: 0x00006D16 File Offset: 0x00004F16
		public SyncGoldsForSkirmish()
		{
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00006D1E File Offset: 0x00004F1E
		public SyncGoldsForSkirmish(VirtualPlayer peer, int goldAmount)
		{
			this.VirtualPlayer = peer;
			this.GoldAmount = goldAmount;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00006D34 File Offset: 0x00004F34
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteVirtualPlayerReferenceToPacket(this.VirtualPlayer);
			GameNetworkMessage.WriteIntToPacket(this.GoldAmount, CompressionBasic.RoundGoldAmountCompressionInfo);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00006D54 File Offset: 0x00004F54
		protected override bool OnRead()
		{
			bool flag = true;
			this.VirtualPlayer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref flag, false);
			this.GoldAmount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RoundGoldAmountCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00006D84 File Offset: 0x00004F84
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00006D8C File Offset: 0x00004F8C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Gold amount set to ",
				this.GoldAmount,
				" for ",
				this.VirtualPlayer.UserName,
				"."
			});
		}
	}
}
