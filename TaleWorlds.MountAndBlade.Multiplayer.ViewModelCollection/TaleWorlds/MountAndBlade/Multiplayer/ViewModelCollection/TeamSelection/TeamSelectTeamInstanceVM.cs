using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection
{
	// Token: 0x0200001A RID: 26
	public class TeamSelectTeamInstanceVM : ViewModel
	{
		// Token: 0x06000181 RID: 385 RVA: 0x00006C40 File Offset: 0x00004E40
		public TeamSelectTeamInstanceVM(MissionScoreboardComponent missionScoreboardComponent, Team team, BasicCultureObject culture, Banner banner, Action<Team> onSelect, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColors)
		{
			this.Team = team;
			this._onSelect = onSelect;
			this._culture = culture;
			Mission mission = Mission.Current;
			this.IsSiege = mission != null && mission.HasMissionBehavior<MissionMultiplayerSiegeClient>();
			if (this.Team != null && this.Team.Side != BattleSideEnum.None)
			{
				this._missionScoreboardComponent = missionScoreboardComponent;
				this._missionScoreboardComponent.OnRoundPropertiesChanged += this.UpdateTeamScores;
				this._missionScoreboardSide = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == this.Team.Side);
				this.IsAttacker = this.Team.Side == BattleSideEnum.Attacker;
				this.UpdateTeamScores();
			}
			this.CultureId = ((culture == null) ? "" : culture.StringId);
			if (team == null)
			{
				this.IsDisabled = true;
			}
			this.Banner = new BannerImageIdentifierVM(banner, true);
			this.CultureColor1 = cultureColors.Color1;
			this.CultureColor2 = cultureColors.Color2;
			this._friends = new List<MPPlayerVM>();
			this.FriendAvatars = new MBBindingList<MPPlayerVM>();
			this.RefreshValues();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006D54 File Offset: 0x00004F54
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DisplayedPrimary = ((this._culture == null) ? new TextObject("{=pSheKLB4}Spectator", null).ToString() : this._culture.Name.ToString());
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006D8C File Offset: 0x00004F8C
		public override void OnFinalize()
		{
			if (this._missionScoreboardComponent != null)
			{
				this._missionScoreboardComponent.OnRoundPropertiesChanged -= this.UpdateTeamScores;
			}
			this._missionScoreboardComponent = null;
			this._missionScoreboardSide = null;
			base.OnFinalize();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006DC1 File Offset: 0x00004FC1
		private void UpdateTeamScores()
		{
			if (this._missionScoreboardSide != null)
			{
				this.Score = this._missionScoreboardSide.SideScore;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006DDC File Offset: 0x00004FDC
		public void RefreshFriends(IEnumerable<MissionPeer> friends)
		{
			List<MissionPeer> list = friends.ToList<MissionPeer>();
			List<MPPlayerVM> list2 = new List<MPPlayerVM>();
			foreach (MPPlayerVM mpplayerVM in this._friends)
			{
				if (!list.Contains(mpplayerVM.Peer))
				{
					list2.Add(mpplayerVM);
				}
			}
			foreach (MPPlayerVM mpplayerVM2 in list2)
			{
				this._friends.Remove(mpplayerVM2);
			}
			List<MissionPeer> list3 = this._friends.Select<MPPlayerVM, MissionPeer>((MPPlayerVM x) => x.Peer).ToList<MissionPeer>();
			foreach (MissionPeer missionPeer in list)
			{
				if (!list3.Contains(missionPeer))
				{
					this._friends.Add(new MPPlayerVM(missionPeer));
				}
			}
			this.FriendAvatars.Clear();
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "RefreshFriends");
			for (int i = 0; i < this._friends.Count; i++)
			{
				if (i < 6)
				{
					this.FriendAvatars.Add(this._friends[i]);
				}
				else
				{
					mbstringBuilder.AppendLine<string>(this._friends[i].Peer.DisplayedName);
				}
			}
			int num = this._friends.Count - 6;
			if (num > 0)
			{
				this.HasExtraFriends = true;
				TextObject textObject = new TextObject("{=hbwp3g3k}+{FRIEND_COUNT} {newline} {?PLURAL}friends{?}friend{\\?}", null);
				textObject.SetTextVariable("FRIEND_COUNT", num);
				textObject.SetTextVariable("PLURAL", (num == 1) ? 0 : 1);
				this.FriendsExtraText = textObject.ToString();
				this.FriendsExtraHint = new HintViewModel(textObject, null);
				return;
			}
			mbstringBuilder.Release();
			this.HasExtraFriends = false;
			this.FriendsExtraText = "";
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007014 File Offset: 0x00005214
		public void SetIsDisabled(bool isCurrentTeam, bool disabledForBalance)
		{
			this.IsDisabled = isCurrentTeam || disabledForBalance;
			if (isCurrentTeam)
			{
				this.LockText = new TextObject("{=SoQcsslF}CURRENT TEAM", null).ToString();
				return;
			}
			if (disabledForBalance)
			{
				this.LockText = new TextObject("{=qe46yXVJ}LOCKED FOR BALANCE", null).ToString();
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007052 File Offset: 0x00005252
		public void ExecuteSelectTeam()
		{
			if (this._onSelect != null)
			{
				this._onSelect(this.Team);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000706D File Offset: 0x0000526D
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00007075 File Offset: 0x00005275
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (this._cultureId != value)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007098 File Offset: 0x00005298
		// (set) Token: 0x0600018B RID: 395 RVA: 0x000070A0 File Offset: 0x000052A0
		[DataSourceProperty]
		public int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000070BE File Offset: 0x000052BE
		// (set) Token: 0x0600018D RID: 397 RVA: 0x000070C6 File Offset: 0x000052C6
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (this._isDisabled != value)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000070E4 File Offset: 0x000052E4
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000070EC File Offset: 0x000052EC
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (this._isAttacker != value)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000710A File Offset: 0x0000530A
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00007112 File Offset: 0x00005312
		[DataSourceProperty]
		public bool IsSiege
		{
			get
			{
				return this._isSiege;
			}
			set
			{
				if (this._isSiege != value)
				{
					this._isSiege = value;
					base.OnPropertyChangedWithValue(value, "IsSiege");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007130 File Offset: 0x00005330
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00007138 File Offset: 0x00005338
		[DataSourceProperty]
		public string DisplayedPrimary
		{
			get
			{
				return this._displayedPrimary;
			}
			set
			{
				this._displayedPrimary = value;
				base.OnPropertyChangedWithValue<string>(value, "DisplayedPrimary");
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000714D File Offset: 0x0000534D
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00007155 File Offset: 0x00005355
		[DataSourceProperty]
		public string DisplayedSecondary
		{
			get
			{
				return this._displayedSecondary;
			}
			set
			{
				this._displayedSecondary = value;
				base.OnPropertyChangedWithValue<string>(value, "DisplayedSecondary");
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000716A File Offset: 0x0000536A
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00007172 File Offset: 0x00005372
		[DataSourceProperty]
		public string DisplayedSecondarySub
		{
			get
			{
				return this._displayedSecondarySub;
			}
			set
			{
				this._displayedSecondarySub = value;
				base.OnPropertyChangedWithValue<string>(value, "DisplayedSecondarySub");
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007187 File Offset: 0x00005387
		// (set) Token: 0x06000199 RID: 409 RVA: 0x0000718F File Offset: 0x0000538F
		[DataSourceProperty]
		public string LockText
		{
			get
			{
				return this._lockText;
			}
			set
			{
				this._lockText = value;
				base.OnPropertyChangedWithValue<string>(value, "LockText");
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000071A4 File Offset: 0x000053A4
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000071AC File Offset: 0x000053AC
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner && (value == null || this._banner == null || this._banner.Id != value.Id))
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000071F8 File Offset: 0x000053F8
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00007200 File Offset: 0x00005400
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> FriendAvatars
		{
			get
			{
				return this._friendAvatars;
			}
			set
			{
				if (this._friendAvatars != value)
				{
					this._friendAvatars = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "FriendAvatars");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000721E File Offset: 0x0000541E
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00007226 File Offset: 0x00005426
		[DataSourceProperty]
		public bool HasExtraFriends
		{
			get
			{
				return this._hasExtraFriends;
			}
			set
			{
				if (this._hasExtraFriends != value)
				{
					this._hasExtraFriends = value;
					base.OnPropertyChangedWithValue(value, "HasExtraFriends");
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007244 File Offset: 0x00005444
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000724C File Offset: 0x0000544C
		[DataSourceProperty]
		public string FriendsExtraText
		{
			get
			{
				return this._friendsExtraText;
			}
			set
			{
				if (this._friendsExtraText != value)
				{
					this._friendsExtraText = value;
					base.OnPropertyChangedWithValue<string>(value, "FriendsExtraText");
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000726F File Offset: 0x0000546F
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00007277 File Offset: 0x00005477
		[DataSourceProperty]
		public HintViewModel FriendsExtraHint
		{
			get
			{
				return this._friendsExtraHint;
			}
			set
			{
				if (this._friendsExtraHint != value)
				{
					this._friendsExtraHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FriendsExtraHint");
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007295 File Offset: 0x00005495
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000729D File Offset: 0x0000549D
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x000072C0 File Offset: 0x000054C0
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x000072C8 File Offset: 0x000054C8
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x040000CD RID: 205
		private const int MaxFriendAvatarCount = 6;

		// Token: 0x040000CE RID: 206
		public readonly Team Team;

		// Token: 0x040000CF RID: 207
		public readonly Action<Team> _onSelect;

		// Token: 0x040000D0 RID: 208
		private readonly List<MPPlayerVM> _friends;

		// Token: 0x040000D1 RID: 209
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x040000D2 RID: 210
		private MissionScoreboardComponent.MissionScoreboardSide _missionScoreboardSide;

		// Token: 0x040000D3 RID: 211
		private readonly BasicCultureObject _culture;

		// Token: 0x040000D4 RID: 212
		private bool _isDisabled;

		// Token: 0x040000D5 RID: 213
		private string _displayedPrimary;

		// Token: 0x040000D6 RID: 214
		private string _displayedSecondary;

		// Token: 0x040000D7 RID: 215
		private string _displayedSecondarySub;

		// Token: 0x040000D8 RID: 216
		private string _lockText;

		// Token: 0x040000D9 RID: 217
		private string _cultureId;

		// Token: 0x040000DA RID: 218
		private int _score;

		// Token: 0x040000DB RID: 219
		private BannerImageIdentifierVM _banner;

		// Token: 0x040000DC RID: 220
		private MBBindingList<MPPlayerVM> _friendAvatars;

		// Token: 0x040000DD RID: 221
		private bool _hasExtraFriends;

		// Token: 0x040000DE RID: 222
		private bool _isAttacker;

		// Token: 0x040000DF RID: 223
		private bool _isSiege;

		// Token: 0x040000E0 RID: 224
		private string _friendsExtraText;

		// Token: 0x040000E1 RID: 225
		private HintViewModel _friendsExtraHint;

		// Token: 0x040000E2 RID: 226
		private Color _cultureColor1;

		// Token: 0x040000E3 RID: 227
		private Color _cultureColor2;
	}
}
