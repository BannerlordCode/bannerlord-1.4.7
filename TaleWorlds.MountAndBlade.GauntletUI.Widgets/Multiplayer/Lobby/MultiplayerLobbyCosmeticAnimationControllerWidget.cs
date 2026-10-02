using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A1 RID: 161
	public class MultiplayerLobbyCosmeticAnimationControllerWidget : Widget
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x00018B47 File Offset: 0x00016D47
		private double GetRandomDoubleBetween(double min, double max)
		{
			return base.Context.UIRandom.NextDouble() * (max - min) + max;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00018B60 File Offset: 0x00016D60
		public MultiplayerLobbyCosmeticAnimationControllerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00018BB6 File Offset: 0x00016DB6
		private void RestartAllAnimations()
		{
			this.SetAllAnimationPartColors();
			this.StopAllAnimations();
			this.StartAllAnimations();
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00018BCA File Offset: 0x00016DCA
		private void SetAllAnimationPartColors()
		{
			this.ApplyActionOnAllAnimations(new Action<MultiplayerLobbyCosmeticAnimationPartWidget>(this.SetColorOfPart));
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00018BDE File Offset: 0x00016DDE
		private void StartAllAnimations()
		{
			this.ApplyActionOnAllAnimations(new Action<MultiplayerLobbyCosmeticAnimationPartWidget>(this.StartAnimationOfPart));
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00018BF2 File Offset: 0x00016DF2
		private void StopAllAnimations()
		{
			this.ApplyActionOnAllAnimations(new Action<MultiplayerLobbyCosmeticAnimationPartWidget>(this.StopAnimationOfPart));
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00018C08 File Offset: 0x00016E08
		private void StartAnimationOfPart(MultiplayerLobbyCosmeticAnimationPartWidget part)
		{
			double randomDoubleBetween = this.GetRandomDoubleBetween((double)this.MinAlphaChangeDuration, (double)this.MaxAlphaChangeDuration);
			double randomDoubleBetween2 = this.GetRandomDoubleBetween((double)this.MinAlphaLowerBound, (double)this.MinAlphaUpperBound);
			double randomDoubleBetween3 = this.GetRandomDoubleBetween((double)this.MaxAlphaLowerBound, (double)this.MaxAlphaUpperBound);
			part.StartAnimation((float)randomDoubleBetween, (float)randomDoubleBetween2, (float)randomDoubleBetween3);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00018C60 File Offset: 0x00016E60
		private void StopAnimationOfPart(MultiplayerLobbyCosmeticAnimationPartWidget part)
		{
			part.StopAnimation();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00018C68 File Offset: 0x00016E68
		private void SetColorOfPart(MultiplayerLobbyCosmeticAnimationPartWidget part)
		{
			switch (this.CosmeticRarity)
			{
			case 0:
			case 1:
				part.Color = this.RarityCommonColor;
				return;
			case 2:
				part.Color = this.RarityRareColor;
				return;
			case 3:
				part.Color = this.RarityUniqueColor;
				return;
			default:
				part.Color = MultiplayerLobbyCosmeticAnimationControllerWidget.DefaultColor;
				return;
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00018CC8 File Offset: 0x00016EC8
		private void ApplyActionOnAllAnimations(Action<MultiplayerLobbyCosmeticAnimationPartWidget> action)
		{
			BasicContainer animationPartContainer = this.AnimationPartContainer;
			if (animationPartContainer == null)
			{
				return;
			}
			animationPartContainer.Children.ForEach(delegate(Widget c)
			{
				action(c as MultiplayerLobbyCosmeticAnimationPartWidget);
			});
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00018D04 File Offset: 0x00016F04
		private void OnAnimationPartAdded(Widget parent, Widget child)
		{
			MultiplayerLobbyCosmeticAnimationPartWidget multiplayerLobbyCosmeticAnimationPartWidget = child as MultiplayerLobbyCosmeticAnimationPartWidget;
			this.SetColorOfPart(multiplayerLobbyCosmeticAnimationPartWidget);
			this.StartAnimationOfPart(multiplayerLobbyCosmeticAnimationPartWidget);
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x00018D26 File Offset: 0x00016F26
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x00018D2E File Offset: 0x00016F2E
		[Editor(false)]
		public int CosmeticRarity
		{
			get
			{
				return this._cosmeticRarity;
			}
			set
			{
				if (value != this._cosmeticRarity)
				{
					this._cosmeticRarity = value;
					base.OnPropertyChanged(value, "CosmeticRarity");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00018D52 File Offset: 0x00016F52
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00018D5A File Offset: 0x00016F5A
		[Editor(false)]
		public float MinAlphaChangeDuration
		{
			get
			{
				return this._minAlphaChangeDuration;
			}
			set
			{
				if (this._minAlphaChangeDuration != value)
				{
					this._minAlphaChangeDuration = value;
					base.OnPropertyChanged(value, "MinAlphaChangeDuration");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00018D7E File Offset: 0x00016F7E
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00018D86 File Offset: 0x00016F86
		[Editor(false)]
		public float MaxAlphaChangeDuration
		{
			get
			{
				return this._maxAlphaChangeDuration;
			}
			set
			{
				if (this._maxAlphaChangeDuration != value)
				{
					this._maxAlphaChangeDuration = value;
					base.OnPropertyChanged(value, "MaxAlphaChangeDuration");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00018DAA File Offset: 0x00016FAA
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x00018DB2 File Offset: 0x00016FB2
		[Editor(false)]
		public float MinAlphaLowerBound
		{
			get
			{
				return this._minAlphaLowerBound;
			}
			set
			{
				if (this._minAlphaLowerBound != value)
				{
					this._minAlphaLowerBound = value;
					base.OnPropertyChanged(value, "MinAlphaLowerBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00018DD6 File Offset: 0x00016FD6
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00018DDE File Offset: 0x00016FDE
		[Editor(false)]
		public float MinAlphaUpperBound
		{
			get
			{
				return this._minAlphaUpperBound;
			}
			set
			{
				if (this._minAlphaUpperBound != value)
				{
					this._minAlphaUpperBound = value;
					base.OnPropertyChanged(value, "MinAlphaUpperBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00018E02 File Offset: 0x00017002
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x00018E0A File Offset: 0x0001700A
		[Editor(false)]
		public float MaxAlphaLowerBound
		{
			get
			{
				return this._maxAlphaLowerBound;
			}
			set
			{
				if (this._maxAlphaLowerBound != value)
				{
					this._maxAlphaLowerBound = value;
					base.OnPropertyChanged(value, "MaxAlphaLowerBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00018E2E File Offset: 0x0001702E
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x00018E36 File Offset: 0x00017036
		[Editor(false)]
		public float MaxAlphaUpperBound
		{
			get
			{
				return this._maxAlphaUpperBound;
			}
			set
			{
				if (this._maxAlphaUpperBound != value)
				{
					this._maxAlphaUpperBound = value;
					base.OnPropertyChanged(value, "MaxAlphaUpperBound");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x00018E5A File Offset: 0x0001705A
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x00018E62 File Offset: 0x00017062
		[Editor(false)]
		public Color RarityCommonColor
		{
			get
			{
				return this._rarityCommonColor;
			}
			set
			{
				if (this._rarityCommonColor != value)
				{
					this._rarityCommonColor = value;
					base.OnPropertyChanged(value, "RarityCommonColor");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00018E8B File Offset: 0x0001708B
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x00018E93 File Offset: 0x00017093
		[Editor(false)]
		public Color RarityRareColor
		{
			get
			{
				return this._rarityRareColor;
			}
			set
			{
				if (this._rarityRareColor != value)
				{
					this._rarityRareColor = value;
					base.OnPropertyChanged(value, "RarityRareColor");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00018EBC File Offset: 0x000170BC
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00018EC4 File Offset: 0x000170C4
		[Editor(false)]
		public Color RarityUniqueColor
		{
			get
			{
				return this._rarityUniqueColor;
			}
			set
			{
				if (this._rarityUniqueColor != value)
				{
					this._rarityUniqueColor = value;
					base.OnPropertyChanged(value, "RarityUniqueColor");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00018EED File Offset: 0x000170ED
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00018EF8 File Offset: 0x000170F8
		[Editor(false)]
		public BasicContainer AnimationPartContainer
		{
			get
			{
				return this._animationPartContainer;
			}
			set
			{
				if (value != this._animationPartContainer)
				{
					if (this._animationPartContainer != null)
					{
						this._animationPartContainer.ItemAddEventHandlers.Remove(new Action<Widget, Widget>(this.OnAnimationPartAdded));
					}
					this._animationPartContainer = value;
					if (this._animationPartContainer != null)
					{
						this._animationPartContainer.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnAnimationPartAdded));
					}
					base.OnPropertyChanged<BasicContainer>(value, "AnimationPartContainer");
					this.RestartAllAnimations();
				}
			}
		}

		// Token: 0x040003E4 RID: 996
		private static readonly Color DefaultColor = Color.FromUint(0U);

		// Token: 0x040003E5 RID: 997
		private int _cosmeticRarity;

		// Token: 0x040003E6 RID: 998
		private float _minAlphaChangeDuration = 1.5f;

		// Token: 0x040003E7 RID: 999
		private float _maxAlphaChangeDuration = 2.5f;

		// Token: 0x040003E8 RID: 1000
		private float _minAlphaLowerBound = 0.4f;

		// Token: 0x040003E9 RID: 1001
		private float _minAlphaUpperBound = 0.6f;

		// Token: 0x040003EA RID: 1002
		private float _maxAlphaLowerBound = 0.6f;

		// Token: 0x040003EB RID: 1003
		private float _maxAlphaUpperBound = 0.8f;

		// Token: 0x040003EC RID: 1004
		private Color _rarityCommonColor;

		// Token: 0x040003ED RID: 1005
		private Color _rarityRareColor;

		// Token: 0x040003EE RID: 1006
		private Color _rarityUniqueColor;

		// Token: 0x040003EF RID: 1007
		private BasicContainer _animationPartContainer;
	}
}
