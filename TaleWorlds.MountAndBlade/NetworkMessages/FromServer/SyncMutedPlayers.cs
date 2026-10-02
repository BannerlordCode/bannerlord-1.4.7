using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000067 RID: 103
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncMutedPlayers : GameNetworkMessage
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00006E53 File Offset: 0x00005053
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00006E5B File Offset: 0x0000505B
		public int MutedPlayerCount { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00006E64 File Offset: 0x00005064
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00006E6C File Offset: 0x0000506C
		public List<PlayerId> MutedPlayerIds { get; private set; }

		// Token: 0x06000393 RID: 915 RVA: 0x00006E75 File Offset: 0x00005075
		public SyncMutedPlayers()
		{
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00006E7D File Offset: 0x0000507D
		public SyncMutedPlayers(List<PlayerId> mutedPlayerIds)
		{
			this.MutedPlayerIds = mutedPlayerIds;
			List<PlayerId> mutedPlayerIds2 = this.MutedPlayerIds;
			this.MutedPlayerCount = ((mutedPlayerIds2 != null) ? mutedPlayerIds2.Count : 0);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00006EA4 File Offset: 0x000050A4
		protected override bool OnRead()
		{
			bool flag = true;
			this.MutedPlayerIds = new List<PlayerId>();
			this.MutedPlayerCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionVoterCountCompressionInfo, ref flag);
			for (int i = 0; i < this.MutedPlayerCount; i++)
			{
				ulong num = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				ulong num2 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				ulong num3 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				this.MutedPlayerIds.Add(new PlayerId(num, num2, num3, num4));
			}
			return flag;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00006F30 File Offset: 0x00005130
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MutedPlayerCount, CompressionBasic.IntermissionVoterCountCompressionInfo);
			for (int i = 0; i < this.MutedPlayerCount; i++)
			{
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part1, CompressionBasic.DebugULongNonCompressionInfo);
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part2, CompressionBasic.DebugULongNonCompressionInfo);
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part3, CompressionBasic.DebugULongNonCompressionInfo);
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part4, CompressionBasic.DebugULongNonCompressionInfo);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00006FD9 File Offset: 0x000051D9
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00006FE1 File Offset: 0x000051E1
		protected override string OnGetLogFormat()
		{
			return string.Format("SyncMutedPlayers {0} muted players.", this.MutedPlayerCount);
		}
	}
}
