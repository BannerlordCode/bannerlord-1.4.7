using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F7 RID: 247
	public class NameMarkerScreenWidget : Widget
	{
		// Token: 0x06000CE3 RID: 3299 RVA: 0x00023193 File Offset: 0x00021393
		public NameMarkerScreenWidget(UIContext context)
			: base(context)
		{
			this._markers = new List<NameMarkerListPanel>();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000231A8 File Offset: 0x000213A8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = (this.IsMarkersEnabled ? this.TargetAlphaValue : 0f);
			float num2 = MathF.Clamp(dt * 10f, 0f, 1f);
			base.AlphaFactor = Mathf.Lerp(base.AlphaFactor, num, num2);
			bool flag = this._markers.Count > 0;
			for (int i = 0; i < this._markers.Count; i++)
			{
				this._markers[i].Update(dt);
				flag &= this._markers[i].TypeVisualWidget.AlphaFactor > 0f;
			}
			if (flag)
			{
				this._markers.Sort((NameMarkerListPanel m1, NameMarkerListPanel m2) => m1.Rect.Left.CompareTo(m2.Rect.Left));
				for (int j = 0; j < this._markers.Count; j++)
				{
					int num3 = j + 1;
					while (num3 < this._markers.Count && this._markers[num3].Rect.Left - this._markers[j].Rect.Left <= this._markers[j].Rect.Width)
					{
						if (this._markers[j].Rect.IsOverlapping(this._markers[num3].Rect))
						{
							this._markers[num3].ScaledPositionXOffset += this._markers[j].Rect.Right - this._markers[num3].Rect.Left;
							this._markers[num3].UpdateRectangle();
						}
						num3++;
					}
				}
				NameMarkerListPanel nameMarkerListPanel = null;
				float num4 = 3600f;
				for (int k = 0; k < this._markers.Count; k++)
				{
					if (this._markers[k].IsInScreenBoundaries)
					{
						NameMarkerListPanel nameMarkerListPanel2 = this._markers[k];
						float num5 = base.EventManager.PageSize.X / 2f;
						float num6 = base.EventManager.PageSize.Y / 2f;
						float num7 = Mathf.Abs(num5 - nameMarkerListPanel2.Rect.CenterX);
						float num8 = Mathf.Abs(num6 - nameMarkerListPanel2.Rect.CenterY);
						float num9 = num7 * num7 + num8 * num8;
						if (num9 < num4)
						{
							num4 = num9;
							nameMarkerListPanel = nameMarkerListPanel2;
						}
					}
				}
				if (nameMarkerListPanel != this._lastFocusedWidget)
				{
					if (this._lastFocusedWidget != null)
					{
						this._lastFocusedWidget.IsFocused = false;
					}
					this._lastFocusedWidget = nameMarkerListPanel;
					if (this._lastFocusedWidget != null)
					{
						this._lastFocusedWidget.IsFocused = true;
					}
				}
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0002348C File Offset: 0x0002168C
		private void OnMarkersChanged(Widget widget, string eventName, object[] args)
		{
			NameMarkerListPanel nameMarkerListPanel;
			if (args.Length == 1 && (nameMarkerListPanel = args[0] as NameMarkerListPanel) != null)
			{
				if (eventName == "ItemAdd")
				{
					this._markers.Add(nameMarkerListPanel);
					return;
				}
				if (eventName == "ItemRemove")
				{
					this._markers.Remove(nameMarkerListPanel);
				}
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000234DF File Offset: 0x000216DF
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x000234E7 File Offset: 0x000216E7
		public bool IsMarkersEnabled
		{
			get
			{
				return this._isMarkersEnabled;
			}
			set
			{
				if (this._isMarkersEnabled != value)
				{
					this._isMarkersEnabled = value;
					base.OnPropertyChanged(value, "IsMarkersEnabled");
				}
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00023505 File Offset: 0x00021705
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0002350D File Offset: 0x0002170D
		public float TargetAlphaValue
		{
			get
			{
				return this._targetAlphaValue;
			}
			set
			{
				if (this._targetAlphaValue != value)
				{
					this._targetAlphaValue = value;
					base.OnPropertyChanged(value, "TargetAlphaValue");
				}
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0002352B File Offset: 0x0002172B
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x00023534 File Offset: 0x00021734
		[Editor(false)]
		public Widget MarkersContainer
		{
			get
			{
				return this._markersContainer;
			}
			set
			{
				if (value != this._markersContainer)
				{
					if (this._markersContainer != null)
					{
						this._markersContainer.EventFire += this.OnMarkersChanged;
					}
					this._markersContainer = value;
					if (this._markersContainer != null)
					{
						this._markersContainer.EventFire += this.OnMarkersChanged;
					}
					base.OnPropertyChanged<Widget>(value, "MarkersContainer");
				}
			}
		}

		// Token: 0x040005D4 RID: 1492
		private const float MinDistanceToFocusSquared = 3600f;

		// Token: 0x040005D5 RID: 1493
		private List<NameMarkerListPanel> _markers;

		// Token: 0x040005D6 RID: 1494
		private NameMarkerListPanel _lastFocusedWidget;

		// Token: 0x040005D7 RID: 1495
		private bool _isMarkersEnabled;

		// Token: 0x040005D8 RID: 1496
		private float _targetAlphaValue;

		// Token: 0x040005D9 RID: 1497
		private Widget _markersContainer;
	}
}
