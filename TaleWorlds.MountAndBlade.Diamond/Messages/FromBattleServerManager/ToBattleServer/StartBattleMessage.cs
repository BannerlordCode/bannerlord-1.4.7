using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E6 RID: 230
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class StartBattleMessage : Message
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00004E40 File Offset: 0x00003040
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x00004E48 File Offset: 0x00003048
		[JsonProperty]
		public string SceneName { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00004E51 File Offset: 0x00003051
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00004E59 File Offset: 0x00003059
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00004E62 File Offset: 0x00003062
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x00004E6A File Offset: 0x0000306A
		[JsonProperty]
		public Guid BattleId { get; private set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00004E73 File Offset: 0x00003073
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00004E7B File Offset: 0x0000307B
		[JsonProperty]
		public string Faction1 { get; private set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00004E84 File Offset: 0x00003084
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00004E8C File Offset: 0x0000308C
		[JsonProperty]
		public string Faction2 { get; private set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00004E95 File Offset: 0x00003095
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00004E9D File Offset: 0x0000309D
		[JsonProperty]
		public int MinRequiredPlayerCountToStartBattle { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00004EA6 File Offset: 0x000030A6
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00004EAE File Offset: 0x000030AE
		[JsonProperty]
		public int BattleSize { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00004EB7 File Offset: 0x000030B7
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00004EBF File Offset: 0x000030BF
		[JsonProperty]
		public int RoundThreshold { get; private set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00004EC8 File Offset: 0x000030C8
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00004ED0 File Offset: 0x000030D0
		[JsonProperty]
		public float MoraleThreshold { get; private set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00004ED9 File Offset: 0x000030D9
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00004EE1 File Offset: 0x000030E1
		[JsonProperty]
		public bool UseAnalytics { get; private set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00004EEA File Offset: 0x000030EA
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00004EF2 File Offset: 0x000030F2
		[JsonProperty]
		public bool CaptureMovementData { get; private set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00004EFB File Offset: 0x000030FB
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00004F03 File Offset: 0x00003103
		[JsonProperty]
		public string AnalyticsServiceAddress { get; private set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00004F0C File Offset: 0x0000310C
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00004F14 File Offset: 0x00003114
		[JsonProperty]
		public int MaxFriendlyKillCount { get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00004F1D File Offset: 0x0000311D
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00004F25 File Offset: 0x00003125
		[JsonProperty]
		public float MaxFriendlyDamage { get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00004F2E File Offset: 0x0000312E
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00004F36 File Offset: 0x00003136
		[JsonProperty]
		public float MaxFriendlyDamagePerSingleRound { get; private set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00004F3F File Offset: 0x0000313F
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00004F47 File Offset: 0x00003147
		[JsonProperty]
		public float RoundFriendlyDamageLimit { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00004F50 File Offset: 0x00003150
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00004F58 File Offset: 0x00003158
		[JsonProperty]
		public int MaxRoundsOverLimitCount { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00004F61 File Offset: 0x00003161
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00004F69 File Offset: 0x00003169
		[JsonProperty]
		public bool IsPremadeGame { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00004F72 File Offset: 0x00003172
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x00004F7A File Offset: 0x0000317A
		[JsonProperty]
		public string[] ProfanityList { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00004F83 File Offset: 0x00003183
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x00004F8B File Offset: 0x0000318B
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00004F94 File Offset: 0x00003194
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00004F9C File Offset: 0x0000319C
		[JsonProperty]
		public string[] AllowList { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00004FA5 File Offset: 0x000031A5
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00004FAD File Offset: 0x000031AD
		[JsonProperty]
		public PlayerId[] AssignedPlayers { get; private set; }

		// Token: 0x06000460 RID: 1120 RVA: 0x00004FB6 File Offset: 0x000031B6
		public StartBattleMessage()
		{
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00004FC0 File Offset: 0x000031C0
		public StartBattleMessage(Guid battleId, string sceneName, string gameType, string faction1, string faction2, int minRequiredPlayerCountToStartBattle, int battleSize, int roundThreshold, float moraleThreshold, bool useAnalytics, bool captureMovementData, string analyticsServiceAddress, int maxFriendlyKillCount, float maxFriendlyDamage, float maxFriendlyDamagePerSingleRound, float roundFriendlyDamageLimit, int maxRoundsOverLimitCount, bool isPremadeGame, PremadeGameType premadeGameType, string[] profanityList, string[] allowList, PlayerId[] assignedPlayers)
		{
			this.SceneName = sceneName;
			this.GameType = gameType;
			this.BattleId = battleId;
			this.Faction1 = faction1;
			this.Faction2 = faction2;
			this.MinRequiredPlayerCountToStartBattle = minRequiredPlayerCountToStartBattle;
			this.BattleSize = battleSize;
			this.UseAnalytics = useAnalytics;
			this.CaptureMovementData = captureMovementData;
			this.AnalyticsServiceAddress = analyticsServiceAddress;
			this.RoundThreshold = roundThreshold;
			this.MoraleThreshold = moraleThreshold;
			this.MaxFriendlyKillCount = maxFriendlyKillCount;
			this.MaxFriendlyDamage = maxFriendlyDamage;
			this.MaxFriendlyDamagePerSingleRound = maxFriendlyDamagePerSingleRound;
			this.RoundFriendlyDamageLimit = roundFriendlyDamageLimit;
			this.MaxRoundsOverLimitCount = maxRoundsOverLimitCount;
			this.IsPremadeGame = isPremadeGame;
			this.PremadeGameType = premadeGameType;
			this.ProfanityList = profanityList;
			this.AllowList = allowList;
			this.AssignedPlayers = assignedPlayers;
		}
	}
}
