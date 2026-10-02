using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200005E RID: 94
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionUsableMapAdded : GameNetworkMessage
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00006507 File Offset: 0x00004707
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000650F File Offset: 0x0000470F
		public string MapId { get; private set; }

		// Token: 0x0600034F RID: 847 RVA: 0x00006518 File Offset: 0x00004718
		public MultiplayerIntermissionUsableMapAdded()
		{
			this.CompatibleGameTypes = new List<string>();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000652B File Offset: 0x0000472B
		public MultiplayerIntermissionUsableMapAdded(string mapId, bool isCompatibleWithAllGameTypes, int compatibleGameTypeCount, List<string> compatibleGameTypes)
		{
			this.MapId = mapId;
			this.IsCompatibleWithAllGameTypes = isCompatibleWithAllGameTypes;
			this.CompatibleGameTypeCount = compatibleGameTypeCount;
			this.CompatibleGameTypes = compatibleGameTypes;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00006550 File Offset: 0x00004750
		protected override bool OnRead()
		{
			bool flag = true;
			this.MapId = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsCompatibleWithAllGameTypes = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.CompatibleGameTypeCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionMapVoteItemCountCompressionInfo, ref flag);
			for (int i = 0; i < this.CompatibleGameTypeCount; i++)
			{
				this.CompatibleGameTypes.Add(GameNetworkMessage.ReadStringFromPacket(ref flag));
			}
			return flag;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000065B0 File Offset: 0x000047B0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.MapId);
			GameNetworkMessage.WriteBoolToPacket(this.IsCompatibleWithAllGameTypes);
			GameNetworkMessage.WriteIntToPacket(this.CompatibleGameTypeCount, CompressionBasic.IntermissionMapVoteItemCountCompressionInfo);
			for (int i = 0; i < this.CompatibleGameTypeCount; i++)
			{
				GameNetworkMessage.WriteStringToPacket(this.CompatibleGameTypes[i]);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00006605 File Offset: 0x00004805
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000660D File Offset: 0x0000480D
		protected override string OnGetLogFormat()
		{
			return "Adding usable map with id: " + this.MapId + ".";
		}

		// Token: 0x040000A1 RID: 161
		public bool IsCompatibleWithAllGameTypes;

		// Token: 0x040000A2 RID: 162
		public int CompatibleGameTypeCount;

		// Token: 0x040000A3 RID: 163
		public List<string> CompatibleGameTypes;
	}
}
