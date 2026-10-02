using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x0200005C RID: 92
	public class MapViewsContainer
	{
		// Token: 0x06000397 RID: 919 RVA: 0x0001C936 File Offset: 0x0001AB36
		public MapViewsContainer()
		{
			this.MapViews = new ObservableCollection<MapView>();
			this._mapViewsCopyCache = this.MapViews.ToList<MapView>();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001C95A File Offset: 0x0001AB5A
		public void Add(MapView mapView)
		{
			this.MapViews.Add(mapView);
			this._isViewListDirty = true;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001C96F File Offset: 0x0001AB6F
		public void Remove(MapView mapView)
		{
			this.MapViews.Remove(mapView);
			this._isViewListDirty = true;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001C985 File Offset: 0x0001AB85
		public bool Contains(MapView mapView)
		{
			return this.MapViews.Contains(mapView);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001C994 File Offset: 0x0001AB94
		public void Foreach(Action<MapView> action)
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized)
				{
					action(mapView);
				}
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
		public void ForeachReverse(Action<MapView> action)
		{
			List<MapView> mapViewsCopy = this.GetMapViewsCopy();
			for (int i = mapViewsCopy.Count - 1; i >= 0; i--)
			{
				if (!mapViewsCopy[i].IsFinalized)
				{
					action(mapViewsCopy[i]);
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001CA34 File Offset: 0x0001AC34
		public MapView ReturnFirstElementWithCondition(Func<MapView, bool> condition)
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized && condition(mapView))
				{
					return mapView;
				}
			}
			return null;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001CA98 File Offset: 0x0001AC98
		public T GetMapViewWithType<T>() where T : MapView
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized && mapView is T)
				{
					return mapView as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001CB10 File Offset: 0x0001AD10
		public TutorialContexts GetContextToChangeTo()
		{
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized)
				{
					TutorialContexts tutorialContext = mapView.GetTutorialContext();
					if (tutorialContext != TutorialContexts.MapWindow)
					{
						return tutorialContext;
					}
				}
			}
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001CB78 File Offset: 0x0001AD78
		public bool IsThereAnyViewIsEscaped()
		{
			return this.ReturnFirstElementWithCondition((MapView view) => view.IsEscaped()) != null;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001CBA4 File Offset: 0x0001ADA4
		public bool IsOpeningEscapeMenuOnFocusChangeAllowedForAll()
		{
			bool flag = true;
			foreach (MapView mapView in this.GetMapViewsCopy())
			{
				if (!mapView.IsFinalized)
				{
					flag &= mapView.IsOpeningEscapeMenuOnFocusChangeAllowed();
				}
			}
			return flag;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001CC04 File Offset: 0x0001AE04
		private List<MapView> GetMapViewsCopy()
		{
			if (this._isViewListDirty)
			{
				this._mapViewsCopyCache = this.MapViews.ToList<MapView>();
				this._isViewListDirty = false;
			}
			return this._mapViewsCopyCache;
		}

		// Token: 0x040001D8 RID: 472
		public readonly ObservableCollection<MapView> MapViews;

		// Token: 0x040001D9 RID: 473
		private List<MapView> _mapViewsCopyCache;

		// Token: 0x040001DA RID: 474
		private bool _isViewListDirty;
	}
}
