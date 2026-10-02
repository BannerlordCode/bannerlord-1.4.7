using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tournament
{
	// Token: 0x02000053 RID: 83
	public class TournamentScreenWidget : Widget
	{
		// Token: 0x06000486 RID: 1158 RVA: 0x0000E632 File Offset: 0x0000C832
		public TournamentScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000E63B File Offset: 0x0000C83B
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._isAnimationActive && this.IsOver)
			{
				this.StartBattleResultAnimation();
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000E65C File Offset: 0x0000C85C
		private void StartBattleResultAnimation()
		{
			this._isAnimationActive = true;
			DelayedStateChanger shieldStateChanger = this.ShieldStateChanger;
			if (shieldStateChanger != null)
			{
				shieldStateChanger.Start();
			}
			DelayedStateChanger winnerTextContainer = this.WinnerTextContainer1;
			if (winnerTextContainer != null)
			{
				winnerTextContainer.Start();
			}
			DelayedStateChanger characterContainer = this.CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.Start();
			}
			DelayedStateChanger rewardsContainer = this.RewardsContainer;
			if (rewardsContainer != null)
			{
				rewardsContainer.Start();
			}
			DelayedStateChanger flagsSuccess = this.FlagsSuccess;
			if (flagsSuccess != null)
			{
				flagsSuccess.Start();
			}
			ScoreboardBattleRewardsWidget scoreboardBattleRewardsWidget = this.ScoreboardBattleRewardsWidget;
			if (scoreboardBattleRewardsWidget == null)
			{
				return;
			}
			scoreboardBattleRewardsWidget.StartAnimation();
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000E6D5 File Offset: 0x0000C8D5
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x0000E6DD File Offset: 0x0000C8DD
		[Editor(false)]
		public bool IsOver
		{
			get
			{
				return this._isOver;
			}
			set
			{
				if (this._isOver != value)
				{
					this._isOver = value;
					base.OnPropertyChanged(value, "IsOver");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0000E6FB File Offset: 0x0000C8FB
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x0000E703 File Offset: 0x0000C903
		[Editor(false)]
		public DelayedStateChanger FlagsSuccess
		{
			get
			{
				return this._flagsSuccess;
			}
			set
			{
				if (this._flagsSuccess != value)
				{
					this._flagsSuccess = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "FlagsSuccess");
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000E721 File Offset: 0x0000C921
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x0000E729 File Offset: 0x0000C929
		[Editor(false)]
		public DelayedStateChanger ShieldStateChanger
		{
			get
			{
				return this._shieldStateChanger;
			}
			set
			{
				if (this._shieldStateChanger != value)
				{
					this._shieldStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "ShieldStateChanger");
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0000E747 File Offset: 0x0000C947
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0000E74F File Offset: 0x0000C94F
		[Editor(false)]
		public DelayedStateChanger WinnerTextContainer1
		{
			get
			{
				return this._winnerTextContainer1;
			}
			set
			{
				if (this._winnerTextContainer1 != value)
				{
					this._winnerTextContainer1 = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "WinnerTextContainer1");
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000E76D File Offset: 0x0000C96D
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x0000E775 File Offset: 0x0000C975
		[Editor(false)]
		public DelayedStateChanger CharacterContainer
		{
			get
			{
				return this._characterContainer;
			}
			set
			{
				if (this._characterContainer != value)
				{
					this._characterContainer = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "CharacterContainer");
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000E793 File Offset: 0x0000C993
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0000E79B File Offset: 0x0000C99B
		[Editor(false)]
		public DelayedStateChanger RewardsContainer
		{
			get
			{
				return this._rewardsContainer;
			}
			set
			{
				if (this._rewardsContainer != value)
				{
					this._rewardsContainer = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "RewardsContainer");
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000E7B9 File Offset: 0x0000C9B9
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
		[Editor(false)]
		public ScoreboardBattleRewardsWidget ScoreboardBattleRewardsWidget
		{
			get
			{
				return this._scoreboardBattleRewardsWidget;
			}
			set
			{
				if (this._scoreboardBattleRewardsWidget != value)
				{
					this._scoreboardBattleRewardsWidget = value;
					base.OnPropertyChanged<ScoreboardBattleRewardsWidget>(value, "ScoreboardBattleRewardsWidget");
				}
			}
		}

		// Token: 0x040001EF RID: 495
		private bool _isAnimationActive;

		// Token: 0x040001F0 RID: 496
		private bool _isOver;

		// Token: 0x040001F1 RID: 497
		private DelayedStateChanger _flagsSuccess;

		// Token: 0x040001F2 RID: 498
		private DelayedStateChanger _shieldStateChanger;

		// Token: 0x040001F3 RID: 499
		private DelayedStateChanger _winnerTextContainer1;

		// Token: 0x040001F4 RID: 500
		private DelayedStateChanger _characterContainer;

		// Token: 0x040001F5 RID: 501
		private DelayedStateChanger _rewardsContainer;

		// Token: 0x040001F6 RID: 502
		private ScoreboardBattleRewardsWidget _scoreboardBattleRewardsWidget;
	}
}
