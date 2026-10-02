using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FA RID: 250
	[Serializable]
	public class BattleResult
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x000058A8 File Offset: 0x00003AA8
		public BattleResult()
		{
			this.PlayerEntries = new Dictionary<string, BattlePlayerEntry>();
			this.IsCancelled = false;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000058C4 File Offset: 0x00003AC4
		public void AddOrUpdatePlayerEntry(PlayerId playerId, int teamNo, string gameMode, Guid party, int overriddenInitialPlayTime = -1)
		{
			BattlePlayerEntry battlePlayerEntry;
			if (this.PlayerEntries.TryGetValue(playerId.ToString(), out battlePlayerEntry))
			{
				battlePlayerEntry.TeamNo = teamNo;
				battlePlayerEntry.Party = party;
				battlePlayerEntry.GameType = gameMode;
				if (battlePlayerEntry.Disconnected)
				{
					battlePlayerEntry.Disconnected = false;
					battlePlayerEntry.LastJoinTime = DateTime.Now;
					return;
				}
			}
			else
			{
				BattlePlayerStatsBase battlePlayerStatsBase = this.CreatePlayerBattleStats(gameMode);
				battlePlayerEntry = new BattlePlayerEntry();
				battlePlayerEntry.PlayerId = playerId;
				battlePlayerEntry.TeamNo = teamNo;
				battlePlayerEntry.Party = party;
				battlePlayerEntry.GameType = gameMode;
				battlePlayerEntry.PlayerStats = battlePlayerStatsBase;
				battlePlayerEntry.LastJoinTime = DateTime.Now;
				battlePlayerEntry.PlayTime = ((overriddenInitialPlayTime != -1) ? overriddenInitialPlayTime : 0);
				battlePlayerEntry.Disconnected = false;
				this.PlayerEntries.Add(playerId.ToString(), battlePlayerEntry);
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000598E File Offset: 0x00003B8E
		public bool TryGetPlayerEntry(PlayerId playerId, out BattlePlayerEntry battlePlayerEntry)
		{
			return this.PlayerEntries.TryGetValue(playerId.ToString(), out battlePlayerEntry);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000059AC File Offset: 0x00003BAC
		public void HandlePlayerDisconnect(PlayerId playerId)
		{
			BattlePlayerEntry battlePlayerEntry;
			if (this.PlayerEntries.TryGetValue(playerId.ToString(), out battlePlayerEntry))
			{
				battlePlayerEntry.Disconnected = true;
				battlePlayerEntry.PlayTime += (int)(DateTime.Now - battlePlayerEntry.LastJoinTime).TotalSeconds;
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00005A04 File Offset: 0x00003C04
		public void DebugPrint()
		{
			Debug.Print("-----PRINTING BATTLE RESULT-----", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (BattlePlayerEntry battlePlayerEntry in this.PlayerEntries.Values)
			{
				Debug.Print("Player: " + battlePlayerEntry.PlayerId + "[DEBUG] ", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("Kill: " + battlePlayerEntry.PlayerStats.Kills + "[DEBUG] ", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("Death: " + battlePlayerEntry.PlayerStats.Deaths + "[DEBUG] ", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("----", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			Debug.Print("-----PRINTING OVER-----", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00005B28 File Offset: 0x00003D28
		public void SetBattleFinished(int winnerTeamNo, bool isPremadeGame, PremadeGameType premadeGameType)
		{
			this.WinnerTeamNo = winnerTeamNo;
			this.IsPremadeGame = isPremadeGame;
			this.PremadeGameType = premadeGameType;
			foreach (BattlePlayerEntry battlePlayerEntry in this.PlayerEntries.Values)
			{
				battlePlayerEntry.Won = battlePlayerEntry.TeamNo == winnerTeamNo;
				if (!battlePlayerEntry.Disconnected)
				{
					battlePlayerEntry.PlayTime += (int)(DateTime.Now - battlePlayerEntry.LastJoinTime).TotalSeconds;
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00005BCC File Offset: 0x00003DCC
		public void SetBattleCancelled()
		{
			this.IsCancelled = true;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00005BD8 File Offset: 0x00003DD8
		private BattlePlayerStatsBase CreatePlayerBattleStats(string gameType)
		{
			if (gameType == "Skirmish")
			{
				return new BattlePlayerStatsSkirmish();
			}
			if (gameType == "Captain")
			{
				return new BattlePlayerStatsCaptain();
			}
			if (gameType == "Siege")
			{
				return new BattlePlayerStatsSiege();
			}
			if (gameType == "TeamDeathmatch")
			{
				return new BattlePlayerStatsTeamDeathmatch();
			}
			if (gameType == "Duel")
			{
				return new BattlePlayerStatsDuel();
			}
			if (gameType == "Battle")
			{
				return new BattlePlayerStatsBattle();
			}
			return new BattlePlayerStatsBase();
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00005C5C File Offset: 0x00003E5C
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00005C64 File Offset: 0x00003E64
		[JsonProperty]
		public bool IsCancelled { get; private set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00005C6D File Offset: 0x00003E6D
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00005C75 File Offset: 0x00003E75
		[JsonProperty]
		public int WinnerTeamNo { get; private set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00005C7E File Offset: 0x00003E7E
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x00005C86 File Offset: 0x00003E86
		[JsonProperty]
		public bool IsPremadeGame { get; private set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00005C8F File Offset: 0x00003E8F
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x00005C97 File Offset: 0x00003E97
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00005CA0 File Offset: 0x00003EA0
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x00005CA8 File Offset: 0x00003EA8
		[JsonProperty]
		public Dictionary<string, BattlePlayerEntry> PlayerEntries { get; private set; }
	}
}
