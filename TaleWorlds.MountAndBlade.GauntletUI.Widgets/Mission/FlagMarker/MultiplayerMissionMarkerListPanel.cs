using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.FlagMarker
{
	// Token: 0x020000FF RID: 255
	public class MultiplayerMissionMarkerListPanel : ListPanel
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00025398 File Offset: 0x00023598
		// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x000253A0 File Offset: 0x000235A0
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x000253A9 File Offset: 0x000235A9
		// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x000253B1 File Offset: 0x000235B1
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x000253BA File Offset: 0x000235BA
		// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x000253C2 File Offset: 0x000235C2
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000253CB File Offset: 0x000235CB
		public MultiplayerMissionMarkerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000253F8 File Offset: 0x000235F8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = MathF.Clamp(dt * 12f, 0f, 1f);
			if (!this._initialized)
			{
				this.SetInitialAlphaValuesOnCreation();
				this._initialized = true;
			}
			if (this.IsMarkerEnabled)
			{
				List<Widget> allChildrenAndThisRecursive = base.GetAllChildrenAndThisRecursive();
				for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
				{
					Widget widget = allChildrenAndThisRecursive[i];
					bool flag;
					if (widget != this && widget != this._activeWidget)
					{
						Widget activeWidget = this._activeWidget;
						flag = activeWidget != null && activeWidget.CheckIsMyChildRecursive(widget);
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						float distanceRelatedAlphaTarget = this.GetDistanceRelatedAlphaTarget(this.Distance);
						if (widget == this.SpawnFlagIconWidget)
						{
							widget.SetAlpha(this.IsSpawnFlag ? this.LocalLerp(widget.AlphaFactor, distanceRelatedAlphaTarget, num) : 0f);
						}
						else
						{
							widget.SetAlpha(this.LocalLerp(widget.AlphaFactor, distanceRelatedAlphaTarget, num));
						}
						if (widget != this.RemovalTimeVisiblityWidget)
						{
							widget.IsVisible = (double)widget.AlphaFactor > 0.05;
						}
					}
					else if (widget != this.RemovalTimeVisiblityWidget)
					{
						widget.IsVisible = false;
					}
				}
			}
			else
			{
				List<Widget> allChildrenAndThisRecursive2 = base.GetAllChildrenAndThisRecursive();
				for (int j = 0; j < allChildrenAndThisRecursive2.Count; j++)
				{
					Widget widget2 = allChildrenAndThisRecursive2[j];
					bool flag2;
					if (widget2 != this && widget2 != this._activeWidget)
					{
						Widget activeWidget2 = this._activeWidget;
						flag2 = activeWidget2 != null && activeWidget2.CheckIsMyChildRecursive(widget2);
					}
					else
					{
						flag2 = true;
					}
					if (flag2)
					{
						if (widget2 == this.SpawnFlagIconWidget)
						{
							widget2.SetAlpha(this.IsSpawnFlag ? this.LocalLerp(widget2.AlphaFactor, 0f, num) : 0f);
						}
						else
						{
							widget2.SetAlpha(this.LocalLerp(widget2.AlphaFactor, 0f, num));
						}
						if (widget2 != this.RemovalTimeVisiblityWidget)
						{
							widget2.IsVisible = (double)widget2.AlphaFactor > 0.05;
						}
					}
					else if (widget2 != this.RemovalTimeVisiblityWidget)
					{
						widget2.IsVisible = false;
					}
				}
			}
			Widget activeWidget3 = this._activeWidget;
			if (activeWidget3 != null && activeWidget3.IsVisible)
			{
				if (this._activeMarkerType == MultiplayerMissionMarkerListPanel.MissionMarkerType.Flag)
				{
					float x = base.Context.EventManager.PageSize.X;
					float y = base.Context.EventManager.PageSize.Y;
					base.ScaledPositionXOffset = MathF.Clamp(this.Position.x - base.Size.X / 2f, 10f, x - base.Size.X - 10f);
					base.ScaledPositionYOffset = MathF.Clamp(this.Position.y - base.Size.Y / 2f, 10f, y - base.Size.Y - 10f);
					return;
				}
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00025710 File Offset: 0x00023910
		private float GetDistanceRelatedAlphaTarget(int distance)
		{
			if ((float)distance > this.FarDistanceCutoff)
			{
				return this.FarAlphaTarget;
			}
			if ((float)distance <= this.FarDistanceCutoff && (float)distance >= this.CloseDistanceCutoff)
			{
				float num = (float)Math.Pow((double)(((float)distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
				return MathF.Clamp(MathF.Lerp(1f, this.FarAlphaTarget, num, 1E-05f), this.FarAlphaTarget, 1f);
			}
			return 1f;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00025798 File Offset: 0x00023998
		private void SetInitialAlphaValuesOnCreation()
		{
			if (this.IsMarkerEnabled)
			{
				List<Widget> allChildrenAndThisRecursive = base.GetAllChildrenAndThisRecursive();
				for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
				{
					Widget widget = allChildrenAndThisRecursive[i];
					bool flag;
					if (widget != this && widget != this._activeWidget)
					{
						Widget activeWidget = this._activeWidget;
						flag = activeWidget != null && activeWidget.CheckIsMyChildRecursive(widget);
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						if (widget == this.SpawnFlagIconWidget)
						{
							widget.SetAlpha((float)(this.IsSpawnFlag ? 1 : 0));
						}
						else
						{
							widget.SetAlpha(1f);
						}
						if (widget != this.RemovalTimeVisiblityWidget)
						{
							widget.IsVisible = (double)widget.AlphaFactor > 0.05;
						}
					}
					else if (widget != this.RemovalTimeVisiblityWidget)
					{
						widget.IsVisible = false;
					}
				}
				return;
			}
			List<Widget> allChildrenAndThisRecursive2 = base.GetAllChildrenAndThisRecursive();
			for (int j = 0; j < allChildrenAndThisRecursive2.Count; j++)
			{
				Widget widget2 = allChildrenAndThisRecursive2[j];
				bool flag2;
				if (widget2 != this && widget2 != this._activeWidget)
				{
					Widget activeWidget2 = this._activeWidget;
					flag2 = activeWidget2 != null && activeWidget2.CheckIsMyChildRecursive(widget2);
				}
				else
				{
					flag2 = true;
				}
				if (flag2)
				{
					widget2.SetAlpha(0f);
					if (widget2 != this.RemovalTimeVisiblityWidget)
					{
						widget2.IsVisible = false;
					}
				}
				else if (widget2 != this.RemovalTimeVisiblityWidget)
				{
					widget2.IsVisible = false;
				}
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000258D8 File Offset: 0x00023AD8
		private float LocalLerp(float start, float end, float delta)
		{
			if (Math.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000258F4 File Offset: 0x00023AF4
		private void MarkerTypeUpdated()
		{
			this._activeMarkerType = (MultiplayerMissionMarkerListPanel.MissionMarkerType)this.MarkerType;
			switch (this._activeMarkerType)
			{
			case MultiplayerMissionMarkerListPanel.MissionMarkerType.Flag:
				this._activeWidget = this.FlagWidget;
				return;
			case MultiplayerMissionMarkerListPanel.MissionMarkerType.Peer:
				this._activeWidget = this.PeerWidget;
				return;
			case MultiplayerMissionMarkerListPanel.MissionMarkerType.SiegeEngine:
				this._activeWidget = this.SiegeEngineWidget;
				return;
			default:
				return;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0002594D File Offset: 0x00023B4D
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x00025955 File Offset: 0x00023B55
		public Widget FlagWidget
		{
			get
			{
				return this._flagWidget;
			}
			set
			{
				if (this._flagWidget != value)
				{
					this._flagWidget = value;
					base.OnPropertyChanged<Widget>(value, "FlagWidget");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00025979 File Offset: 0x00023B79
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x00025981 File Offset: 0x00023B81
		public Widget RemovalTimeVisiblityWidget
		{
			get
			{
				return this._removalTimeVisiblityWidget;
			}
			set
			{
				if (this._removalTimeVisiblityWidget != value)
				{
					this._removalTimeVisiblityWidget = value;
					base.OnPropertyChanged<Widget>(value, "RemovalTimeVisiblityWidget");
				}
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0002599F File Offset: 0x00023B9F
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x000259A7 File Offset: 0x00023BA7
		public Widget SpawnFlagIconWidget
		{
			get
			{
				return this._spawnFlagIconWidget;
			}
			set
			{
				if (this._spawnFlagIconWidget != value)
				{
					this._spawnFlagIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "SpawnFlagIconWidget");
				}
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x000259C5 File Offset: 0x00023BC5
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x000259CD File Offset: 0x00023BCD
		public Widget PeerWidget
		{
			get
			{
				return this._peerWidget;
			}
			set
			{
				if (this._peerWidget != value)
				{
					this._peerWidget = value;
					base.OnPropertyChanged<Widget>(value, "PeerWidget");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x000259F1 File Offset: 0x00023BF1
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x000259F9 File Offset: 0x00023BF9
		public Widget SiegeEngineWidget
		{
			get
			{
				return this._siegeEngineWidget;
			}
			set
			{
				if (value != this._siegeEngineWidget)
				{
					this._siegeEngineWidget = value;
					base.OnPropertyChanged<Widget>(value, "SiegeEngineWidget");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00025A1D File Offset: 0x00023C1D
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x00025A25 File Offset: 0x00023C25
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00025A48 File Offset: 0x00023C48
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x00025A50 File Offset: 0x00023C50
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value)
				{
					this._distance = value;
					base.OnPropertyChanged(value, "Distance");
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00025A6E File Offset: 0x00023C6E
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x00025A76 File Offset: 0x00023C76
		public bool IsMarkerEnabled
		{
			get
			{
				return this._isMarkerEnabled;
			}
			set
			{
				if (this._isMarkerEnabled != value)
				{
					this._isMarkerEnabled = value;
					base.OnPropertyChanged(value, "IsMarkerEnabled");
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00025A94 File Offset: 0x00023C94
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x00025A9C File Offset: 0x00023C9C
		public bool IsSpawnFlag
		{
			get
			{
				return this._isSpawnFlag;
			}
			set
			{
				if (this._isSpawnFlag != value)
				{
					this._isSpawnFlag = value;
					base.OnPropertyChanged(value, "IsSpawnFlag");
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00025ABA File Offset: 0x00023CBA
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x00025AC2 File Offset: 0x00023CC2
		public int MarkerType
		{
			get
			{
				return this._markerType;
			}
			set
			{
				if (this._markerType != value)
				{
					this._markerType = value;
					base.OnPropertyChanged(value, "MarkerType");
					this.MarkerTypeUpdated();
				}
			}
		}

		// Token: 0x0400062A RID: 1578
		private const int FlagMarkerEdgeMargin = 10;

		// Token: 0x0400062E RID: 1582
		private MultiplayerMissionMarkerListPanel.MissionMarkerType _activeMarkerType;

		// Token: 0x0400062F RID: 1583
		private Widget _activeWidget;

		// Token: 0x04000630 RID: 1584
		private bool _initialized;

		// Token: 0x04000631 RID: 1585
		private int _distance;

		// Token: 0x04000632 RID: 1586
		private Widget _flagWidget;

		// Token: 0x04000633 RID: 1587
		private Widget _peerWidget;

		// Token: 0x04000634 RID: 1588
		private Widget _siegeEngineWidget;

		// Token: 0x04000635 RID: 1589
		private Widget _spawnFlagIconWidget;

		// Token: 0x04000636 RID: 1590
		private Vec2 _position;

		// Token: 0x04000637 RID: 1591
		private bool _isMarkerEnabled;

		// Token: 0x04000638 RID: 1592
		private bool _isSpawnFlag;

		// Token: 0x04000639 RID: 1593
		private int _markerType;

		// Token: 0x0400063A RID: 1594
		private Widget _removalTimeVisiblityWidget;

		// Token: 0x020001C5 RID: 453
		public enum MissionMarkerType
		{
			// Token: 0x04000A16 RID: 2582
			Flag,
			// Token: 0x04000A17 RID: 2583
			Peer,
			// Token: 0x04000A18 RID: 2584
			SiegeEngine
		}
	}
}
