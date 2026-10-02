using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000EF RID: 239
	public class BattlePeer
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x000053D4 File Offset: 0x000035D4
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x000053DC File Offset: 0x000035DC
		public int Index { get; private set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000053E5 File Offset: 0x000035E5
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x000053ED File Offset: 0x000035ED
		public string Name { get; private set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x000053F6 File Offset: 0x000035F6
		public PlayerId PlayerId
		{
			get
			{
				return this.PlayerData.PlayerId;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00005403 File Offset: 0x00003603
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x0000540B File Offset: 0x0000360B
		public int TeamNo { get; private set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00005414 File Offset: 0x00003614
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x0000541C File Offset: 0x0000361C
		public BattleJoinType BattleJoinType { get; private set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00005425 File Offset: 0x00003625
		public bool Quit
		{
			get
			{
				return this.QuitType > BattlePeerQuitType.None;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00005430 File Offset: 0x00003630
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00005438 File Offset: 0x00003638
		public PlayerData PlayerData { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00005441 File Offset: 0x00003641
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x00005449 File Offset: 0x00003649
		public Dictionary<string, List<string>> UsedCosmetics { get; private set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00005452 File Offset: 0x00003652
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0000545A File Offset: 0x0000365A
		public int SessionKey { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00005463 File Offset: 0x00003663
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0000546B File Offset: 0x0000366B
		public BattlePeerQuitType QuitType { get; private set; }

		// Token: 0x060004A3 RID: 1187 RVA: 0x00005474 File Offset: 0x00003674
		public BattlePeer(string name, PlayerData playerData, Dictionary<string, List<string>> usedCosmetics, int teamNo, BattleJoinType battleJoinType)
		{
			this.Index = -1;
			this.Name = name;
			this.PlayerData = playerData;
			this.UsedCosmetics = usedCosmetics;
			this.TeamNo = teamNo;
			this.BattleJoinType = battleJoinType;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000054A8 File Offset: 0x000036A8
		internal void Flee()
		{
			this.QuitType = BattlePeerQuitType.Fled;
			this.Index = -1;
			this.SessionKey = 0;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000054BF File Offset: 0x000036BF
		internal void SetPlayerDisconnectdFromLobby()
		{
			this.QuitType = BattlePeerQuitType.DisconnectedFromLobby;
			this.Index = -1;
			this.SessionKey = 0;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000054D6 File Offset: 0x000036D6
		internal void SetPlayerDisconnectdFromGameSession()
		{
			this.QuitType = BattlePeerQuitType.DisconnectedFromGameSession;
			this.Index = -1;
			this.SessionKey = 0;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000054ED File Offset: 0x000036ED
		public void Rejoin(int teamNo)
		{
			this.QuitType = BattlePeerQuitType.None;
			this.TeamNo = teamNo;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000054FD File Offset: 0x000036FD
		public void InitializeSession(int index, int sessionKey)
		{
			this.Index = index;
			this.SessionKey = sessionKey;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000550D File Offset: 0x0000370D
		internal void SetPlayerKickedDueToFriendlyDamage()
		{
			this.QuitType = BattlePeerQuitType.KickedDueToFriendlyDamage;
			this.Index = -1;
			this.SessionKey = 0;
		}
	}
}
