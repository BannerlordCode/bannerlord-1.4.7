using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009B RID: 155
	public class MultiplayerLobbyAfterBattleExperiencePanelWidget : Widget
	{
		// Token: 0x06000847 RID: 2119 RVA: 0x00017D57 File Offset: 0x00015F57
		public MultiplayerLobbyAfterBattleExperiencePanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00017D60 File Offset: 0x00015F60
		public void StartAnimation(float animationDelay)
		{
			this.ExperienceFillBar.StartAnimation(animationDelay);
			this.EarnedExperienceCounterTextWidget.IntTarget = this.GainedExperience;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00017D7F File Offset: 0x00015F7F
		public void Reset()
		{
			MultiplayerScoreboardAnimatedFillBarWidget experienceFillBar = this.ExperienceFillBar;
			if (experienceFillBar != null)
			{
				experienceFillBar.Reset();
			}
			CounterTextBrushWidget earnedExperienceCounterTextWidget = this.EarnedExperienceCounterTextWidget;
			if (earnedExperienceCounterTextWidget == null)
			{
				return;
			}
			earnedExperienceCounterTextWidget.SetInitialValue(0f);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00017DA7 File Offset: 0x00015FA7
		private void OnFillBarFill(bool isPositive)
		{
			this.CurrentLevelTextWidget.IntText += (isPositive ? 1 : (-1));
			this.NextLevelTextWidget.IntText += (isPositive ? 1 : (-1));
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00017DDB File Offset: 0x00015FDB
		protected override void RefreshState()
		{
			if (base.IsHidden)
			{
				MultiplayerScoreboardAnimatedFillBarWidget experienceFillBar = this.ExperienceFillBar;
				if (experienceFillBar == null)
				{
					return;
				}
				experienceFillBar.Reset();
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00017DF5 File Offset: 0x00015FF5
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x00017DFD File Offset: 0x00015FFD
		[Editor(false)]
		public int GainedExperience
		{
			get
			{
				return this._gainedExperience;
			}
			set
			{
				if (value != this._gainedExperience)
				{
					this._gainedExperience = value;
					base.OnPropertyChanged(value, "GainedExperience");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x00017E1B File Offset: 0x0001601B
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x00017E24 File Offset: 0x00016024
		[Editor(false)]
		public MultiplayerScoreboardAnimatedFillBarWidget ExperienceFillBar
		{
			get
			{
				return this._experienceFillBar;
			}
			set
			{
				if (value != this._experienceFillBar)
				{
					if (this._experienceFillBar != null)
					{
						this._experienceFillBar.OnFullFillFinished -= this.OnFillBarFill;
					}
					this._experienceFillBar = value;
					if (this._experienceFillBar != null)
					{
						this._experienceFillBar.OnFullFillFinished += this.OnFillBarFill;
					}
					base.OnPropertyChanged<MultiplayerScoreboardAnimatedFillBarWidget>(value, "ExperienceFillBar");
					this.Reset();
				}
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x00017E91 File Offset: 0x00016091
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x00017E99 File Offset: 0x00016099
		[Editor(false)]
		public CounterTextBrushWidget EarnedExperienceCounterTextWidget
		{
			get
			{
				return this._earnedExperienceCounterTextWidget;
			}
			set
			{
				if (value != this._earnedExperienceCounterTextWidget)
				{
					this._earnedExperienceCounterTextWidget = value;
					base.OnPropertyChanged<CounterTextBrushWidget>(value, "EarnedExperienceCounterTextWidget");
					this.Reset();
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00017EBD File Offset: 0x000160BD
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x00017EC5 File Offset: 0x000160C5
		[Editor(false)]
		public TextWidget CurrentLevelTextWidget
		{
			get
			{
				return this._currentLevelTextWidget;
			}
			set
			{
				if (value != this._currentLevelTextWidget)
				{
					this._currentLevelTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "CurrentLevelTextWidget");
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00017EE3 File Offset: 0x000160E3
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x00017EEB File Offset: 0x000160EB
		public TextWidget NextLevelTextWidget
		{
			get
			{
				return this._nextLevelTextWidget;
			}
			set
			{
				if (value != this._nextLevelTextWidget)
				{
					this._nextLevelTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NextLevelTextWidget");
				}
			}
		}

		// Token: 0x040003B1 RID: 945
		private int _gainedExperience;

		// Token: 0x040003B2 RID: 946
		private MultiplayerScoreboardAnimatedFillBarWidget _experienceFillBar;

		// Token: 0x040003B3 RID: 947
		private CounterTextBrushWidget _earnedExperienceCounterTextWidget;

		// Token: 0x040003B4 RID: 948
		private TextWidget _currentLevelTextWidget;

		// Token: 0x040003B5 RID: 949
		private TextWidget _nextLevelTextWidget;
	}
}
