using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DB RID: 219
	public class CrosshairWidget : Widget
	{
		// Token: 0x06000B1B RID: 2843 RVA: 0x0001F361 File Offset: 0x0001D561
		public CrosshairWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0001F36C File Offset: 0x0001D56C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsVisible)
			{
				base.SuggestedWidth = (float)((int)(74.0 + this.CrosshairAccuracy * 300.0));
				base.SuggestedHeight = (float)((int)(74.0 + this.CrosshairAccuracy * 300.0));
			}
			this.LeftArrow.Brush.AlphaFactor = (float)this.LeftArrowOpacity;
			this.RightArrow.Brush.AlphaFactor = (float)this.RightArrowOpacity;
			this.TopArrow.Brush.AlphaFactor = (float)this.TopArrowOpacity;
			this.BottomArrow.Brush.AlphaFactor = (float)this.BottomArrowOpacity;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0001F428 File Offset: 0x0001D628
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.AddState("Invalid");
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0001F43C File Offset: 0x0001D63C
		private void HitMarkerUpdated()
		{
			if (this.HitMarker != null)
			{
				this.HitMarker.AddState("Show");
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0001F456 File Offset: 0x0001D656
		private void HeadshotMarkerUpdated()
		{
			if (this.HeadshotMarker != null)
			{
				this.HitMarker.AddState("Show");
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0001F470 File Offset: 0x0001D670
		private void ShowHitMarkerChanged()
		{
			if (this.HitMarker == null)
			{
				return;
			}
			string text = (this.IsVictimDead ? "ShowDeath" : "Show");
			if (this.HitMarker.CurrentState != text)
			{
				this.HitMarker.SetState(text);
				return;
			}
			this.HitMarker.BrushRenderer.RestartAnimation();
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0001F4CC File Offset: 0x0001D6CC
		private void ShowHeadshotMarkerChanged()
		{
			if (this.HeadshotMarker == null)
			{
				return;
			}
			string text = (this.IsHumanoidHeadshot ? "Show" : "Default");
			if (this.HeadshotMarker.CurrentState != text)
			{
				this.HeadshotMarker.SetState(text);
			}
			this.HeadshotMarker.BrushRenderer.RestartAnimation();
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0001F526 File Offset: 0x0001D726
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x0001F52E File Offset: 0x0001D72E
		[Editor(false)]
		public double TopArrowOpacity
		{
			get
			{
				return this._topArrowOpacity;
			}
			set
			{
				if (value != this._topArrowOpacity)
				{
					this._topArrowOpacity = value;
					base.OnPropertyChanged(value, "TopArrowOpacity");
				}
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0001F54C File Offset: 0x0001D74C
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x0001F554 File Offset: 0x0001D754
		[Editor(false)]
		public double BottomArrowOpacity
		{
			get
			{
				return this._bottomArrowOpacity;
			}
			set
			{
				if (value != this._bottomArrowOpacity)
				{
					this._bottomArrowOpacity = value;
					base.OnPropertyChanged(value, "BottomArrowOpacity");
				}
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0001F572 File Offset: 0x0001D772
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x0001F57A File Offset: 0x0001D77A
		[Editor(false)]
		public double RightArrowOpacity
		{
			get
			{
				return this._rightArrowOpacity;
			}
			set
			{
				if (value != this._rightArrowOpacity)
				{
					this._rightArrowOpacity = value;
					base.OnPropertyChanged(value, "RightArrowOpacity");
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0001F598 File Offset: 0x0001D798
		// (set) Token: 0x06000B29 RID: 2857 RVA: 0x0001F5A0 File Offset: 0x0001D7A0
		[Editor(false)]
		public double LeftArrowOpacity
		{
			get
			{
				return this._leftArrowOpacity;
			}
			set
			{
				if (value != this._leftArrowOpacity)
				{
					this._leftArrowOpacity = value;
					base.OnPropertyChanged(value, "LeftArrowOpacity");
				}
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0001F5BE File Offset: 0x0001D7BE
		// (set) Token: 0x06000B2B RID: 2859 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
		[Editor(false)]
		public bool IsTargetInvalid
		{
			get
			{
				return this._isTargetInvalid;
			}
			set
			{
				if (value != this._isTargetInvalid)
				{
					this._isTargetInvalid = value;
					base.OnPropertyChanged(value, "IsTargetInvalid");
					base.ApplyActionToAllChildrenRecursive(delegate(Widget child)
					{
						child.SetState(value ? "Invalid" : "Default");
					});
				}
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0001F61F File Offset: 0x0001D81F
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x0001F627 File Offset: 0x0001D827
		[Editor(false)]
		public double CrosshairAccuracy
		{
			get
			{
				return this._crosshairAccuracy;
			}
			set
			{
				if (value != this._crosshairAccuracy)
				{
					this._crosshairAccuracy = value;
					base.OnPropertyChanged(value, "CrosshairAccuracy");
				}
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0001F645 File Offset: 0x0001D845
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x0001F64D File Offset: 0x0001D84D
		[Editor(false)]
		public double CrosshairScale
		{
			get
			{
				return this._crosshairScale;
			}
			set
			{
				if (value != this._crosshairScale)
				{
					this._crosshairScale = value;
					base.OnPropertyChanged(value, "CrosshairScale");
				}
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0001F66B File Offset: 0x0001D86B
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x0001F673 File Offset: 0x0001D873
		[Editor(false)]
		public bool IsVictimDead
		{
			get
			{
				return this._isVictimDead;
			}
			set
			{
				if (value != this._isVictimDead)
				{
					this._isVictimDead = value;
					base.OnPropertyChanged(value, "IsVictimDead");
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0001F691 File Offset: 0x0001D891
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x0001F699 File Offset: 0x0001D899
		[Editor(false)]
		public bool IsHumanoidHeadshot
		{
			get
			{
				return this._isHumanoidHeadshot;
			}
			set
			{
				if (value != this._isHumanoidHeadshot)
				{
					this._isHumanoidHeadshot = value;
					base.OnPropertyChanged(value, "IsHumanoidHeadshot");
					this.ShowHeadshotMarkerChanged();
				}
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0001F6BD File Offset: 0x0001D8BD
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0001F6C5 File Offset: 0x0001D8C5
		[Editor(false)]
		public bool ShowHitMarker
		{
			get
			{
				return this._showHitMarker;
			}
			set
			{
				if (value != this._showHitMarker)
				{
					this._showHitMarker = value;
					base.OnPropertyChanged(value, "ShowHitMarker");
					this.ShowHitMarkerChanged();
				}
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0001F6E9 File Offset: 0x0001D8E9
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x0001F6F1 File Offset: 0x0001D8F1
		[Editor(false)]
		public BrushWidget LeftArrow
		{
			get
			{
				return this._leftArrow;
			}
			set
			{
				if (value != this._leftArrow)
				{
					this._leftArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "LeftArrow");
				}
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0001F70F File Offset: 0x0001D90F
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0001F717 File Offset: 0x0001D917
		[Editor(false)]
		public BrushWidget RightArrow
		{
			get
			{
				return this._rightArrow;
			}
			set
			{
				if (value != this._rightArrow)
				{
					this._rightArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "RightArrow");
				}
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0001F735 File Offset: 0x0001D935
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x0001F73D File Offset: 0x0001D93D
		[Editor(false)]
		public BrushWidget TopArrow
		{
			get
			{
				return this._topArrow;
			}
			set
			{
				if (value != this._topArrow)
				{
					this._topArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "TopArrow");
				}
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0001F75B File Offset: 0x0001D95B
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x0001F763 File Offset: 0x0001D963
		[Editor(false)]
		public BrushWidget BottomArrow
		{
			get
			{
				return this._bottomArrow;
			}
			set
			{
				if (value != this._bottomArrow)
				{
					this._bottomArrow = value;
					base.OnPropertyChanged<BrushWidget>(value, "BottomArrow");
				}
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0001F781 File Offset: 0x0001D981
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x0001F789 File Offset: 0x0001D989
		[Editor(false)]
		public BrushWidget HitMarker
		{
			get
			{
				return this._hitMarker;
			}
			set
			{
				if (value != this._hitMarker)
				{
					this._hitMarker = value;
					base.OnPropertyChanged<BrushWidget>(value, "HitMarker");
					this.HitMarkerUpdated();
				}
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0001F7AD File Offset: 0x0001D9AD
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x0001F7B5 File Offset: 0x0001D9B5
		[Editor(false)]
		public BrushWidget HeadshotMarker
		{
			get
			{
				return this._headshotMarker;
			}
			set
			{
				if (value != this._headshotMarker)
				{
					this._headshotMarker = value;
					base.OnPropertyChanged<BrushWidget>(value, "HeadshotMarker");
					this.HeadshotMarkerUpdated();
				}
			}
		}

		// Token: 0x04000507 RID: 1287
		private double _crosshairAccuracy;

		// Token: 0x04000508 RID: 1288
		private double _crosshairScale;

		// Token: 0x04000509 RID: 1289
		private bool _isTargetInvalid;

		// Token: 0x0400050A RID: 1290
		private double _topArrowOpacity;

		// Token: 0x0400050B RID: 1291
		private double _bottomArrowOpacity;

		// Token: 0x0400050C RID: 1292
		private double _rightArrowOpacity;

		// Token: 0x0400050D RID: 1293
		private double _leftArrowOpacity;

		// Token: 0x0400050E RID: 1294
		private bool _isVictimDead;

		// Token: 0x0400050F RID: 1295
		private bool _showHitMarker;

		// Token: 0x04000510 RID: 1296
		private bool _isHumanoidHeadshot;

		// Token: 0x04000511 RID: 1297
		private BrushWidget _leftArrow;

		// Token: 0x04000512 RID: 1298
		private BrushWidget _rightArrow;

		// Token: 0x04000513 RID: 1299
		private BrushWidget _topArrow;

		// Token: 0x04000514 RID: 1300
		private BrushWidget _bottomArrow;

		// Token: 0x04000515 RID: 1301
		private BrushWidget _hitMarker;

		// Token: 0x04000516 RID: 1302
		private BrushWidget _headshotMarker;
	}
}
