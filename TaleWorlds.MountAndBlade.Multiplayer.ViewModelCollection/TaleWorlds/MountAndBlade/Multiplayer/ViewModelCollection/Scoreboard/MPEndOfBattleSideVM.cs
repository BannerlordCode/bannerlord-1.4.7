using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x02000022 RID: 34
	public class MPEndOfBattleSideVM : ViewModel
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00009B5B File Offset: 0x00007D5B
		// (set) Token: 0x06000260 RID: 608 RVA: 0x00009B63 File Offset: 0x00007D63
		public MissionScoreboardComponent.MissionScoreboardSide Side { get; private set; }

		// Token: 0x06000261 RID: 609 RVA: 0x00009B6C File Offset: 0x00007D6C
		public MPEndOfBattleSideVM(MissionScoreboardComponent missionScoreboardComponent, MissionScoreboardComponent.MissionScoreboardSide side, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColorInfo)
		{
			this._missionScoreboardComponent = missionScoreboardComponent;
			this.Side = side;
			this._culture = cultureColorInfo.Culture;
			if (this.Side != null)
			{
				this.CultureId = this._culture.StringId;
				this.Score = this.Side.SideScore;
				this.IsRoundWinner = this._missionScoreboardComponent.RoundWinner == side.Side || this._missionScoreboardComponent.RoundWinner == BattleSideEnum.None;
			}
			this.CultureColor1 = cultureColorInfo.Color1;
			this.CultureColor2 = cultureColorInfo.Color2;
			this.RefreshValues();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009C0B File Offset: 0x00007E0B
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Side != null)
			{
				this.CultureId = this._culture.StringId;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00009C2C File Offset: 0x00007E2C
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00009C34 File Offset: 0x00007E34
		[DataSourceProperty]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (value != this._factionName)
				{
					this._factionName = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionName");
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00009C57 File Offset: 0x00007E57
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00009C5F File Offset: 0x00007E5F
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00009C82 File Offset: 0x00007E82
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00009C8A File Offset: 0x00007E8A
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00009CA8 File Offset: 0x00007EA8
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00009CB0 File Offset: 0x00007EB0
		[DataSourceProperty]
		public bool IsRoundWinner
		{
			get
			{
				return this._isRoundWinner;
			}
			set
			{
				if (value != this._isRoundWinner)
				{
					this._isRoundWinner = value;
					base.OnPropertyChangedWithValue(value, "IsRoundWinner");
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00009CCE File Offset: 0x00007ECE
		// (set) Token: 0x0600026C RID: 620 RVA: 0x00009CD6 File Offset: 0x00007ED6
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00009CF9 File Offset: 0x00007EF9
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00009D01 File Offset: 0x00007F01
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

		// Token: 0x04000141 RID: 321
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x04000142 RID: 322
		private BasicCultureObject _culture;

		// Token: 0x04000143 RID: 323
		private string _factionName;

		// Token: 0x04000144 RID: 324
		private string _cultureId;

		// Token: 0x04000145 RID: 325
		private int _score;

		// Token: 0x04000146 RID: 326
		private bool _isRoundWinner;

		// Token: 0x04000147 RID: 327
		private Color _cultureColor1;

		// Token: 0x04000148 RID: 328
		private Color _cultureColor2;
	}
}
