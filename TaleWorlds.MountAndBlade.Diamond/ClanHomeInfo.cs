using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000105 RID: 261
	[Serializable]
	public class ClanHomeInfo
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00006FBD File Offset: 0x000051BD
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x00006FC5 File Offset: 0x000051C5
		[JsonProperty]
		public bool IsInClan { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00006FCE File Offset: 0x000051CE
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00006FD6 File Offset: 0x000051D6
		[JsonProperty]
		public bool CanCreateClan { get; private set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00006FDF File Offset: 0x000051DF
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x00006FE7 File Offset: 0x000051E7
		[JsonProperty]
		public ClanInfo ClanInfo { get; private set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00006FF0 File Offset: 0x000051F0
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x00006FF8 File Offset: 0x000051F8
		[JsonProperty]
		public NotEnoughPlayersInfo NotEnoughPlayersInfo { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00007001 File Offset: 0x00005201
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00007009 File Offset: 0x00005209
		[JsonProperty]
		public PlayerNotEligibleInfo[] PlayerNotEligibleInfos { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00007012 File Offset: 0x00005212
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0000701A File Offset: 0x0000521A
		[JsonProperty]
		public ClanPlayerInfo[] ClanPlayerInfos { get; private set; }

		// Token: 0x0600058A RID: 1418 RVA: 0x00007023 File Offset: 0x00005223
		public ClanHomeInfo(bool isInClan, bool canCreateClan, ClanInfo clanInfo, NotEnoughPlayersInfo notEnoughPlayersInfo, PlayerNotEligibleInfo[] playerNotEligibleInfos, ClanPlayerInfo[] clanPlayerInfos)
		{
			this.IsInClan = isInClan;
			this.CanCreateClan = canCreateClan;
			this.ClanInfo = clanInfo;
			this.NotEnoughPlayersInfo = notEnoughPlayersInfo;
			this.PlayerNotEligibleInfos = playerNotEligibleInfos;
			this.ClanPlayerInfos = clanPlayerInfos;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00007058 File Offset: 0x00005258
		public static ClanHomeInfo CreateInClanInfo(ClanInfo clanInfo, ClanPlayerInfo[] clanPlayerInfos)
		{
			return new ClanHomeInfo(true, false, clanInfo, null, null, clanPlayerInfos);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00007065 File Offset: 0x00005265
		public static ClanHomeInfo CreateCanCreateClanInfo()
		{
			return new ClanHomeInfo(false, true, null, null, null, null);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00007072 File Offset: 0x00005272
		public static ClanHomeInfo CreateCantCreateClanInfo(NotEnoughPlayersInfo notEnoughPlayersInfo, PlayerNotEligibleInfo[] playerNotEligibleInfos)
		{
			return new ClanHomeInfo(false, false, null, notEnoughPlayersInfo, playerNotEligibleInfos, null);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000707F File Offset: 0x0000527F
		public static ClanHomeInfo CreateInvalidStateClanInfo()
		{
			return new ClanHomeInfo(false, false, null, null, null, null);
		}
	}
}
