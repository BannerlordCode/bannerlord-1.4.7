using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x0200004A RID: 74
	public class MapTrackerCollectionVM : ViewModel
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x00011F54 File Offset: 0x00010154
		public MapTrackerCollectionVM()
		{
			this._mapTrackerProvider = new MapTrackerProvider();
			this.Trackers = new MBBindingList<MapTrackerItemVM>();
			foreach (MapTrackerItemVM mapTrackerItemVM in this._mapTrackerProvider.GetTrackers())
			{
				this.Trackers.Add(mapTrackerItemVM);
			}
			this._mapTrackerProvider.OnTrackerAddedOrRemoved += this.OnTrackerAddedOrRemoved;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00011FBE File Offset: 0x000101BE
		private void OnTrackerAddedOrRemoved(MapTrackerItemVM item, bool added)
		{
			if (added)
			{
				this.Trackers.Add(item);
				return;
			}
			this.Trackers.Remove(item);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00011FE0 File Offset: 0x000101E0
		public void Tick(float dt)
		{
			for (int i = 0; i < this.Trackers.Count; i++)
			{
				this.Trackers[i].RefreshBinding();
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00012014 File Offset: 0x00010214
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._mapTrackerProvider.OnTrackerAddedOrRemoved -= this.OnTrackerAddedOrRemoved;
			this.Trackers.ApplyActionOnAllItems(delegate(MapTrackerItemVM t)
			{
				t.OnFinalize();
			});
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00012068 File Offset: 0x00010268
		public void UpdateProperties()
		{
			this.Trackers.ApplyActionOnAllItems(delegate(MapTrackerItemVM t)
			{
				t.UpdateProperties();
			});
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00012094 File Offset: 0x00010294
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x0001209C File Offset: 0x0001029C
		public MBBindingList<MapTrackerItemVM> Trackers
		{
			get
			{
				return this._trackers;
			}
			set
			{
				if (value != this._trackers)
				{
					this._trackers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapTrackerItemVM>>(value, "Trackers");
				}
			}
		}

		// Token: 0x0400024F RID: 591
		private readonly MapTrackerProvider _mapTrackerProvider;

		// Token: 0x04000250 RID: 592
		private MBBindingList<MapTrackerItemVM> _trackers;
	}
}
