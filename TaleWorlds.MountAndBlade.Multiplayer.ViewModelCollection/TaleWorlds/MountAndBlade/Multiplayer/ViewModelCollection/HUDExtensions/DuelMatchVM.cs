using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000093 RID: 147
	public class DuelMatchVM : ViewModel
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0002C030 File Offset: 0x0002A230
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x0002C038 File Offset: 0x0002A238
		public MissionPeer FirstPlayerPeer { get; private set; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0002C041 File Offset: 0x0002A241
		// (set) Token: 0x06000E48 RID: 3656 RVA: 0x0002C049 File Offset: 0x0002A249
		public MissionPeer SecondPlayerPeer { get; private set; }

		// Token: 0x06000E49 RID: 3657 RVA: 0x0002C052 File Offset: 0x0002A252
		public DuelMatchVM()
		{
			this.IsEnabled = false;
			this._duelCountdownText = new TextObject("{=cO2FDHCa}Duel with {OPPONENT_NAME} is starting in {DUEL_REMAINING_TIME} seconds.", null);
			this.RefreshValues();
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0002C078 File Offset: 0x0002A278
		public void OnDuelPrepStarted(MissionPeer opponentPeer, int prepDuration)
		{
			this._prepTimeRemaining = (float)prepDuration;
			GameTexts.SetVariable("OPPONENT_NAME", opponentPeer.DisplayedName);
			this.IsPreparing = true;
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002C09C File Offset: 0x0002A29C
		public void Tick(float dt)
		{
			if (this._prepTimeRemaining > 0f)
			{
				GameTexts.SetVariable("DUEL_REMAINING_TIME", (float)MathF.Ceiling(this._prepTimeRemaining));
				this.CountdownMessage = this._duelCountdownText.ToString();
				this._prepTimeRemaining -= dt;
				return;
			}
			this.IsPreparing = false;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0002C0F4 File Offset: 0x0002A2F4
		public void OnDuelStarted(MissionPeer firstPeer, MissionPeer secondPeer, int arenaType)
		{
			this.FirstPlayerPeer = firstPeer;
			this.SecondPlayerPeer = secondPeer;
			this.FirstPlayerScore = 0;
			this.SecondPlayerScore = 0;
			this.FirstPlayer = new MPPlayerVM(firstPeer);
			this.SecondPlayer = new MPPlayerVM(secondPeer);
			this.FirstPlayer.RefreshDivision(true);
			this.SecondPlayer.RefreshDivision(true);
			this.ArenaType = arenaType;
			this.UpdateScore();
			this.IsEnabled = true;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0002C161 File Offset: 0x0002A361
		public void OnDuelEnded()
		{
			this.FirstPlayerPeer = null;
			this.SecondPlayerPeer = null;
			this.IsEnabled = false;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0002C178 File Offset: 0x0002A378
		public void OnPeerScored(MissionPeer peer)
		{
			if (peer == this.FirstPlayerPeer)
			{
				int num = this.FirstPlayerScore;
				this.FirstPlayerScore = num + 1;
			}
			else if (peer == this.SecondPlayerPeer)
			{
				int num = this.SecondPlayerScore;
				this.SecondPlayerScore = num + 1;
			}
			this.UpdateScore();
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002C1BF File Offset: 0x0002A3BF
		public void RefreshNames(bool changeGenericNames = false)
		{
			if (changeGenericNames)
			{
				this.FirstPlayer.Name = this.FirstPlayerPeer.DisplayedName;
				this.SecondPlayer.Name = this.SecondPlayerPeer.DisplayedName;
			}
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0002C1F0 File Offset: 0x0002A3F0
		private void UpdateScore()
		{
			GameTexts.SetVariable("LEFT", this.FirstPlayerScore);
			GameTexts.SetVariable("RIGHT", this.SecondPlayerScore);
			this.Score = GameTexts.FindText("str_LEFT_dash_RIGHT", null).ToString();
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0002C228 File Offset: 0x0002A428
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x0002C230 File Offset: 0x0002A430
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0002C24E File Offset: 0x0002A44E
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x0002C256 File Offset: 0x0002A456
		[DataSourceProperty]
		public bool IsPreparing
		{
			get
			{
				return this._isPreparing;
			}
			set
			{
				if (value != this._isPreparing)
				{
					this._isPreparing = value;
					base.OnPropertyChangedWithValue(value, "IsPreparing");
				}
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0002C274 File Offset: 0x0002A474
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x0002C27C File Offset: 0x0002A47C
		[DataSourceProperty]
		public string CountdownMessage
		{
			get
			{
				return this._countdownMessage;
			}
			set
			{
				if (value != this._countdownMessage)
				{
					this._countdownMessage = value;
					base.OnPropertyChangedWithValue<string>(value, "CountdownMessage");
				}
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002C29F File Offset: 0x0002A49F
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x0002C2A7 File Offset: 0x0002A4A7
		[DataSourceProperty]
		public string Score
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
					base.OnPropertyChangedWithValue<string>(value, "Score");
				}
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0002C2CA File Offset: 0x0002A4CA
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0002C2D2 File Offset: 0x0002A4D2
		[DataSourceProperty]
		public int ArenaType
		{
			get
			{
				return this._arenaType;
			}
			set
			{
				if (value != this._arenaType)
				{
					this._arenaType = value;
					base.OnPropertyChangedWithValue(value, "ArenaType");
				}
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0002C2F0 File Offset: 0x0002A4F0
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x0002C2F8 File Offset: 0x0002A4F8
		[DataSourceProperty]
		public int FirstPlayerScore
		{
			get
			{
				return this._firstPlayerScore;
			}
			set
			{
				if (value != this._firstPlayerScore)
				{
					this._firstPlayerScore = value;
					base.OnPropertyChangedWithValue(value, "FirstPlayerScore");
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0002C316 File Offset: 0x0002A516
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x0002C31E File Offset: 0x0002A51E
		[DataSourceProperty]
		public int SecondPlayerScore
		{
			get
			{
				return this._secondPlayerScore;
			}
			set
			{
				if (value != this._secondPlayerScore)
				{
					this._secondPlayerScore = value;
					base.OnPropertyChangedWithValue(value, "SecondPlayerScore");
				}
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0002C33C File Offset: 0x0002A53C
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x0002C344 File Offset: 0x0002A544
		[DataSourceProperty]
		public MPPlayerVM FirstPlayer
		{
			get
			{
				return this._firstPlayer;
			}
			set
			{
				if (value != this._firstPlayer)
				{
					this._firstPlayer = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "FirstPlayer");
				}
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0002C362 File Offset: 0x0002A562
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x0002C36A File Offset: 0x0002A56A
		[DataSourceProperty]
		public MPPlayerVM SecondPlayer
		{
			get
			{
				return this._secondPlayer;
			}
			set
			{
				if (value != this._secondPlayer)
				{
					this._secondPlayer = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "SecondPlayer");
				}
			}
		}

		// Token: 0x0400068E RID: 1678
		private float _prepTimeRemaining;

		// Token: 0x0400068F RID: 1679
		private TextObject _duelCountdownText;

		// Token: 0x04000690 RID: 1680
		private bool _isEnabled;

		// Token: 0x04000691 RID: 1681
		private bool _isPreparing;

		// Token: 0x04000692 RID: 1682
		private string _countdownMessage;

		// Token: 0x04000693 RID: 1683
		private string _score;

		// Token: 0x04000694 RID: 1684
		private int _arenaType;

		// Token: 0x04000695 RID: 1685
		private int _firstPlayerScore;

		// Token: 0x04000696 RID: 1686
		private int _secondPlayerScore;

		// Token: 0x04000697 RID: 1687
		private MPPlayerVM _firstPlayer;

		// Token: 0x04000698 RID: 1688
		private MPPlayerVM _secondPlayer;
	}
}
