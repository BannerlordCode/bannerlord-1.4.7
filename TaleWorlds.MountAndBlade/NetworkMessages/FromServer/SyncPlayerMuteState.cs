using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000069 RID: 105
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncPlayerMuteState : GameNetworkMessage
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00007136 File Offset: 0x00005336
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000713E File Offset: 0x0000533E
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00007147 File Offset: 0x00005347
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x0000714F File Offset: 0x0000534F
		public bool IsMuted { get; private set; }

		// Token: 0x060003A7 RID: 935 RVA: 0x00007158 File Offset: 0x00005358
		public SyncPlayerMuteState()
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00007160 File Offset: 0x00005360
		public SyncPlayerMuteState(PlayerId playerId, bool isMuted)
		{
			this.PlayerId = playerId;
			this.IsMuted = isMuted;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00007178 File Offset: 0x00005378
		protected override bool OnRead()
		{
			bool flag = true;
			ulong num = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num2 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num3 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			if (flag)
			{
				this.PlayerId = new PlayerId(num, num2, num3, num4);
			}
			this.IsMuted = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000071E0 File Offset: 0x000053E0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part1, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part2, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part3, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part4, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsMuted);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00007258 File Offset: 0x00005458
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00007260 File Offset: 0x00005460
		protected override string OnGetLogFormat()
		{
			return string.Format("SyncPlayerMuteState Player:{0}, IsMuted:{1}", this.PlayerId, this.IsMuted);
		}
	}
}
