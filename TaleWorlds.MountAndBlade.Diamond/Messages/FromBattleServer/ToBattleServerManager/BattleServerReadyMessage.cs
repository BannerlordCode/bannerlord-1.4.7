using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.Library;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D4 RID: 212
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleServerReadyMessage : LoginMessage
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00004A41 File Offset: 0x00002C41
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x00004A49 File Offset: 0x00002C49
		[JsonProperty]
		public ApplicationVersion ApplicationVersion { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00004A52 File Offset: 0x00002C52
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x00004A5A File Offset: 0x00002C5A
		[JsonProperty]
		public string AssignedAddress { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00004A63 File Offset: 0x00002C63
		// (set) Token: 0x060003DB RID: 987 RVA: 0x00004A6B File Offset: 0x00002C6B
		[JsonProperty]
		public ushort AssignedPort { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00004A74 File Offset: 0x00002C74
		// (set) Token: 0x060003DD RID: 989 RVA: 0x00004A7C File Offset: 0x00002C7C
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00004A85 File Offset: 0x00002C85
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00004A8D File Offset: 0x00002C8D
		[JsonProperty]
		public sbyte Priority { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00004A96 File Offset: 0x00002C96
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x00004A9E File Offset: 0x00002C9E
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00004AA7 File Offset: 0x00002CA7
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x00004AAF File Offset: 0x00002CAF
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x060003E4 RID: 996 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public BattleServerReadyMessage()
		{
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00004AC0 File Offset: 0x00002CC0
		public BattleServerReadyMessage(PeerId peerId, ApplicationVersion applicationVersion, string assignedAddress, ushort assignedPort, string region, sbyte priority, string password, string gameType)
			: base(peerId, null)
		{
			this.ApplicationVersion = applicationVersion;
			this.AssignedAddress = assignedAddress;
			this.AssignedPort = assignedPort;
			this.Region = region;
			this.Priority = priority;
			this.Password = password;
			this.GameType = gameType;
		}
	}
}
