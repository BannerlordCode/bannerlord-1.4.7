using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000144 RID: 324
	[Serializable]
	public class PlayerData
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0000CCBA File Offset: 0x0000AEBA
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x0000CCC2 File Offset: 0x0000AEC2
		public PlayerId PlayerId { get; set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0000CCCB File Offset: 0x0000AECB
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x0000CCD3 File Offset: 0x0000AED3
		public PlayerId OwnerPlayerId { get; set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		public string Sigil { get; set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0000CCED File Offset: 0x0000AEED
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x0000CCF5 File Offset: 0x0000AEF5
		public BodyProperties BodyProperties
		{
			get
			{
				return this._bodyProperties;
			}
			set
			{
				this.SetBodyProperties(value);
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000CCFE File Offset: 0x0000AEFE
		private void SetBodyProperties(BodyProperties bodyProperties)
		{
			this._bodyProperties = bodyProperties.ClampForMultiplayer();
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0000CD0D File Offset: 0x0000AF0D
		[JsonIgnore]
		public int ShownBadgeIndex
		{
			get
			{
				Badge byId = BadgeManager.GetById(this.ShownBadgeId);
				if (byId == null)
				{
					return -1;
				}
				return byId.Index;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0000CD25 File Offset: 0x0000AF25
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x0000CD2D File Offset: 0x0000AF2D
		public PlayerStatsBase[] Stats { get; set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0000CD36 File Offset: 0x0000AF36
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x0000CD3E File Offset: 0x0000AF3E
		public int Race { get; set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0000CD47 File Offset: 0x0000AF47
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x0000CD4F File Offset: 0x0000AF4F
		public bool IsFemale { get; set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0000CD58 File Offset: 0x0000AF58
		[JsonIgnore]
		public int KillCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.KillCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0000CD94 File Offset: 0x0000AF94
		[JsonIgnore]
		public int DeathCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.DeathCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0000CDD0 File Offset: 0x0000AFD0
		[JsonIgnore]
		public int AssistCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.AssistCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0000CE0C File Offset: 0x0000B00C
		[JsonIgnore]
		public int WinCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.WinCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0000CE48 File Offset: 0x0000B048
		[JsonIgnore]
		public int LoseCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.LoseCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0000CE82 File Offset: 0x0000B082
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0000CE8A File Offset: 0x0000B08A
		public int Experience { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000CE93 File Offset: 0x0000B093
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0000CE9B File Offset: 0x0000B09B
		public string LastPlayerName { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0000CEAC File Offset: 0x0000B0AC
		public string Username { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0000CEB5 File Offset: 0x0000B0B5
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0000CEBD File Offset: 0x0000B0BD
		public int UserId { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0000CEC6 File Offset: 0x0000B0C6
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0000CECE File Offset: 0x0000B0CE
		public bool IsUsingClanSigil { get; set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0000CED7 File Offset: 0x0000B0D7
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0000CEDF File Offset: 0x0000B0DF
		public string LastRegion { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
		public string[] LastGameTypes { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0000CEF9 File Offset: 0x0000B0F9
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0000CF01 File Offset: 0x0000B101
		public DateTime? LastLogin { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0000CF0A File Offset: 0x0000B10A
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0000CF12 File Offset: 0x0000B112
		public int Playtime { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0000CF1B File Offset: 0x0000B11B
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0000CF23 File Offset: 0x0000B123
		public string ShownBadgeId { get; set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0000CF2C File Offset: 0x0000B12C
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0000CF34 File Offset: 0x0000B134
		public int Gold { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0000CF3D File Offset: 0x0000B13D
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x0000CF45 File Offset: 0x0000B145
		public bool IsMuted { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0000CF50 File Offset: 0x0000B150
		[JsonIgnore]
		public int Level
		{
			get
			{
				return new PlayerDataExperience(this.Experience).Level;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0000CF70 File Offset: 0x0000B170
		[JsonIgnore]
		public int ExperienceToNextLevel
		{
			get
			{
				return new PlayerDataExperience(this.Experience).ExperienceToNextLevel;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0000CF90 File Offset: 0x0000B190
		[JsonIgnore]
		public int ExperienceInCurrentLevel
		{
			get
			{
				return new PlayerDataExperience(this.Experience).ExperienceInCurrentLevel;
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		public void FillWith(PlayerId playerId, PlayerId ownerPlayerId, BodyProperties bodyProperties, bool isFemale, string sigil, int experience, string lastPlayerName, string username, int userId, string lastRegion, string[] lastGameTypes, DateTime? lastLogin, int playtime, string shownBadgeId, int gold, PlayerStatsBase[] stats, bool shouldLog, bool isUsingClanSigil)
		{
			this.PlayerId = playerId;
			this.OwnerPlayerId = ownerPlayerId;
			this.BodyProperties = bodyProperties;
			this.IsFemale = isFemale;
			this.Sigil = sigil;
			this.IsUsingClanSigil = isUsingClanSigil;
			this.Experience = experience;
			this.LastPlayerName = lastPlayerName;
			this.Username = username;
			this.UserId = userId;
			this.LastRegion = lastRegion;
			this.LastGameTypes = lastGameTypes;
			this.LastLogin = lastLogin;
			this.Playtime = playtime;
			this.ShownBadgeId = shownBadgeId;
			this.Gold = gold;
			this.Stats = stats;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0000D04C File Offset: 0x0000B24C
		public void FillWithNewPlayer(PlayerId playerId, PlayerId ownerPlayerId, string[] gameTypes)
		{
			this.Stats = new PlayerStatsBase[0];
			this.PlayerId = playerId;
			this.OwnerPlayerId = ownerPlayerId;
			this.Sigil = "11.8.1.4345.4345.770.774.1.0.0.158.7.5.512.512.770.769.1.0.0";
			this.IsUsingClanSigil = false;
			this.LastGameTypes = gameTypes;
			this.Username = null;
			this.UserId = -1;
			this.Gold = 0;
			BodyProperties bodyProperties;
			if (BodyProperties.FromString("<BodyProperties version='4' age='36.35' weight='0.1025' build='0.7'  key='001C380CC000234B88E68BBA1372B7578B7BB5D788BC567878966669835754B604F926450F67798C000000000000000000000000000000000000000000DC10C4' />", out bodyProperties))
			{
				this.BodyProperties = bodyProperties;
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0000D0B6 File Offset: 0x0000B2B6
		public bool HasGameStats(string gameType)
		{
			return this.GetGameStats(gameType) != null;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
		public PlayerStatsBase GetGameStats(string gameType)
		{
			if (this.Stats != null)
			{
				foreach (PlayerStatsBase playerStatsBase in this.Stats)
				{
					if (playerStatsBase.GameType == gameType)
					{
						return playerStatsBase;
					}
				}
			}
			return null;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0000D104 File Offset: 0x0000B304
		public void UpdateGameStats(PlayerStatsBase playerGameTypeStats)
		{
			bool flag = false;
			if (this.Stats != null)
			{
				for (int i = 0; i < this.Stats.Length; i++)
				{
					if (this.Stats[i].GameType == playerGameTypeStats.GameType)
					{
						this.Stats[i] = playerGameTypeStats;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				List<PlayerStatsBase> list = new List<PlayerStatsBase>();
				if (this.Stats != null)
				{
					list.AddRange(this.Stats);
				}
				list.Add(playerGameTypeStats);
				this.Stats = list.ToArray();
			}
		}

		// Token: 0x040003AF RID: 943
		private const string DefaultBodyProperties1 = "<BodyProperties version='4' age='36.35' weight='0.1025' build='0.7'  key='001C380CC000234B88E68BBA1372B7578B7BB5D788BC567878966669835754B604F926450F67798C000000000000000000000000000000000000000000DC10C4' />";

		// Token: 0x040003B0 RID: 944
		private const string DefaultBodyProperties2 = "<BodyProperties version='4' age='46.35' weight='0.1025' build='0.7'  key='001C380CC000234B88E68BBA1372B7578B7BB5D788BC567878966669835754B604F926450F67798C000000000000000000000000000000000000000000DC10C4' />";

		// Token: 0x040003B1 RID: 945
		public const string DefaultSigil = "11.8.1.4345.4345.770.774.1.0.0.158.7.5.512.512.770.769.1.0.0";

		// Token: 0x040003B5 RID: 949
		private BodyProperties _bodyProperties;
	}
}
