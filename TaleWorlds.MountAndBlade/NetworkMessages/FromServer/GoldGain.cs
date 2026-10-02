using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000053 RID: 83
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class GoldGain : GameNetworkMessage
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000592F File Offset: 0x00003B2F
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x00005937 File Offset: 0x00003B37
		public List<KeyValuePair<ushort, int>> GoldChangeEventList { get; private set; }

		// Token: 0x060002D3 RID: 723 RVA: 0x00005940 File Offset: 0x00003B40
		public GoldGain(List<KeyValuePair<ushort, int>> goldChangeEventList)
		{
			this.GoldChangeEventList = goldChangeEventList;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000594F File Offset: 0x00003B4F
		public GoldGain()
		{
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00005958 File Offset: 0x00003B58
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.GoldChangeEventList.Count - 1, CompressionMission.TdmGoldGainTypeCompressionInfo);
			foreach (KeyValuePair<ushort, int> keyValuePair in this.GoldChangeEventList)
			{
				GameNetworkMessage.WriteIntToPacket((int)keyValuePair.Key, CompressionMission.TdmGoldGainTypeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(keyValuePair.Value, CompressionMission.TdmGoldChangeCompressionInfo);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000059E0 File Offset: 0x00003BE0
		protected override bool OnRead()
		{
			bool flag = true;
			this.GoldChangeEventList = new List<KeyValuePair<ushort, int>>();
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TdmGoldGainTypeCompressionInfo, ref flag) + 1;
			for (int i = 0; i < num; i++)
			{
				ushort num2 = (ushort)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TdmGoldGainTypeCompressionInfo, ref flag);
				int num3 = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TdmGoldChangeCompressionInfo, ref flag);
				this.GoldChangeEventList.Add(new KeyValuePair<ushort, int>(num2, num3));
			}
			return flag;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00005A45 File Offset: 0x00003C45
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00005A4D File Offset: 0x00003C4D
		protected override string OnGetLogFormat()
		{
			return "Gold change events synced.";
		}
	}
}
