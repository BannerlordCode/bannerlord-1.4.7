using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009C RID: 156
	public class MultiplayerLobbyAfterBattlePopupWidget : Widget
	{
		// Token: 0x06000856 RID: 2134 RVA: 0x00017F09 File Offset: 0x00016109
		public MultiplayerLobbyAfterBattlePopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00017F14 File Offset: 0x00016114
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.IsActive = base.IsVisible;
			if (this._isActive)
			{
				this._timePassed += dt;
			}
			if (this._isFinished)
			{
				return;
			}
			if (this._timePassed >= this.AnimationDuration + this.AnimationDelay + (float)this._currentRewardIndex * this.RewardRevealDuration && this._currentRewardIndex < this.RewardsListPanel.Children.Count)
			{
				(this.RewardsListPanel.Children[this._currentRewardIndex] as MultiplayerLobbyBattleRewardWidget).StartAnimation();
				this._currentRewardIndex++;
			}
			if (this._timePassed >= this.AnimationDelay + this.AnimationDuration + (float)this.RewardsListPanel.Children.Count * this.RewardRevealDuration)
			{
				this._isFinished = true;
				this.ClickToContinueTextWidget.IsVisible = true;
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00018000 File Offset: 0x00016200
		public void StartAnimation()
		{
			foreach (Widget widget in this.RewardsListPanel.Children)
			{
				(widget as MultiplayerLobbyBattleRewardWidget).StartPreAnimation();
			}
			this._isFinished = false;
			this._timePassed = 0f;
			this._currentRewardIndex = 0;
			this.ClickToContinueTextWidget.IsVisible = false;
			this.ExperiencePanel.StartAnimation(this.AnimationDelay);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00018090 File Offset: 0x00016290
		private void Reset()
		{
			this.ExperiencePanel.Reset();
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001809D File Offset: 0x0001629D
		private void IsActiveUpdated()
		{
			if (this.IsActive)
			{
				this.StartAnimation();
				return;
			}
			this.Reset();
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000180B4 File Offset: 0x000162B4
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x000180BC File Offset: 0x000162BC
		[Editor(false)]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					this.IsActiveUpdated();
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x000180E0 File Offset: 0x000162E0
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x000180E8 File Offset: 0x000162E8
		[Editor(false)]
		public float AnimationDelay
		{
			get
			{
				return this._animationDelay;
			}
			set
			{
				if (value != this._animationDelay)
				{
					this._animationDelay = value;
					base.OnPropertyChanged(value, "AnimationDelay");
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00018106 File Offset: 0x00016306
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0001810E File Offset: 0x0001630E
		[Editor(false)]
		public float AnimationDuration
		{
			get
			{
				return this._animationDuration;
			}
			set
			{
				if (value != this._animationDuration)
				{
					this._animationDuration = value;
					base.OnPropertyChanged(value, "AnimationDuration");
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001812C File Offset: 0x0001632C
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x00018134 File Offset: 0x00016334
		[Editor(false)]
		public float RewardRevealDuration
		{
			get
			{
				return this._rewardRevealDuration;
			}
			set
			{
				if (value != this._rewardRevealDuration)
				{
					this._rewardRevealDuration = value;
					base.OnPropertyChanged(value, "RewardRevealDuration");
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00018152 File Offset: 0x00016352
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0001815A File Offset: 0x0001635A
		[Editor(false)]
		public MultiplayerLobbyAfterBattleExperiencePanelWidget ExperiencePanel
		{
			get
			{
				return this._experiencePanel;
			}
			set
			{
				if (value != this._experiencePanel)
				{
					this._experiencePanel = value;
					base.OnPropertyChanged<MultiplayerLobbyAfterBattleExperiencePanelWidget>(value, "ExperiencePanel");
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00018178 File Offset: 0x00016378
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x00018180 File Offset: 0x00016380
		[Editor(false)]
		public TextWidget ClickToContinueTextWidget
		{
			get
			{
				return this._clickToContinueTextWidget;
			}
			set
			{
				if (value != this._clickToContinueTextWidget)
				{
					this._clickToContinueTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "ClickToContinueTextWidget");
				}
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0001819E File Offset: 0x0001639E
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x000181A6 File Offset: 0x000163A6
		[Editor(false)]
		public ListPanel RewardsListPanel
		{
			get
			{
				return this._rewardsListPanel;
			}
			set
			{
				if (value != this._rewardsListPanel)
				{
					this._rewardsListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "RewardsListPanel");
				}
			}
		}

		// Token: 0x040003B6 RID: 950
		private bool _isFinished;

		// Token: 0x040003B7 RID: 951
		private float _timePassed;

		// Token: 0x040003B8 RID: 952
		private int _currentRewardIndex;

		// Token: 0x040003B9 RID: 953
		private bool _isActive;

		// Token: 0x040003BA RID: 954
		private float _animationDelay;

		// Token: 0x040003BB RID: 955
		private float _animationDuration;

		// Token: 0x040003BC RID: 956
		private float _rewardRevealDuration;

		// Token: 0x040003BD RID: 957
		private MultiplayerLobbyAfterBattleExperiencePanelWidget _experiencePanel;

		// Token: 0x040003BE RID: 958
		private TextWidget _clickToContinueTextWidget;

		// Token: 0x040003BF RID: 959
		private ListPanel _rewardsListPanel;
	}
}
