using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009D RID: 157
	public class MultiplayerLobbyAnimatedRankChangeWidget : Widget
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x000181C4 File Offset: 0x000163C4
		public MultiplayerLobbyAnimatedRankChangeWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000181E4 File Offset: 0x000163E4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.IsAnimationRequested)
			{
				return;
			}
			if (this._preAnimationTimeElapsed < this._animationDelay)
			{
				this._preAnimationTimeElapsed += dt;
				return;
			}
			if (this._animationTimeElapsed >= this._animationDuration)
			{
				this.NewRankName.SetGlobalAlphaRecursively(1f);
				this.NewRankSprite.SetGlobalAlphaRecursively(1f);
				this.OldRankName.SetGlobalAlphaRecursively(0f);
				this.OldRankSprite.SetGlobalAlphaRecursively(0f);
				this.NewRankSprite.ScaledSuggestedWidth = base.ScaledSuggestedWidth / 2f;
				this.NewRankSprite.ScaledSuggestedHeight = base.ScaledSuggestedHeight / 2f;
				return;
			}
			float num = MathF.Lerp(0f, 1f, this._animationTimeElapsed / this._animationDuration, 1E-05f);
			this.OldRankSprite.SetGlobalAlphaRecursively(1f - num);
			this.OldRankName.SetGlobalAlphaRecursively(1f - num);
			this.NewRankSprite.SetGlobalAlphaRecursively(num);
			this.NewRankName.SetGlobalAlphaRecursively(num);
			this.NewRankSprite.ScaledSuggestedWidth = MathF.Lerp(base.ScaledSuggestedWidth, base.ScaledSuggestedWidth / 2f, this._animationTimeElapsed / this._animationDuration, 1E-05f);
			this.NewRankSprite.ScaledSuggestedHeight = MathF.Lerp(base.ScaledSuggestedHeight, base.ScaledSuggestedHeight / 2f, this._animationTimeElapsed / this._animationDuration, 1E-05f);
			this._animationTimeElapsed += dt;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00018370 File Offset: 0x00016570
		private void StartAnimation()
		{
			this.NewRankName.SetGlobalAlphaRecursively(0f);
			this.NewRankSprite.SetGlobalAlphaRecursively(0f);
			this.OldRankName.SetGlobalAlphaRecursively(1f);
			this.OldRankSprite.SetGlobalAlphaRecursively(1f);
			this.OldRankSprite.ScaledSuggestedWidth = base.ScaledSuggestedWidth / 2f;
			this.OldRankSprite.ScaledSuggestedHeight = base.ScaledSuggestedHeight / 2f;
			this._preAnimationTimeElapsed = 0f;
			this._animationTimeElapsed = 0f;
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00018401 File Offset: 0x00016601
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00018409 File Offset: 0x00016609
		[Editor(false)]
		public bool IsAnimationRequested
		{
			get
			{
				return this._isAnimationRequested;
			}
			set
			{
				if (value != this._isAnimationRequested)
				{
					this._isAnimationRequested = value;
					base.OnPropertyChanged(value, "IsAnimationRequested");
					this.StartAnimation();
				}
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0001842D File Offset: 0x0001662D
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x00018435 File Offset: 0x00016635
		[Editor(false)]
		public bool IsPromoted
		{
			get
			{
				return this._isPromoted;
			}
			set
			{
				if (value != this._isPromoted)
				{
					this._isPromoted = value;
					base.OnPropertyChanged(value, "IsPromoted");
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00018453 File Offset: 0x00016653
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0001845B File Offset: 0x0001665B
		[Editor(false)]
		public TextWidget OldRankName
		{
			get
			{
				return this._oldRankName;
			}
			set
			{
				if (value != this._oldRankName)
				{
					this._oldRankName = value;
					base.OnPropertyChanged<TextWidget>(value, "OldRankName");
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00018479 File Offset: 0x00016679
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x00018481 File Offset: 0x00016681
		[Editor(false)]
		public TextWidget NewRankName
		{
			get
			{
				return this._newRankName;
			}
			set
			{
				if (value != this._newRankName)
				{
					this._newRankName = value;
					base.OnPropertyChanged<TextWidget>(value, "NewRankName");
				}
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0001849F File Offset: 0x0001669F
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x000184A7 File Offset: 0x000166A7
		[Editor(false)]
		public MultiplayerLobbyRankItemButtonWidget OldRankSprite
		{
			get
			{
				return this._oldRankSprite;
			}
			set
			{
				if (value != this._oldRankSprite)
				{
					this._oldRankSprite = value;
					base.OnPropertyChanged<MultiplayerLobbyRankItemButtonWidget>(value, "OldRankSprite");
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x000184C5 File Offset: 0x000166C5
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x000184CD File Offset: 0x000166CD
		[Editor(false)]
		public MultiplayerLobbyRankItemButtonWidget NewRankSprite
		{
			get
			{
				return this._newRankSprite;
			}
			set
			{
				if (value != this._newRankSprite)
				{
					this._newRankSprite = value;
					base.OnPropertyChanged<MultiplayerLobbyRankItemButtonWidget>(value, "NewRankSprite");
				}
			}
		}

		// Token: 0x040003C0 RID: 960
		private float _animationTimeElapsed;

		// Token: 0x040003C1 RID: 961
		private float _animationDuration = 0.25f;

		// Token: 0x040003C2 RID: 962
		private float _preAnimationTimeElapsed;

		// Token: 0x040003C3 RID: 963
		private float _animationDelay = 0.5f;

		// Token: 0x040003C4 RID: 964
		private bool _isAnimationRequested;

		// Token: 0x040003C5 RID: 965
		private bool _isPromoted;

		// Token: 0x040003C6 RID: 966
		private TextWidget _oldRankName;

		// Token: 0x040003C7 RID: 967
		private TextWidget _newRankName;

		// Token: 0x040003C8 RID: 968
		private MultiplayerLobbyRankItemButtonWidget _oldRankSprite;

		// Token: 0x040003C9 RID: 969
		private MultiplayerLobbyRankItemButtonWidget _newRankSprite;
	}
}
