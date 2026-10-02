using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000173 RID: 371
	public class MatchHistoryData : MultiplayerLocalData
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00010A13 File Offset: 0x0000EC13
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x00010A1B File Offset: 0x0000EC1B
		public string MatchId { get; set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00010A24 File Offset: 0x0000EC24
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x00010A2C File Offset: 0x0000EC2C
		public string MatchType { get; set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00010A35 File Offset: 0x0000EC35
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x00010A3D File Offset: 0x0000EC3D
		public string GameType { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00010A46 File Offset: 0x0000EC46
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x00010A4E File Offset: 0x0000EC4E
		public string Map { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00010A57 File Offset: 0x0000EC57
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x00010A5F File Offset: 0x0000EC5F
		public DateTime MatchDate { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00010A68 File Offset: 0x0000EC68
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x00010A70 File Offset: 0x0000EC70
		public int WinnerTeam { get; set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00010A79 File Offset: 0x0000EC79
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x00010A81 File Offset: 0x0000EC81
		public string Faction1 { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00010A8A File Offset: 0x0000EC8A
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x00010A92 File Offset: 0x0000EC92
		public string Faction2 { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00010A9B File Offset: 0x0000EC9B
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x00010AA3 File Offset: 0x0000ECA3
		public int DefenderScore { get; set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00010AAC File Offset: 0x0000ECAC
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		public int AttackerScore { get; set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00010ABD File Offset: 0x0000ECBD
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x00010AC5 File Offset: 0x0000ECC5
		public List<PlayerInfo> Players { get; set; }

		// Token: 0x06000A65 RID: 2661 RVA: 0x00010ACE File Offset: 0x0000ECCE
		public MatchHistoryData()
		{
			this.Players = new List<PlayerInfo>();
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00010AE4 File Offset: 0x0000ECE4
		public override bool HasSameContentWith(MultiplayerLocalData other)
		{
			MatchHistoryData matchHistoryData;
			if ((matchHistoryData = other as MatchHistoryData) == null)
			{
				return false;
			}
			bool flag = this.MatchId == matchHistoryData.MatchId && this.MatchType == matchHistoryData.MatchType && this.GameType == matchHistoryData.GameType && this.Map == matchHistoryData.Map && this.MatchDate == matchHistoryData.MatchDate && this.WinnerTeam == matchHistoryData.WinnerTeam && this.Faction1 == matchHistoryData.Faction1 && this.Faction2 == matchHistoryData.Faction2 && this.DefenderScore == matchHistoryData.DefenderScore && this.AttackerScore == matchHistoryData.AttackerScore;
			if (!flag)
			{
				return false;
			}
			if (this.Players != null || matchHistoryData.Players != null)
			{
				List<PlayerInfo> players = this.Players;
				int? num = ((players != null) ? new int?(players.Count) : null);
				List<PlayerInfo> players2 = matchHistoryData.Players;
				int? num2 = ((players2 != null) ? new int?(players2.Count) : null);
				if (!((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null))))
				{
					return flag;
				}
			}
			for (int i = 0; i < this.Players.Count; i++)
			{
				PlayerInfo playerInfo = this.Players[i];
				PlayerInfo playerInfo2 = matchHistoryData.Players[i];
				if (!playerInfo.HasSameContentWith(playerInfo2))
				{
					return false;
				}
			}
			return flag;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00010C78 File Offset: 0x0000EE78
		private PlayerInfo TryGetPlayer(string id)
		{
			foreach (PlayerInfo playerInfo in this.Players)
			{
				if (playerInfo.PlayerId == id)
				{
					return playerInfo;
				}
			}
			return null;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00010CDC File Offset: 0x0000EEDC
		public void AddOrUpdatePlayer(string id, string username, int forcedIndex, int teamNo)
		{
			PlayerInfo playerInfo = this.TryGetPlayer(id);
			if (playerInfo == null)
			{
				this.Players.Add(new PlayerInfo
				{
					PlayerId = id,
					Username = username,
					ForcedIndex = forcedIndex,
					TeamNo = teamNo
				});
				return;
			}
			playerInfo.TeamNo = teamNo;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00010D2C File Offset: 0x0000EF2C
		public bool TryUpdatePlayerStats(string id, int kill, int death, int assist)
		{
			PlayerInfo playerInfo = this.TryGetPlayer(id);
			if (playerInfo != null)
			{
				playerInfo.Kill = kill;
				playerInfo.Death = death;
				playerInfo.Assist = assist;
				return true;
			}
			return false;
		}
	}
}
