using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F2 RID: 242
	public class OrderOfBattleScreenWidget : Widget
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00021D15 File Offset: 0x0001FF15
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x00021D1D File Offset: 0x0001FF1D
		public float AlphaChangeDuration { get; set; } = 0.15f;

		// Token: 0x06000C66 RID: 3174 RVA: 0x00021D26 File Offset: 0x0001FF26
		public OrderOfBattleScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00021D5C File Offset: 0x0001FF5C
		protected override void OnLateUpdate(float dt)
		{
			this.CanToggleHeroSelection = base.EventManager.DraggedWidget == null;
			if (this._isTransitioning)
			{
				if (this._alphaChangeTimeElapsed < this.AlphaChangeDuration)
				{
					this._currentAlpha = MathF.Lerp(this._initialAlpha, this._targetAlpha, this._alphaChangeTimeElapsed / this.AlphaChangeDuration, 1E-05f);
					ListPanel leftSideFormations = this.LeftSideFormations;
					if (leftSideFormations != null)
					{
						leftSideFormations.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					ListPanel rightSideFormations = this.RightSideFormations;
					if (rightSideFormations != null)
					{
						rightSideFormations.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					ListPanel captainPool = this.CaptainPool;
					if (captainPool != null)
					{
						captainPool.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					Widget markers = this.Markers;
					if (markers != null)
					{
						markers.SetGlobalAlphaRecursively(this._currentAlpha);
					}
					this._alphaChangeTimeElapsed += dt;
					return;
				}
				this._currentAlpha = this._targetAlpha;
				ListPanel leftSideFormations2 = this.LeftSideFormations;
				if (leftSideFormations2 != null)
				{
					leftSideFormations2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				ListPanel rightSideFormations2 = this.RightSideFormations;
				if (rightSideFormations2 != null)
				{
					rightSideFormations2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				ListPanel captainPool2 = this.CaptainPool;
				if (captainPool2 != null)
				{
					captainPool2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				Widget markers2 = this.Markers;
				if (markers2 != null)
				{
					markers2.SetGlobalAlphaRecursively(this._currentAlpha);
				}
				this._isTransitioning = false;
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00021E9C File Offset: 0x0002009C
		protected void OnCameraControlsEnabledChanged()
		{
			this._alphaChangeTimeElapsed = 0f;
			this._targetAlpha = (this.AreCameraControlsEnabled ? this.CameraEnabledAlpha : 1f);
			this._initialAlpha = this._currentAlpha;
			this._isTransitioning = true;
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00021ED7 File Offset: 0x000200D7
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x00021EDF File Offset: 0x000200DF
		[Editor(false)]
		public bool AreCameraControlsEnabled
		{
			get
			{
				return this._areCameraControlsEnabled;
			}
			set
			{
				if (value != this._areCameraControlsEnabled)
				{
					this._areCameraControlsEnabled = value;
					base.OnPropertyChanged(value, "AreCameraControlsEnabled");
					this.OnCameraControlsEnabledChanged();
				}
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00021F03 File Offset: 0x00020103
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00021F0B File Offset: 0x0002010B
		[Editor(false)]
		public float CameraEnabledAlpha
		{
			get
			{
				return this._cameraEnabledAlpha;
			}
			set
			{
				if (value != this._cameraEnabledAlpha)
				{
					this._cameraEnabledAlpha = value;
					base.OnPropertyChanged(value, "CameraEnabledAlpha");
				}
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00021F29 File Offset: 0x00020129
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x00021F31 File Offset: 0x00020131
		[Editor(false)]
		public ListPanel LeftSideFormations
		{
			get
			{
				return this._leftSideFormations;
			}
			set
			{
				if (value != this._leftSideFormations)
				{
					this._leftSideFormations = value;
					base.OnPropertyChanged<ListPanel>(value, "LeftSideFormations");
				}
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00021F4F File Offset: 0x0002014F
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x00021F57 File Offset: 0x00020157
		[Editor(false)]
		public ListPanel RightSideFormations
		{
			get
			{
				return this._rightSideFormations;
			}
			set
			{
				if (value != this._rightSideFormations)
				{
					this._rightSideFormations = value;
					base.OnPropertyChanged<ListPanel>(value, "RightSideFormations");
				}
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00021F75 File Offset: 0x00020175
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x00021F7D File Offset: 0x0002017D
		[Editor(false)]
		public ListPanel CaptainPool
		{
			get
			{
				return this._captainPool;
			}
			set
			{
				if (value != this._captainPool)
				{
					this._captainPool = value;
					base.OnPropertyChanged<ListPanel>(value, "CaptainPool");
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00021F9B File Offset: 0x0002019B
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x00021FA3 File Offset: 0x000201A3
		[Editor(false)]
		public Widget Markers
		{
			get
			{
				return this._markers;
			}
			set
			{
				if (value != this._markers)
				{
					this._markers = value;
					base.OnPropertyChanged<Widget>(value, "Markers");
				}
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00021FC1 File Offset: 0x000201C1
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x00021FC9 File Offset: 0x000201C9
		[Editor(false)]
		public bool CanToggleHeroSelection
		{
			get
			{
				return this._canToggleHeroSelection;
			}
			set
			{
				if (value != this._canToggleHeroSelection)
				{
					this._canToggleHeroSelection = value;
					base.OnPropertyChanged(value, "CanToggleHeroSelection");
				}
			}
		}

		// Token: 0x04000595 RID: 1429
		private float _alphaChangeTimeElapsed;

		// Token: 0x04000596 RID: 1430
		private float _initialAlpha = 1f;

		// Token: 0x04000597 RID: 1431
		private float _targetAlpha;

		// Token: 0x04000598 RID: 1432
		private float _currentAlpha = 1f;

		// Token: 0x04000599 RID: 1433
		private bool _isTransitioning;

		// Token: 0x0400059A RID: 1434
		private bool _areCameraControlsEnabled;

		// Token: 0x0400059B RID: 1435
		private float _cameraEnabledAlpha = 0.2f;

		// Token: 0x0400059C RID: 1436
		private ListPanel _leftSideFormations;

		// Token: 0x0400059D RID: 1437
		private ListPanel _rightSideFormations;

		// Token: 0x0400059E RID: 1438
		private ListPanel _captainPool;

		// Token: 0x0400059F RID: 1439
		private Widget _markers;

		// Token: 0x040005A0 RID: 1440
		private bool _canToggleHeroSelection;
	}
}
