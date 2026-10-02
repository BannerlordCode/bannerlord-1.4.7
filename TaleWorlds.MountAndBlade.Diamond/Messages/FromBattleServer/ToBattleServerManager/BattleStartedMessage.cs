using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D6 RID: 214
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleStartedMessage : Message
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00004B40 File Offset: 0x00002D40
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00004B48 File Offset: 0x00002D48
		[JsonProperty]
		public bool Report { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00004B51 File Offset: 0x00002D51
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00004B59 File Offset: 0x00002D59
		[JsonProperty]
		public Dictionary<string, int> PlayerTeams { get; private set; }

		// Token: 0x060003F0 RID: 1008 RVA: 0x00004B62 File Offset: 0x00002D62
		public BattleStartedMessage()
		{
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00004B6A File Offset: 0x00002D6A
		public BattleStartedMessage(bool report)
		{
			this.Report = report;
			this.PlayerTeams = null;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00004B80 File Offset: 0x00002D80
		public BattleStartedMessage(bool report, Dictionary<string, int> playerTeams)
		{
			this.Report = report;
			this.PlayerTeams = playerTeams;
		}
	}
}
