using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200013F RID: 319
	[Serializable]
	public class PlayerBattleInfo
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0000C9E0 File Offset: 0x0000ABE0
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x0000C9E8 File Offset: 0x0000ABE8
		public PlayerId PlayerId { get; set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0000C9F1 File Offset: 0x0000ABF1
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x0000C9F9 File Offset: 0x0000ABF9
		public string Name { get; set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0000CA02 File Offset: 0x0000AC02
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x0000CA0A File Offset: 0x0000AC0A
		public int TeamNo { get; set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0000CA13 File Offset: 0x0000AC13
		public bool Fled
		{
			get
			{
				return this._state == PlayerBattleInfo.State.Fled;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0000CA1E File Offset: 0x0000AC1E
		public bool Disconnected
		{
			get
			{
				return this._state == PlayerBattleInfo.State.Disconnected;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0000CA29 File Offset: 0x0000AC29
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x0000CA31 File Offset: 0x0000AC31
		public BattleJoinType JoinType { get; set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0000CA3A File Offset: 0x0000AC3A
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x0000CA42 File Offset: 0x0000AC42
		public int PeerIndex { get; set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0000CA4B File Offset: 0x0000AC4B
		public PlayerBattleInfo.State CurrentState
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0000CA53 File Offset: 0x0000AC53
		public PlayerBattleInfo()
		{
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0000CA5B File Offset: 0x0000AC5B
		public PlayerBattleInfo(PlayerId playerId, string name, int teamNo)
		{
			this.PlayerId = playerId;
			this.Name = name;
			this.TeamNo = teamNo;
			this.PeerIndex = -1;
			this._state = PlayerBattleInfo.State.AssignedToBattle;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0000CA86 File Offset: 0x0000AC86
		public PlayerBattleInfo(PlayerId playerId, string name, int teamNo, int peerIndex, PlayerBattleInfo.State state)
		{
			this.PlayerId = playerId;
			this.Name = name;
			this.TeamNo = teamNo;
			this.PeerIndex = peerIndex;
			this._state = state;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0000CAB3 File Offset: 0x0000ACB3
		public void Flee()
		{
			if (this._state != PlayerBattleInfo.State.Disconnected && this._state != PlayerBattleInfo.State.AtBattle)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected AtBattle or Disconnected; got " + this._state);
			}
			this._state = PlayerBattleInfo.State.Fled;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0000CAE9 File Offset: 0x0000ACE9
		public void Disconnect()
		{
			if (this._state != PlayerBattleInfo.State.AtBattle)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected AtBattle got " + this._state);
			}
			this._state = PlayerBattleInfo.State.Disconnected;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0000CB16 File Offset: 0x0000AD16
		public void Initialize(int peerIndex)
		{
			if (this._state != PlayerBattleInfo.State.AssignedToBattle)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected AssignedToBattle got " + this._state);
			}
			this.PeerIndex = peerIndex;
			this._state = PlayerBattleInfo.State.AtBattle;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0000CB4A File Offset: 0x0000AD4A
		public void RejoinBattle(int teamNo)
		{
			if (this._state != PlayerBattleInfo.State.Disconnected)
			{
				throw new Exception("PlayerBattleInfo incorrect state, expected Disconnected got " + this._state);
			}
			this.TeamNo = teamNo;
			this.PeerIndex = -1;
			this._state = PlayerBattleInfo.State.AssignedToBattle;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000CB85 File Offset: 0x0000AD85
		public PlayerBattleInfo Clone()
		{
			return new PlayerBattleInfo(this.PlayerId, this.Name, this.TeamNo, this.PeerIndex, this._state);
		}

		// Token: 0x040003A3 RID: 931
		private PlayerBattleInfo.State _state;

		// Token: 0x020001CC RID: 460
		public enum State
		{
			// Token: 0x040006AD RID: 1709
			Created,
			// Token: 0x040006AE RID: 1710
			AssignedToBattle,
			// Token: 0x040006AF RID: 1711
			AtBattle,
			// Token: 0x040006B0 RID: 1712
			Disconnected,
			// Token: 0x040006B1 RID: 1713
			Fled
		}
	}
}
