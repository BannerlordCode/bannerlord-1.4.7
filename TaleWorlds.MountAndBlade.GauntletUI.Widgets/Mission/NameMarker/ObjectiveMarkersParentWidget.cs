using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F8 RID: 248
	public class ObjectiveMarkersParentWidget : Widget
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0002359B File Offset: 0x0002179B
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x000235A3 File Offset: 0x000217A3
		public float MinDistanceToFocus { get; set; }

		// Token: 0x06000CEE RID: 3310 RVA: 0x000235AC File Offset: 0x000217AC
		public ObjectiveMarkersParentWidget(UIContext context)
			: base(context)
		{
			this._markers = new List<ObjectiveMarkerWidget>();
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x000235C0 File Offset: 0x000217C0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = (this.IsMarkersEnabled ? this.TargetAlphaValue : 0f);
			float num2 = MathF.Clamp(dt * 10f, 0f, 1f);
			base.AlphaFactor = Mathf.Lerp(base.AlphaFactor, num, num2);
			base.Children.Sort(new ObjectiveMarkersParentWidget.MarkerRenderOrderComparer());
			this._markers.Sort(new ObjectiveMarkersParentWidget.MarkerPositionComparer());
			this.UpdateMarkerCombinations();
			this.UpdateMarkerFocus();
			for (int i = 0; i < this._markers.Count; i++)
			{
				this._markers[i].Update(dt);
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00023668 File Offset: 0x00021868
		private void UpdateMarkerCombinations()
		{
			for (int i = 0; i < this._markers.Count; i++)
			{
				this._markers[i].CombinedSiblingsCount = 0;
				this._markers[i].IsMainCombinationMarker = false;
			}
			for (int j = 0; j < this._markers.Count; j++)
			{
				ObjectiveMarkerWidget objectiveMarkerWidget = this._markers[j];
				if (!objectiveMarkerWidget.IsCombinedWithOtherMarkers && objectiveMarkerWidget.IsMarkerActive && objectiveMarkerWidget.IsMarkerEnabled)
				{
					Vec2 vec;
					List<ObjectiveMarkerWidget> list = this.FindClosestMarkersToCombine(objectiveMarkerWidget, out vec);
					objectiveMarkerWidget.CombinedSiblingsCount = list.Count;
					objectiveMarkerWidget.IsMainCombinationMarker = list.Count > 0;
					if (objectiveMarkerWidget.IsCombinedWithOtherMarkers)
					{
						objectiveMarkerWidget.CombinedAveragePosition = vec;
					}
					for (int k = 0; k < list.Count; k++)
					{
						list[k].CombinedSiblingsCount = list.Count;
						list[k].CombinedAveragePosition = vec;
						list[k].IsMainCombinationMarker = false;
					}
				}
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00023770 File Offset: 0x00021970
		private void UpdateMarkerFocus()
		{
			float num;
			ObjectiveMarkerWidget objectiveMarkerWidget = this.FindClosestMarkerToFocus(base.EventManager.PageSize * 0.5f, out num);
			if (objectiveMarkerWidget != this._lastFocusedWidget)
			{
				if (this._lastFocusedWidget != null)
				{
					this._lastFocusedWidget.IsFocused = false;
				}
				this._lastFocusedWidget = objectiveMarkerWidget;
				if (this._lastFocusedWidget != null)
				{
					this._lastFocusedWidget.IsFocused = true;
				}
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x000237D8 File Offset: 0x000219D8
		private ObjectiveMarkerWidget FindClosestMarkerToFocus(Vec2 screenPosition, out float distanceSquared)
		{
			ObjectiveMarkerWidget objectiveMarkerWidget = null;
			distanceSquared = this.MinDistanceToFocus * this.MinDistanceToFocus;
			for (int i = 0; i < this._markers.Count; i++)
			{
				ObjectiveMarkerWidget objectiveMarkerWidget2 = this._markers[i];
				if (objectiveMarkerWidget2.IsInScreenBoundaries)
				{
					float num = objectiveMarkerWidget2.Position.DistanceSquared(screenPosition);
					if (num < distanceSquared)
					{
						distanceSquared = num;
						objectiveMarkerWidget = objectiveMarkerWidget2;
					}
				}
			}
			return objectiveMarkerWidget;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00023840 File Offset: 0x00021A40
		private List<ObjectiveMarkerWidget> FindClosestMarkersToCombine(ObjectiveMarkerWidget marker, out Vec2 averageScreenPosition)
		{
			List<ObjectiveMarkerWidget> list = new List<ObjectiveMarkerWidget>();
			averageScreenPosition = marker.Position;
			for (int i = 0; i < this._markers.Count; i++)
			{
				if (this._markers[i] != marker && this._markers[i].IsInScreenBoundaries && !this._markers[i].IsCombinedWithOtherMarkers && this._markers[i].IsMarkerActive && this._markers[i].IsMarkerEnabled && marker.Position.Distance(this._markers[i].Position) < this.MaxDistanceToCombineMarkers)
				{
					averageScreenPosition += this._markers[i].Position;
					list.Add(this._markers[i]);
				}
			}
			averageScreenPosition /= (float)(list.Count + 1);
			return list;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00023958 File Offset: 0x00021B58
		private void OnMarkersChanged(Widget widget, string eventName, object[] args)
		{
			ObjectiveMarkerWidget objectiveMarkerWidget;
			if (args.Length == 1 && (objectiveMarkerWidget = args[0] as ObjectiveMarkerWidget) != null)
			{
				if (eventName == "ItemAdd")
				{
					this._markers.Add(objectiveMarkerWidget);
					return;
				}
				if (eventName == "ItemRemove")
				{
					this._markers.Remove(objectiveMarkerWidget);
				}
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x000239AB File Offset: 0x00021BAB
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x000239B3 File Offset: 0x00021BB3
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

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x000239D1 File Offset: 0x00021BD1
		// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x000239D9 File Offset: 0x00021BD9
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

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x000239F7 File Offset: 0x00021BF7
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x000239FF File Offset: 0x00021BFF
		public float MaxDistanceToCombineMarkers
		{
			get
			{
				return this._maxDistanceToCombineMarkers;
			}
			set
			{
				if (this._maxDistanceToCombineMarkers != value)
				{
					this._maxDistanceToCombineMarkers = value;
					base.OnPropertyChanged(value, "MaxDistanceToCombineMarkers");
				}
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00023A1D File Offset: 0x00021C1D
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00023A28 File Offset: 0x00021C28
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

		// Token: 0x040005DB RID: 1499
		private List<ObjectiveMarkerWidget> _markers;

		// Token: 0x040005DC RID: 1500
		private ObjectiveMarkerWidget _lastFocusedWidget;

		// Token: 0x040005DD RID: 1501
		private bool _isMarkersEnabled;

		// Token: 0x040005DE RID: 1502
		private float _targetAlphaValue;

		// Token: 0x040005DF RID: 1503
		private float _maxDistanceToCombineMarkers;

		// Token: 0x040005E0 RID: 1504
		private Widget _markersContainer;

		// Token: 0x020001C2 RID: 450
		private class MarkerPositionComparer : IComparer<ObjectiveMarkerWidget>
		{
			// Token: 0x06001551 RID: 5457 RVA: 0x00039D40 File Offset: 0x00037F40
			public int Compare(ObjectiveMarkerWidget x, ObjectiveMarkerWidget y)
			{
				return x.Position.x.CompareTo(y.Position.x);
			}
		}

		// Token: 0x020001C3 RID: 451
		private class MarkerRenderOrderComparer : IComparer<Widget>
		{
			// Token: 0x06001553 RID: 5459 RVA: 0x00039D74 File Offset: 0x00037F74
			public int Compare(Widget x, Widget y)
			{
				ObjectiveMarkerWidget objectiveMarkerWidget = x as ObjectiveMarkerWidget;
				ObjectiveMarkerWidget objectiveMarkerWidget2 = y as ObjectiveMarkerWidget;
				int num = (objectiveMarkerWidget == null).CompareTo(objectiveMarkerWidget2 == null);
				if (num != 0)
				{
					return num;
				}
				int num2 = objectiveMarkerWidget.IsMainCombinationMarker.CompareTo(objectiveMarkerWidget2.IsMainCombinationMarker);
				if (num2 != 0)
				{
					return num2;
				}
				int num3 = objectiveMarkerWidget.CombinedSiblingsCount.CompareTo(objectiveMarkerWidget2.CombinedSiblingsCount);
				if (num3 != 0)
				{
					return num3;
				}
				return objectiveMarkerWidget.Position.x.CompareTo(objectiveMarkerWidget2.Position.x);
			}
		}
	}
}
