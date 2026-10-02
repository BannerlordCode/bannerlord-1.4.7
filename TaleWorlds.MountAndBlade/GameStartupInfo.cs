using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000237 RID: 567
	public class GameStartupInfo
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x000747EC File Offset: 0x000729EC
		// (set) Token: 0x060020E9 RID: 8425 RVA: 0x000747F4 File Offset: 0x000729F4
		public GameStartupType StartupType { get; internal set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060020EA RID: 8426 RVA: 0x000747FD File Offset: 0x000729FD
		// (set) Token: 0x060020EB RID: 8427 RVA: 0x00074805 File Offset: 0x00072A05
		public DedicatedServerType DedicatedServerType { get; internal set; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060020EC RID: 8428 RVA: 0x0007480E File Offset: 0x00072A0E
		// (set) Token: 0x060020ED RID: 8429 RVA: 0x00074816 File Offset: 0x00072A16
		public bool PlayerHostedDedicatedServer { get; internal set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x0007481F File Offset: 0x00072A1F
		// (set) Token: 0x060020EF RID: 8431 RVA: 0x00074827 File Offset: 0x00072A27
		public bool IsSinglePlatformServer { get; internal set; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x00074830 File Offset: 0x00072A30
		// (set) Token: 0x060020F1 RID: 8433 RVA: 0x00074838 File Offset: 0x00072A38
		public string CustomServerHostIP { get; internal set; } = string.Empty;

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x00074841 File Offset: 0x00072A41
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x00074849 File Offset: 0x00072A49
		public int ServerPort { get; internal set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x00074852 File Offset: 0x00072A52
		// (set) Token: 0x060020F5 RID: 8437 RVA: 0x0007485A File Offset: 0x00072A5A
		public string ServerRegion { get; internal set; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x00074863 File Offset: 0x00072A63
		// (set) Token: 0x060020F7 RID: 8439 RVA: 0x0007486B File Offset: 0x00072A6B
		public sbyte ServerPriority { get; internal set; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x00074874 File Offset: 0x00072A74
		// (set) Token: 0x060020F9 RID: 8441 RVA: 0x0007487C File Offset: 0x00072A7C
		public string ServerGameMode { get; internal set; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x00074885 File Offset: 0x00072A85
		// (set) Token: 0x060020FB RID: 8443 RVA: 0x0007488D File Offset: 0x00072A8D
		public string CustomGameServerConfigFile { get; internal set; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x00074896 File Offset: 0x00072A96
		// (set) Token: 0x060020FD RID: 8445 RVA: 0x0007489E File Offset: 0x00072A9E
		public string CustomGameServerNameOverride { get; internal set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x000748A7 File Offset: 0x00072AA7
		// (set) Token: 0x060020FF RID: 8447 RVA: 0x000748AF File Offset: 0x00072AAF
		public string CustomGameServerPasswordOverride { get; internal set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x000748B8 File Offset: 0x00072AB8
		// (set) Token: 0x06002101 RID: 8449 RVA: 0x000748C0 File Offset: 0x00072AC0
		public string CustomGameServerAuthToken { get; internal set; }

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x000748C9 File Offset: 0x00072AC9
		// (set) Token: 0x06002103 RID: 8451 RVA: 0x000748D1 File Offset: 0x00072AD1
		public bool CustomGameServerAllowsOptionalModules { get; internal set; } = true;

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002104 RID: 8452 RVA: 0x000748DA File Offset: 0x00072ADA
		// (set) Token: 0x06002105 RID: 8453 RVA: 0x000748E2 File Offset: 0x00072AE2
		public string OverridenUserName { get; internal set; }

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002106 RID: 8454 RVA: 0x000748EB File Offset: 0x00072AEB
		// (set) Token: 0x06002107 RID: 8455 RVA: 0x000748F3 File Offset: 0x00072AF3
		public string PremadeGameType { get; internal set; }

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002108 RID: 8456 RVA: 0x000748FC File Offset: 0x00072AFC
		// (set) Token: 0x06002109 RID: 8457 RVA: 0x00074904 File Offset: 0x00072B04
		public int Permission { get; internal set; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x0007490D File Offset: 0x00072B0D
		// (set) Token: 0x0600210B RID: 8459 RVA: 0x00074915 File Offset: 0x00072B15
		public string PlatformInterface { get; internal set; }

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x0007491E File Offset: 0x00072B1E
		// (set) Token: 0x0600210D RID: 8461 RVA: 0x00074926 File Offset: 0x00072B26
		public string EpicUserId { get; internal set; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x0007492F File Offset: 0x00072B2F
		// (set) Token: 0x0600210F RID: 8463 RVA: 0x00074937 File Offset: 0x00072B37
		public string EpicUserName { get; internal set; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x00074940 File Offset: 0x00072B40
		// (set) Token: 0x06002111 RID: 8465 RVA: 0x00074948 File Offset: 0x00072B48
		public bool IsContinueGame { get; internal set; }

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002112 RID: 8466 RVA: 0x00074951 File Offset: 0x00072B51
		// (set) Token: 0x06002113 RID: 8467 RVA: 0x00074959 File Offset: 0x00072B59
		public double ServerBandwidthLimitInMbps { get; internal set; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x00074962 File Offset: 0x00072B62
		// (set) Token: 0x06002115 RID: 8469 RVA: 0x0007496A File Offset: 0x00072B6A
		public int ServerTickRate { get; internal set; }
	}
}
