using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x0200001E RID: 30
	public class MissionScoreboardPlayerVM : MPPlayerVM
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x000078E4 File Offset: 0x00005AE4
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x000078EC File Offset: 0x00005AEC
		public int Score { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x000078F5 File Offset: 0x00005AF5
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x000078FD File Offset: 0x00005AFD
		public bool IsBot { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00007906 File Offset: 0x00005B06
		public bool IsMine
		{
			get
			{
				return this._lobbyPeer != null && this._lobbyPeer.IsMine;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000791D File Offset: 0x00005B1D
		public bool IsTeammate
		{
			get
			{
				return this._lobbyPeer != null && this._lobbyPeer.Team.IsPlayerTeam;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000793C File Offset: 0x00005B3C
		public MissionScoreboardPlayerVM(MissionPeer peer, string[] attributes, string[] headerIDs, int score, Action<MissionScoreboardPlayerVM> executeActivate)
			: base(peer)
		{
			this._chatBox = Game.Current.GetGameHandler<ChatBox>();
			this._executeActivate = executeActivate;
			this._lobbyPeer = peer;
			this.Stats = new MBBindingList<MissionScoreboardStatItemVM>();
			for (int i = 0; i < attributes.Length; i++)
			{
				this.Stats.Add(new MissionScoreboardStatItemVM(this, headerIDs[i], ""));
			}
			this.UpdateAttributes(attributes, score);
			this.IsPlayer = this.IsMine;
			this.MVPBadges = new MBBindingList<MissionScoreboardMVPItemVM>();
			base.Peer.SetMuted(PermaMuteList.IsPlayerMuted(peer.Peer.Id));
			this.UpdateIsMuted();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000079E4 File Offset: 0x00005BE4
		public MissionScoreboardPlayerVM(string[] attributes, string[] headerIDs, int score, Action<MissionScoreboardPlayerVM> executeActivate)
			: base(null)
		{
			this._executeActivate = executeActivate;
			this.Stats = new MBBindingList<MissionScoreboardStatItemVM>();
			for (int i = 0; i < attributes.Length; i++)
			{
				this.Stats.Add(new MissionScoreboardStatItemVM(this, headerIDs[i], ""));
			}
			this.UpdateAttributes(attributes, score);
			this.IsBot = true;
			this.IsPlayer = false;
			base.IsDead = false;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007A4F File Offset: 0x00005C4F
		public void Tick(float dt)
		{
			if (!this.IsBot)
			{
				base.IsDead = this._lobbyPeer == null || !this._lobbyPeer.IsControlledAgentActive;
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007A78 File Offset: 0x00005C78
		public void UpdateAttributes(string[] attributes, int score)
		{
			if (this.Stats.Count == attributes.Length)
			{
				for (int i = 0; i < attributes.Length; i++)
				{
					this.Stats[i].Item = attributes[i] ?? string.Empty;
				}
			}
			this.Score = score;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007AC7 File Offset: 0x00005CC7
		public void ExecuteSelection()
		{
			Action<MissionScoreboardPlayerVM> executeActivate = this._executeActivate;
			if (executeActivate == null)
			{
				return;
			}
			executeActivate(this);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007ADC File Offset: 0x00005CDC
		public void UpdateIsMuted()
		{
			bool flag = PermaMuteList.IsPlayerMuted(this._lobbyPeer.Peer.Id);
			this.IsTextMuted = flag || this._chatBox.IsPlayerMuted(this._lobbyPeer.Peer.Id);
			this.IsVoiceMuted = flag || base.Peer.IsMutedFromGameOrPlatform;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007B40 File Offset: 0x00005D40
		public void SetMVPBadgeCount(int badgeCount)
		{
			this.MVPBadges.Clear();
			for (int i = 0; i < badgeCount; i++)
			{
				this.MVPBadges.Add(new MissionScoreboardMVPItemVM());
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00007B74 File Offset: 0x00005D74
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00007B7C File Offset: 0x00005D7C
		[DataSourceProperty]
		public int Ping
		{
			get
			{
				return this._ping;
			}
			set
			{
				if (value != this._ping)
				{
					this._ping = value;
					base.OnPropertyChangedWithValue(value, "Ping");
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00007B9A File Offset: 0x00005D9A
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00007BA2 File Offset: 0x00005DA2
		[DataSourceProperty]
		public bool IsPlayer
		{
			get
			{
				return this._isPlayer;
			}
			set
			{
				if (value != this._isPlayer)
				{
					this._isPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsPlayer");
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007BC0 File Offset: 0x00005DC0
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007BC8 File Offset: 0x00005DC8
		[DataSourceProperty]
		public bool IsVoiceMuted
		{
			get
			{
				return this._isVoiceMuted;
			}
			set
			{
				if (value != this._isVoiceMuted)
				{
					this._isVoiceMuted = value;
					base.OnPropertyChangedWithValue(value, "IsVoiceMuted");
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007BE6 File Offset: 0x00005DE6
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00007BEE File Offset: 0x00005DEE
		[DataSourceProperty]
		public bool IsTextMuted
		{
			get
			{
				return this._isTextMuted;
			}
			set
			{
				if (value != this._isTextMuted)
				{
					this._isTextMuted = value;
					base.OnPropertyChangedWithValue(value, "IsTextMuted");
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007C0C File Offset: 0x00005E0C
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007C14 File Offset: 0x00005E14
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardStatItemVM> Stats
		{
			get
			{
				return this._stats;
			}
			set
			{
				if (value != this._stats)
				{
					this._stats = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionScoreboardStatItemVM>>(value, "Stats");
				}
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00007C32 File Offset: 0x00005E32
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00007C3A File Offset: 0x00005E3A
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardMVPItemVM> MVPBadges
		{
			get
			{
				return this._mvpBadges;
			}
			set
			{
				if (value != this._mvpBadges)
				{
					this._mvpBadges = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionScoreboardMVPItemVM>>(value, "MVPBadges");
				}
			}
		}

		// Token: 0x040000FD RID: 253
		private const string BadgeHeaderID = "badge";

		// Token: 0x04000100 RID: 256
		private readonly MissionPeer _lobbyPeer;

		// Token: 0x04000101 RID: 257
		private readonly Action<MissionScoreboardPlayerVM> _executeActivate;

		// Token: 0x04000102 RID: 258
		private readonly ChatBox _chatBox;

		// Token: 0x04000103 RID: 259
		private int _ping;

		// Token: 0x04000104 RID: 260
		private bool _isPlayer;

		// Token: 0x04000105 RID: 261
		private bool _isVoiceMuted;

		// Token: 0x04000106 RID: 262
		private bool _isTextMuted;

		// Token: 0x04000107 RID: 263
		private MBBindingList<MissionScoreboardStatItemVM> _stats;

		// Token: 0x04000108 RID: 264
		private MBBindingList<MissionScoreboardMVPItemVM> _mvpBadges;
	}
}
