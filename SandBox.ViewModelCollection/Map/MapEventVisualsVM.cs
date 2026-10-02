using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map
{
	// Token: 0x02000045 RID: 69
	public class MapEventVisualsVM : ViewModel
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x00011A21 File Offset: 0x0000FC21
		public MapEventVisualsVM(Camera mapCamera)
		{
			this._mapCamera = mapCamera;
			this.MapEvents = new MBBindingList<MapEventVisualItemVM>();
			this.UpdateMapEventsAuxPredicate = new TWParallel.ParallelForAuxPredicate(this.UpdateMapEventsAux);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00011A58 File Offset: 0x0000FC58
		private void UpdateMapEventsAux(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.MapEvents[i].ParallelUpdatePosition();
				this.MapEvents[i].DetermineIsVisibleOnMap();
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011A94 File Offset: 0x0000FC94
		public void Update(float dt)
		{
			TWParallel.For(0, this.MapEvents.Count, this.UpdateMapEventsAuxPredicate, 16);
			for (int i = 0; i < this.MapEvents.Count; i++)
			{
				this.MapEvents[i].UpdateBindingProperties();
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00011AE1 File Offset: 0x0000FCE1
		public void OnMapEventVisibilityChanged(MapEvent mapEvent)
		{
			if (this._eventToVisualMap.ContainsKey(mapEvent))
			{
				this._eventToVisualMap[mapEvent].UpdateProperties();
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011B04 File Offset: 0x0000FD04
		public void OnMapEventStarted(MapEvent mapEvent)
		{
			if (!this._eventToVisualMap.ContainsKey(mapEvent))
			{
				if (!this.IsMapEventSettlementRelated(mapEvent))
				{
					MapEventVisualItemVM mapEventVisualItemVM = new MapEventVisualItemVM(this._mapCamera, mapEvent);
					this._eventToVisualMap.Add(mapEvent, mapEventVisualItemVM);
					this.MapEvents.Add(mapEventVisualItemVM);
					mapEventVisualItemVM.UpdateProperties();
				}
				return;
			}
			if (!this.IsMapEventSettlementRelated(mapEvent))
			{
				this._eventToVisualMap[mapEvent].UpdateProperties();
				return;
			}
			MapEventVisualItemVM mapEventVisualItemVM2 = this._eventToVisualMap[mapEvent];
			this.MapEvents.Remove(mapEventVisualItemVM2);
			this._eventToVisualMap.Remove(mapEvent);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00011B98 File Offset: 0x0000FD98
		public void OnMapEventEnded(MapEvent mapEvent)
		{
			if (this._eventToVisualMap.ContainsKey(mapEvent))
			{
				MapEventVisualItemVM mapEventVisualItemVM = this._eventToVisualMap[mapEvent];
				this.MapEvents.Remove(mapEventVisualItemVM);
				this._eventToVisualMap.Remove(mapEvent);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00011BDA File Offset: 0x0000FDDA
		private bool IsMapEventSettlementRelated(MapEvent mapEvent)
		{
			return mapEvent.MapEventSettlement != null;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00011BE5 File Offset: 0x0000FDE5
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x00011BED File Offset: 0x0000FDED
		public MBBindingList<MapEventVisualItemVM> MapEvents
		{
			get
			{
				return this._mapEvents;
			}
			set
			{
				if (this._mapEvents != value)
				{
					this._mapEvents = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapEventVisualItemVM>>(value, "MapEvents");
				}
			}
		}

		// Token: 0x0400023E RID: 574
		private readonly Camera _mapCamera;

		// Token: 0x0400023F RID: 575
		private readonly Dictionary<MapEvent, MapEventVisualItemVM> _eventToVisualMap = new Dictionary<MapEvent, MapEventVisualItemVM>();

		// Token: 0x04000240 RID: 576
		private readonly TWParallel.ParallelForAuxPredicate UpdateMapEventsAuxPredicate;

		// Token: 0x04000241 RID: 577
		private MBBindingList<MapEventVisualItemVM> _mapEvents;
	}
}
