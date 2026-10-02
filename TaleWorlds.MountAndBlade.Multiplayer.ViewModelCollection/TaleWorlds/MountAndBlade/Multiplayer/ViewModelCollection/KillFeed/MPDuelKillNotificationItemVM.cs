using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed
{
	// Token: 0x02000088 RID: 136
	public class MPDuelKillNotificationItemVM : ViewModel
	{
		// Token: 0x06000D4E RID: 3406 RVA: 0x000290E4 File Offset: 0x000272E4
		public MPDuelKillNotificationItemVM(MissionPeer firstPlayerPeer, MissionPeer secondPlayerPeer, int firstPlayerScore, int secondPlayerScore, TroopType arenaTroopType, Action<MPDuelKillNotificationItemVM> onRemove)
		{
			this._onRemove = onRemove;
			this.ArenaType = (int)arenaTroopType;
			this.FirstPlayerScore = firstPlayerScore;
			this.SecondPlayerScore = secondPlayerScore;
			int intValue = MultiplayerOptions.OptionType.MinScoreToWinDuel.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.IsEndOfDuel = this.FirstPlayerScore == intValue || this.SecondPlayerScore == intValue;
			this.InitProperties(firstPlayerPeer, secondPlayerPeer);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00029144 File Offset: 0x00027344
		public void InitProperties(MissionPeer firstPlayerPeer, MissionPeer secondPlayerPeer)
		{
			TargetIconType peerIconType = this.GetPeerIconType(firstPlayerPeer);
			this.FirstPlayerName = firstPlayerPeer.DisplayedName;
			this.FirstPlayerCompassElement = new MPTeammateCompassTargetVM(peerIconType, Color.White.ToUnsignedInteger(), Color.White.ToUnsignedInteger(), Banner.CreateOneColoredEmptyBanner(0), false);
			TargetIconType peerIconType2 = this.GetPeerIconType(secondPlayerPeer);
			this.SecondPlayerName = secondPlayerPeer.DisplayedName;
			this.SecondPlayerCompassElement = new MPTeammateCompassTargetVM(peerIconType2, Color.White.ToUnsignedInteger(), Color.White.ToUnsignedInteger(), Banner.CreateOneColoredEmptyBanner(0), false);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x000291D4 File Offset: 0x000273D4
		private TargetIconType GetPeerIconType(MissionPeer peer)
		{
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(peer, false);
			if (mpheroClassForPeer != null)
			{
				return mpheroClassForPeer.IconType;
			}
			return TargetIconType.None;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000291F4 File Offset: 0x000273F4
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00029202 File Offset: 0x00027402
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x0002920A File Offset: 0x0002740A
		[DataSourceProperty]
		public bool IsEndOfDuel
		{
			get
			{
				return this._isEndOfDuel;
			}
			set
			{
				if (value != this._isEndOfDuel)
				{
					this._isEndOfDuel = value;
					base.OnPropertyChangedWithValue(value, "IsEndOfDuel");
				}
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00029228 File Offset: 0x00027428
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x00029230 File Offset: 0x00027430
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

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0002924E File Offset: 0x0002744E
		// (set) Token: 0x06000D57 RID: 3415 RVA: 0x00029256 File Offset: 0x00027456
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

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00029274 File Offset: 0x00027474
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x0002927C File Offset: 0x0002747C
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

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x0002929A File Offset: 0x0002749A
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x000292A2 File Offset: 0x000274A2
		[DataSourceProperty]
		public string FirstPlayerName
		{
			get
			{
				return this._firstPlayerName;
			}
			set
			{
				if (value != this._firstPlayerName)
				{
					this._firstPlayerName = value;
					base.OnPropertyChangedWithValue<string>(value, "FirstPlayerName");
				}
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x000292C5 File Offset: 0x000274C5
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x000292CD File Offset: 0x000274CD
		[DataSourceProperty]
		public string SecondPlayerName
		{
			get
			{
				return this._secondPlayerName;
			}
			set
			{
				if (value != this._secondPlayerName)
				{
					this._secondPlayerName = value;
					base.OnPropertyChangedWithValue<string>(value, "SecondPlayerName");
				}
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x000292F0 File Offset: 0x000274F0
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x000292F8 File Offset: 0x000274F8
		[DataSourceProperty]
		public MPTeammateCompassTargetVM FirstPlayerCompassElement
		{
			get
			{
				return this._firstPlayerCompassElement;
			}
			set
			{
				if (value != this._firstPlayerCompassElement)
				{
					this._firstPlayerCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "FirstPlayerCompassElement");
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00029316 File Offset: 0x00027516
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x0002931E File Offset: 0x0002751E
		[DataSourceProperty]
		public MPTeammateCompassTargetVM SecondPlayerCompassElement
		{
			get
			{
				return this._secondPlayerCompassElement;
			}
			set
			{
				if (value != this._secondPlayerCompassElement)
				{
					this._secondPlayerCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "SecondPlayerCompassElement");
				}
			}
		}

		// Token: 0x04000616 RID: 1558
		private Action<MPDuelKillNotificationItemVM> _onRemove;

		// Token: 0x04000617 RID: 1559
		private bool _isEndOfDuel;

		// Token: 0x04000618 RID: 1560
		private int _arenaType;

		// Token: 0x04000619 RID: 1561
		private int _firstPlayerScore;

		// Token: 0x0400061A RID: 1562
		private int _secondPlayerScore;

		// Token: 0x0400061B RID: 1563
		private string _firstPlayerName;

		// Token: 0x0400061C RID: 1564
		private string _secondPlayerName;

		// Token: 0x0400061D RID: 1565
		private MPTeammateCompassTargetVM _firstPlayerCompassElement;

		// Token: 0x0400061E RID: 1566
		private MPTeammateCompassTargetVM _secondPlayerCompassElement;
	}
}
